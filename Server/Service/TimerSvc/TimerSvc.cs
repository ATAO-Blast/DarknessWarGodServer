using PENet;
using PEProtocol;
using System;
using System.Collections.Generic;

public class TimerSvc:SingletonPattern<TimerSvc>
{
    class TaskPack
    {
        public int tid;
        public Action<int> callback;
        public TaskPack(int tid, Action<int> callback)
        {
            this.tid = tid;
            this.callback = callback;
        }
    }
    PETimer pt = null;
    Queue<TaskPack> tpQue = new Queue<TaskPack>();
    static readonly string tpQueLock = "tpQueLock";
    public void Init()
    {
        pt = new PETimer(100);
        tpQue.Clear();
        pt.SetLog(info =>
        {
            PECommon.Log(info);
        });
        pt.SetHandle((callback,tid) =>
        {
            if(callback != null)
            {
                lock(tpQueLock)
                {
                    tpQue.Enqueue(new TaskPack(tid, callback));
                }
            }
        });
        PECommon.Log("TimerSvc Init Done");
    }
    public void Update()
    {
        while(tpQue.Count > 0) 
        {
            TaskPack tp = null;
            lock (tpQueLock)
            {
                tp = tpQue.Dequeue();
            }
            tp?.callback(tp.tid);
        }
    }
    public int AddTimeTask(Action<int> callback, double delay, PETimeUnit timeUnit = PETimeUnit.Millisecond, int count = 1)
    {
        return pt.AddTimeTask(callback, delay, timeUnit, count);
    }
    public long GetNowTime()
    {
        return (long)pt.GetMillisecondsTime();
    }
}

