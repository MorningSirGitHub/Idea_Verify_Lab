using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CounterLock : ICounter
{

    private int m_Count = 0;
    public int Count { get { return m_Count; } }

    public void Increment()
    {
        System.Threading.Interlocked.Increment(ref m_Count);
    }

    public void Decrement()
    {
        System.Threading.Interlocked.Decrement(ref m_Count);
    }

}
