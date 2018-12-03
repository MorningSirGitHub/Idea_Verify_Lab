using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Counter : ICounter
{

    private int m_Count = 0;
    public int Count { get { return m_Count; } }

    public void Increment()
    {
        m_Count++;
    }

    public void Decrement()
    {
        m_Count--;
    }

}
