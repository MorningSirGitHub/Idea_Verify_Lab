using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class DrawLine : MonoBehaviour
{

    private UnityEngine.Vector3 mStart = new UnityEngine.Vector3(-1, 1, 0);
    private UnityEngine.Vector3 mEnd = new UnityEngine.Vector3(1, 1, 0);

    private void Update()
    {
        //绘制坐标轴
        Debug.DrawLine(new UnityEngine.Vector3(-100, 0, 0), new UnityEngine.Vector3(100, 0, 0), Color.green);
        Debug.DrawLine(new UnityEngine.Vector3(0, -100, 0), new UnityEngine.Vector3(0, 100, 0), Color.green);
        Debug.DrawLine(new UnityEngine.Vector3(0, 0, -100), new UnityEngine.Vector3(0, 0, 100), Color.green);

        Debug.DrawLine(UnityEngine.Vector3.zero, mStart, Color.red);
        Debug.DrawLine(UnityEngine.Vector3.zero, mEnd, Color.red);
        Debug.DrawLine(mStart, mEnd, Color.red);

        for (int i = 1; i < 10; ++i)
        {
            UnityEngine.Vector3 drawVec = UnityEngine.Vector3.Slerp(mStart, mEnd, 0.1f * i);
            Debug.DrawLine(UnityEngine.Vector3.zero, drawVec, Color.yellow);

            Debug.Log("插值向量长度：" + drawVec.magnitude);
            Debug.Log("角度：" + UnityEngine.Vector3.Angle(drawVec, mStart) / i);

        }

    }

}
