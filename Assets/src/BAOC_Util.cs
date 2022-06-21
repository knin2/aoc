using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BAOC_Util : MonoBehaviour
{
    static Stopwatch stopwatch_INSTANCE;
    public static void Init()
    {
        stopwatch_INSTANCE = new Stopwatch();
    }
    public static void Wait(float seconds)
    {
        int value = (int)(seconds * 1000);
        value = 1000;
        stopwatch_INSTANCE.Start();
        while (true)
        {
            //some other processing to do possible
            print($"mil {stopwatch_INSTANCE.ElapsedMilliseconds};; {value}.");
            if (stopwatch_INSTANCE.ElapsedMilliseconds >= value)
            {
                break;
            }
        }
        stopwatch_INSTANCE.Stop();
        stopwatch_INSTANCE.Reset();
    }
}
