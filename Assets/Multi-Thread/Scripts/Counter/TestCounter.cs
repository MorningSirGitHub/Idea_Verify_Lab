using UnityEngine;
using System.Threading;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// https://blog.csdn.net/WuLex/article/details/53869302
/// </summary>
public class TestCounter
{

    public static void CounterTest(ICounter ic)
    {
        for (int i = 0; i < 100000; i++)
        {
            ic.Increment();
            ic.Decrement();
        }
    }

    //[UnityEditor.MenuItem("TTTTTT/CounterTest")]
    public static void RunTest()
    {

        GeneralCounter();

        ThreadSyncCounter();

    }

    public static void GeneralCounter()
    {
        var c = new Counter();

        var t1 = new Thread((() => CounterTest(c)));
        var t2 = new Thread((() => CounterTest(c)));
        var t3 = new Thread((() => CounterTest(c)));

        t1.Start();
        t2.Start();
        t3.Start();

        t1.Join();
        t2.Join();
        t3.Join();

        Debug.LogFormat("总数: {0}", c.Count);
        Debug.LogFormat("--------------------------");
        Debug.LogFormat("普通计数器");
    }

    public static void ThreadSyncCounter()
    {
        var cl = new CounterLock();

        var t1 = new Thread((() => CounterTest(cl)));
        var t2 = new Thread((() => CounterTest(cl)));
        var t3 = new Thread((() => CounterTest(cl)));

        t1.Start();
        t2.Start();
        t3.Start();

        t1.Join();
        t2.Join();
        t3.Join();

        Debug.LogFormat("总数: {0}", cl.Count);
        Debug.LogFormat("--------------------------");
        Debug.LogFormat("线程同步的计数器");
    }

}
