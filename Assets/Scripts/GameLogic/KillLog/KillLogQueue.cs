using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillLogQueue : ScriptableObject {

    private static KillLogQueue instance = null;
    public static KillLogQueue Instance
    {
        get
        {
            if(instance == null)
            {
                instance = CreateInstance<KillLogQueue>();
                instance.killLogs = new List<Tuple<string, string>>();
                instance.called = new List<bool>();
                instance.start = instance.end = 0;
                instance.lockObj = new object();
            }
            return instance;
        }
    }

    private object lockObj;
    private int start, end;
    public int Start { get { return start; } }
    public int End { get { return end; } }
    private List<Tuple<string, string>> killLogs;
    private List<bool> called;

    public void IncrementStart()
    {
        lock(lockObj)
        {
            if (start < end)
            {
                start++;
                Debug.Log(start);
                KillLogMgr.Notify();
            }
        }
    }

    public void AddLog(string from, string to)
    {
        lock(lockObj)
        {
            end++;
            killLogs.Add(new Tuple<string, string>(from, to));
            called.Add(false);
        }
        KillLogMgr.Notify();
    }

    public void AddLog(Tuple<string, string> log)
    {
        lock(lockObj)
        {
            end++;
            killLogs.Add(log);
            called.Add(false);
        }
        KillLogMgr.Notify();
    }

    public void IncrementEnd()
    {
        lock(lockObj)
        {
            end++;
        }
        KillLogMgr.Notify();
    }

    public Tuple<string, string> GetLog(int index)
    {
        lock(lockObj)
        {
            return killLogs[index];
        }
    }

    public void Called(int index)
    {
        called[index] = true;
    }

    public bool IsCalled(int index)
    {
        return called[index];
    }
}
