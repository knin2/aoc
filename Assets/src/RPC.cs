using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

using Newtonsoft.Json;

public class RPC_Timestamps
{
    public ulong start;
}
public class RPCData
{
    public string state;
    public string details;
    public RPC_Timestamps timestamps;
    public Dictionary<string, string> assets;
    public bool instance;
}
public class RPC : MonoBehaviour
{
    public static Thread main_t;
    public static void Init(RPCData activity) 
    {
       main_t = new Thread(setActivity);
       main_t.Start(activity);
    }
    public static void SetActivity(object activity_)
    {
        main_t.Abort();
        main_t = new Thread(new ParameterizedThreadStart(setActivity));
        main_t.Start(activity_);
    }
    public static void setActivity(object _activity) { 
        RPCData activity = (RPCData)_activity;
        string jsonString = JsonConvert.SerializeObject(activity);
        string TEMP = Path.GetTempPath();
        string json_path = $"{TEMP}/BAOC_ACTIVITY.JSON";
        File.WriteAllText(json_path, jsonString);
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized;
        Debug.Log($"\"{json_path}\"");
        startInfo.FileName = $"{Game.save_path}/rpc/rpc-cmd-api.exe";
        startInfo.UseShellExecute = true;
        startInfo.Arguments = $"\"{json_path}\"";
        process.StartInfo = startInfo;
        process.Start();
    }
    public static void Deinit()
    {
        main_t.Abort();
    }        
}
