using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.IO;
using UnityEngine;
[System.Serializable]
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct BAOC_TimeNumber
{
    [MarshalAs(UnmanagedType.LPStr)]
    public string name;

    [MarshalAs(UnmanagedType.U4)]
    public ulong number;

    public BAOC_TimeNumber(string name, ulong number)
    {
        this.name = name;
        this.number = number;
    }
}

[System.Serializable]
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct BAOC_TimeData
{
    #region marshal
    [MarshalAs(UnmanagedType.U4)]
    public ulong seconds;

    [MarshalAs(UnmanagedType.U4)]
    public ulong minutes;

    [MarshalAs(UnmanagedType.U4)]
    public ulong hours;

    [MarshalAs(UnmanagedType.U4)]
    public ulong days;

    [MarshalAs(UnmanagedType.U4)]
    public ulong weeks;

    [MarshalAs(UnmanagedType.U4)]
    public ulong months;

    [MarshalAs(UnmanagedType.U4)]
    public ulong years;


    [MarshalAs(UnmanagedType.LPStr)]
    public string formmated;

    [MarshalAs(UnmanagedType.U4)]
    public const uint SECONDS = 1;

    [MarshalAs(UnmanagedType.U4)]
    public const uint MINUTES = 60;

    [MarshalAs(UnmanagedType.U4)]
    public const uint HOURS = MINUTES * 60;

    [MarshalAs(UnmanagedType.U4)]
    public const uint DAYS = HOURS * 24;

    [MarshalAs(UnmanagedType.U4)]
    public const uint WEEKS = DAYS * 7;

    [MarshalAs(UnmanagedType.U4)]
    public const uint MONTHS = DAYS * 30;

    [MarshalAs(UnmanagedType.U4)]
    public const uint YEARS = MONTHS * 12;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
    public static uint[] multipliers = new uint[7] { SECONDS, MINUTES, HOURS, DAYS, WEEKS, MONTHS, YEARS };
    #endregion
    public BAOC_TimeNumber GetLargestNonNullNumber()
    {
        ulong[] data = { seconds, minutes, hours, days, weeks, months, years };
        string[] names = { "sek", "min", "hr", "dana", "tj", "mj", "god" };

        BAOC_TimeNumber ret = new BAOC_TimeNumber(names[0], 0);

        for (int i = data.Length; i --> 0;)
        {
            if (data[i] != 0)
            {
                ret.name = names[i];
                ret.number = data[i];

                break;
            }
        }

        return ret;
    }
    public string Format()
    {
        formmated = $"{GetLargestNonNullNumber().number} {GetLargestNonNullNumber().name}";

        return formmated;
    }

    public byte[] ToBytes()
    {
        int size = Marshal.SizeOf(this);
        byte[] arr = new byte[size];

        IntPtr ptr = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(this, ptr, true);
        Marshal.Copy(ptr, arr, 0, size);
        Marshal.FreeHGlobal(ptr);
        return arr;
    }
    public static BAOC_TimeData FromBytes(byte[] arr)
    {
        BAOC_TimeData str = new BAOC_TimeData();

        int size = Marshal.SizeOf(str);
        IntPtr ptr = Marshal.AllocHGlobal(size);

        Marshal.Copy(arr, 0, ptr, size);

        str = (BAOC_TimeData)Marshal.PtrToStructure(ptr, str.GetType());
        Marshal.FreeHGlobal(ptr);

        return str;
    }
    public void Convert()
    {
        seconds = seconds + minutes * 60 + hours * 60 * 60 + days * 60 * 60 * 24 + weeks * 60 * 60 * 24 * 7 + months * 60 * 60 * 24 * 30 + years * 60 * 60 * 24 * 365;


        minutes = seconds / 60;
        hours = minutes / 60;
        days = hours / 24;
        weeks = days / 7;
        months = days / 30;
        years = months / 12;

        months = months - (years * 12);
        weeks = weeks - (months * 4);
        days = days - (weeks * 7);
        hours = hours - (days * 24);
        minutes = minutes - (hours * 60);
        seconds = seconds - (minutes * 60);
    }
}
[System.Serializable]
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct Stats
{
    [MarshalAs(UnmanagedType.Struct, SizeConst = 64)]
    public BAOC_TimeData played;
    public byte[] ToBytes()
    {
        int size = Marshal.SizeOf(this);
        byte[] arr = new byte[size];

        IntPtr ptr = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(this, ptr, true);
        Marshal.Copy(ptr, arr, 0, size);
        Marshal.FreeHGlobal(ptr);
        return arr;
    }
    public static Stats FromBytes(byte[] arr)
    {
        Stats str = new Stats();

        int size = Marshal.SizeOf(str);
        IntPtr ptr = Marshal.AllocHGlobal(size);

        Marshal.Copy(arr, 0, ptr, size);

        str = (Stats)Marshal.PtrToStructure(ptr, str.GetType());
        Marshal.FreeHGlobal(ptr);

        return str;
    }
}

public class SaveManager : MonoBehaviour
{
    public static Stats LoadedStats;
    public static Stats LoadedStatsOnStart;
    public static ulong StartTime;

    [Header("Frekvencija spremanja")]
    public uint SaveFrequency;
    
    public static string SavePath;

    static float currentSecond = 0f;
    public static void Save(Stats stats)
    {
        BAOC.Log($"Saving to {SavePath}...");

        File.WriteAllBytes(SavePath, stats.ToBytes());

        BAOC.Log($"Saved to {SavePath}.");
    }
    public static void Load()
    {
        if (File.Exists(SavePath))
        {
            LoadedStats = Stats.FromBytes(File.ReadAllBytes(SavePath));
        }
        else
        {
            LoadedStats = new Stats();

            BAOC_TimeData time = new BAOC_TimeData();

            LoadedStats.played = time;

            File.WriteAllBytes(SavePath, LoadedStats.ToBytes());
        }
    }
    public static void ApplyStats()
    {
        Game.stats = LoadedStats;
    }
    public static void ManualStart(string savePath)
    {
        StartTime = Game.GetEpoch();
        
        string path = $"{savePath}STATS.BIN";

        SavePath = path;
        BAOC.Log($"Loading stats from {SavePath}...");
        Load();
        BAOC.Log($"Loaded these stats from {SavePath}: {Game.ToJSON(LoadedStats)}");
        LoadedStats.played.Convert();
        LoadedStatsOnStart = LoadedStats;
        ApplyStats();
    }
    private void FixedUpdate()
    {

        currentSecond += Time.fixedDeltaTime;
        if (currentSecond >= 1f)
        {
            currentSecond = 0f;
            BAOC.Log(LoadedStatsOnStart.played.seconds);
            LoadedStats.played.seconds = LoadedStatsOnStart.played.seconds + (Game.GetEpoch() - StartTime);
            LoadedStats.played.Convert();

            RPC.GameInstance.UpdateRichPresence();

            ApplyStats();
        }
    }
    private void OnApplicationQuit()
    {
        Save(Game.stats);
    }
}
