using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BezierUtility : MonoBehaviour
{
    /// <summary>
    /// 根据T值，计算贝塞尔曲线上面相对应的点
    /// </summary>
    /// <param name="t"></param>T值
    /// <param name="p0"></param>起始点
    /// <param name="p1"></param>控制点
    /// <param name="p2"></param>目标点
    /// <returns></returns>根据T值计算出来的贝赛尔曲线点
    private static Vector3 CalculateCubicBezierPoint(float t, UnityEngine.Vector3 p0, UnityEngine.Vector3 p1, UnityEngine.Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        UnityEngine.Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }

    /// <summary>
    /// 获取存储贝塞尔曲线点的数组
    /// </summary>
    /// <param name="startPoint"></param>起始点
    /// <param name="controlPoint"></param>控制点
    /// <param name="endPoint"></param>目标点
    /// <param name="segmentNum"></param>采样点的数量
    /// <returns></returns>存储贝塞尔曲线点的数组
    public static UnityEngine.Vector3[] GetBeizerList(UnityEngine.Vector3 startPoint, UnityEngine.Vector3 controlPoint, UnityEngine.Vector3 endPoint, int segmentNum)
    {
        UnityEngine.Vector3[] path = new UnityEngine.Vector3[segmentNum];

        for (int i = 1; i <= segmentNum; i++)
        {
            float t = i / (float)segmentNum;

            UnityEngine.Vector3 pixel = CalculateCubicBezierPoint(t, startPoint,
                controlPoint, endPoint);

            path[i - 1] = pixel;
            Debug.Log(path[i - 1]);
        }

        return path;
    }

}
