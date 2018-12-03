using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DelegateTest : MonoBehaviour
{
    public delegate void Dele();
    Dele dele;


    void Start()
    {
        dele += AAA;
        dele += BBB;
        dele += CCC;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.LogError("播放！！");
            Debug.Log(dele.Method + " / " + dele.Target);
            if (dele != null)
                dele();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            dele += AAA;
            dele += BBB;
            dele += CCC;
            Debug.LogError("绑定完成！！");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            var list = dele.GetInvocationList();
            for (int i = 0; i < list.Length; i++)
            {
                Debug.Log(list[i].Method + "/" + list[i].Target);
                dele -= list[i] as Dele;
            }
            Debug.LogError("注销完成！！");
        }
    }

    private void OnDestroy()
    {
        dele -= AAA;
        dele -= BBB;
        dele -= CCC;
    }

    void AAA()
    {
        Debug.Log("我是 AAA !");
    }

    void BBB()
    {
        Debug.Log("我是 BBB !");
    }

    void CCC()
    {
        Debug.Log("我是 CCC !");
    }

}
