using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SlerpTools : MonoBehaviour
{

    /// <summary>
    /// 两个向量的球形插值
    /// </summary>
    /// <param name="a">向量a</param>
    /// <param name="b">向量b</param>
    /// <param name="t">t的值在[0..1]</param>
    /// <returns></returns>
    public static Vector3D Slerp(Vector3D a, Vector3D b, float t)
    {
        if (t <= 0)
        {
            return a;
        }
        else if (t >= 1)
        {
            return b;
        }

        Vector3D v = RotateTo(a, b, Vector3D.Angle(a, b) * t);

        //向量的长度，跟线性插值一样计算
        float length = b.magnitude * t + a.magnitude * (1 - t);
        return v.normalized * length;
    }

    /// <summary>
    /// 将向量from向向量to旋转角度angle
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static Vector3D RotateTo(Vector3D from, Vector3D to, float angle)
    {
        //如果两向量角度为0
        if (Vector3D.Angle(from, to) == 0)
        {
            return from;
        }

        //旋转轴
        Vector3D n = Vector3D.Cross(from, to);

        //旋转轴规范化
        n.Normalize();

        //旋转矩阵
        Matrix4x4 rotateMatrix = new Matrix4x4();

        //旋转的弧度
        double radian = angle * Math.PI / 180;
        float cosAngle = (float)Math.Cos(radian);
        float sinAngle = (float)Math.Sin(radian);

        //矩阵的数据
        //这里看不懂的自行科普矩阵知识
        rotateMatrix.SetRow(0, new Vector4(n.x * n.x * (1 - cosAngle) + cosAngle, n.x * n.y * (1 - cosAngle) + n.z * sinAngle, n.x * n.z * (1 - cosAngle) - n.y * sinAngle, 0));
        rotateMatrix.SetRow(1, new Vector4(n.x * n.y * (1 - cosAngle) - n.z * sinAngle, n.y * n.y * (1 - cosAngle) + cosAngle, n.y * n.z * (1 - cosAngle) + n.x * sinAngle, 0));
        rotateMatrix.SetRow(2, new Vector4(n.x * n.z * (1 - cosAngle) + n.y * sinAngle, n.y * n.z * (1 - cosAngle) - n.x * sinAngle, n.z * n.z * (1 - cosAngle) + cosAngle, 0));
        rotateMatrix.SetRow(3, new Vector4(0, 0, 0, 1));

        Vector4 v = Vector3D.ToVector4(from);
        Vector3D vector = new Vector3D();
        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 3; j++)
            {
                vector[i] += v[j] * rotateMatrix[j, i];
            }
        }
        return vector;
    }

    /// <summary>
    /// 将一个Vector3D转换为Vector4
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static Vector4 ToVector4(Vector3D v)
    {
        return new Vector4(v.x, v.y, v.z, 0);
    }

    //public Vector3 ToVector3()
    //{
    //    return new Vector3(x, y, z);
    //}

}
