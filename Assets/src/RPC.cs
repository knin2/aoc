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
    public static void Init()
    {
        DiscordRPC.EventHandlers eventHandlers = new DiscordRPC.EventHandlers();
        DiscordRPC.Initialize("985117362153992302", ref eventHandlers, true, "");
        
    }
    private static DiscordRPC.RichPresence rpc_data_to_rich_presence(RPCData data)
    {
        DiscordRPC.RichPresence richPresence = new DiscordRPC.RichPresence();
        
        richPresence.state = data.state;
        richPresence.details = data.details;
        richPresence.largeImageText = data.assets["large_text"];
        richPresence.instance = data.instance;
        richPresence.startTimestamp = (long)data.timestamps.start;
        richPresence.largeImageKey = data.assets["large_image"];
        richPresence.smallImageKey = data.assets["small_image"];
        richPresence.smallImageText = data.assets["small_text"];

        return richPresence;
    }
    public static void SetActivity(RPCData data)
    {
        DiscordRPC.RichPresence rpc_foreign = rpc_data_to_rich_presence(data);
        DiscordRPC.UpdatePresence(rpc_foreign);
    }
    public static void Abort()
    {
        DiscordRPC.Shutdown();
    }
}
