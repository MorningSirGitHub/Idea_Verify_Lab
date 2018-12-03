using System;
using System.Text;
using UnityEngine;

public class Vector3D
{

    public static Vector3D back
    {
        get
        {
            return new Vector3D(0, 0, -1);
        }
    }

    public static Vector3D down
    {
        get
        {
            return new Vector3D(0, -1, 0);
        }
    }

    public static Vector3D forward
    {
        get
        {
            return new Vector3D(0, 0, 1);
        }
    }

    public static Vector3D left
    {
        get
        {
            return new Vector3D(-1, 0, 0);
        }
    }

    public static Vector3D right
    {
        get
        {
            return new Vector3D(1, 0, 0);
        }
    }

    public static Vector3D up
    {
        get
        {
            return new Vector3D(0, 1, 0);
        }
    }

    public static Vector3D one
    {
        get
        {
            return new Vector3D(1, 1, 1);
        }
    }

    public static Vector3D zero
    {
        get
        {
            return new Vector3D(0, 0, 0);
        }
    }

    /// <summary>
    /// 向量的长度
    /// </summary>
    public float magnitude
    {
        get
        {
            return (float)Math.Sqrt(sqrMagnitudfe);
        }
    }

    /// <summary>
    /// 向量的的长度的平方
    /// </summary>
    public float sqrMagnitudfe
    {
        get
        {
            return x * x + y * y + z * z;
        }
    }

    /// <summary>
    /// 规范化
    /// </summary>
    public Vector3D normalized
    {
        get
        {
            if (IsZero)
            {
                return Vector3D.zero;
            }
            else
            {
                Vector3D v = new Vector3D();
                float length = magnitude;
                v.x = this.x / length;
                v.y = this.y / length;
                v.z = this.z / length;
                return v;
            }
        }
    }

    /// <summary>
    /// 是否是零向量
    /// </summary>
    public bool IsZero
    {
        get
        {
            return this.x == 0 && this.y == 0 && this.z == 0;
        }
    }

    #region 操作符重载

    /// <summary>
    /// 两个向量相加
    /// </summary>
    /// <param name="a">向量a</param>
    /// <param name="b">向量b</param>
    /// <returns>两个向量的和</returns>
    public static Vector3D operator +(Vector3D a, Vector3D b)
    {
        return new Vector3D(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    /// <summary>
    /// 两个向量相减
    /// </summary>
    /// <param name="a">向量a</param>
    /// <param name="b">向量b</param>
    /// <returns>两个向量的差</returns>
    public static Vector3D operator -(Vector3D a, Vector3D b)
    {
        return new Vector3D(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    /// <summary>
    /// 向量取反
    /// </summary>
    /// <param name="a">向量a</param>
    /// <returns>向量a的反向量</returns>
    public static Vector3D operator -(Vector3D a)
    {
        return new Vector3D(-a.x, -a.y, -a.z);
    }

    /// <summary>
    /// 一个数乘以一个向量
    /// </summary>
    /// <param name="d">一个数</param>
    /// <param name="a">一个向量</param>
    /// <returns>返回数与向量的乘积</returns>
    public static Vector3D operator *(float d, Vector3D a)
    {
        return new Vector3D(a.x * d, a.y * d, a.z * d);
    }

    /// <summary>
    /// 一个向量乘以一个数
    /// </summary>
    /// <param name="a">一个向量</param>
    /// <param name="d">一个数</param>
    /// <returns>返回向量与数的乘积</returns>
    public static Vector3D operator *(Vector3D a, float d)
    {
        return new Vector3D(a.x * d, a.y * d, a.z * d);
    }

    /// <summary>
    /// 一个数除一个向量，向量a/数b
    /// </summary>
    /// <param name="a">一个向量</param>
    /// <param name="d">一个数</param>
    /// <returns>返回向量a/d</returns>
    public static Vector3D operator /(Vector3D a, float d)
    {
        return new Vector3D(a.x / d, a.y / d, a.z / d);
    }

    /// <summary>
    /// 两个向量是否相等
    /// </summary>
    /// <param name="lhs">向量lhs</param>
    /// <param name="rhs">向量rhs</param>
    /// <returns>如果相等，则为true，否则为false</returns>
    public static bool operator ==(Vector3D lhs, Vector3D rhs)
    {
        bool x = Math.Abs(lhs.x - rhs.x) < 0.00001f;
        bool y = Math.Abs(lhs.y - rhs.y) < 0.00001f;
        bool z = Math.Abs(lhs.z - rhs.z) < 0.00001f;
        return x && y && z;
    }

    /// <summary>
    /// 两个向量是否不等
    /// </summary>
    /// <param name="lhs">向量lhs</param>
    /// <param name="rhs">向量rhs</param>
    /// <returns>如果不等，则为true，否则为false</returns>
    public static bool operator !=(Vector3D lhs, Vector3D rhs)
    {
        return !(lhs == rhs);
    }

    #endregion

    public float x;
    public float y;
    public float z;

    private float[] data = new float[3];

    #region 构造器
    public Vector3D()
    {
        this.x = 0.0f;
        this.y = 0.0f;
        this.z = 0.0f;

        data[0] = 0.0f;
        data[1] = 0.0f;
        data[2] = 0.0f;
    }

    public Vector3D(float x, float y)
    {
        this.x = x;
        this.y = y;
        this.z = 1.0f;

        data[0] = 0.0f;
        data[1] = 0.0f;
        data[2] = 1.0f;
    }

    public Vector3D(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;

        data[0] = x;
        data[1] = y;
        data[2] = z;
    }

    #endregion

    public float this[int index]
    {
        get
        {
            switch (index)
            {
                case 0:
                    return this.x;
                case 1:
                    return this.y;
                case 2:
                    return this.z;
                default:
                    throw new Exception("index is out of array");
            }
        }
        set
        {
            data[index] = value;
            switch (index)
            {
                case 0:
                    this.x = value;
                    break;
                case 1:
                    this.y = value;
                    break;
                case 2:
                    this.z = value;
                    break;
                default:
                    break;
            }
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("(");
        sb.Append(this.x);
        sb.Append(",");
        sb.Append(this.y);
        sb.Append(",");
        sb.Append(this.z);
        sb.Append(")");
        return sb.ToString();
    }

    public void Set(float new_x, float new_y, float new_z)
    {
        this.x = new_x;
        this.y = new_y;
        this.z = new_z;
        data[0] = new_x;
        data[1] = new_y;
        data[2] = new_z;
    }

    /// <summary>
    /// 规范化，使向量长度为1
    /// </summary>
    public void Normalize()
    {
        if (!IsZero)
        {
            this.x /= magnitude;
            this.y /= magnitude;
            this.z /= magnitude;
        }
    }

    #region 静态函数

    /// <summary>
    /// 距离
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>返回a和b之间的距离</returns>
    public static float Distance(Vector3D a, Vector3D b)
    {
        return (float)Math.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y) + (a.z - b.z) * (a.z - b.z));
    }

    /// <summary>
    /// 计算两个向量的点乘积
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static float Dot(Vector3D lhs, Vector3D rhs)
    {
        return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
    }

    /// <summary>
    /// 计算两个向量的叉乘
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns></returns>
    public static Vector3D Cross(Vector3D v1, Vector3D v2)
    {
        float x = v1.y * v2.z - v2.y * v1.z;
        float y = v1.z * v2.x - v2.z * v1.x;
        float z = v1.x * v2.y - v2.x * v1.y;
        return new Vector3D(x, y, z);
    }

    /// <summary>
    /// 限制长度
    /// </summary>
    /// <param name="v">向量v</param>
    /// <param name="maxLength">最长长度</param>
    /// <returns>返回限制长度后的向量</returns>
    public static Vector3D ClampMagnitude(Vector3D v, float maxLength)
    {
        if (maxLength >= v.magnitude)
        {
            return v;
        }

        return (maxLength / v.magnitude) * v;
    }

    /// <summary>
    /// 计算两个向量的夹角
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public static float Angle(Vector3D from, Vector3D to)
    {
        float dot = Dot(from, to);
        return (float)(180 * Math.Acos(dot / (from.magnitude * to.magnitude)) / Math.PI);
    }

    /// <summary>
    /// 投射一个向量到另一个向量
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="onNormal"></param>
    /// <returns>返回被投射到onNormal的vector</returns>
    public static Vector3D Project(Vector3D vector, Vector3D onNormal)
    {
        if (vector.IsZero || onNormal == Vector3D.zero)
        {
            return Vector3D.zero;
        }
        return Dot(vector, onNormal) / (onNormal.magnitude * onNormal.magnitude) * onNormal;
    }

    /// <summary>
    /// 反射
    /// </summary>
    /// <param name="inDirection"></param>
    /// <param name="inNormal"></param>
    /// <returns></returns>
    public static Vector3D Reflect(Vector3D inDirection, Vector3D inNormal)
    {
        return Vector3D.zero;
    }

    /// <summary>
    /// 缩放
    /// </summary>
    /// <param name="scale"></param>
    public void Scale(Vector3D scale)
    {
        this.x *= scale.x;
        this.y *= scale.y;
        this.z *= scale.z;
    }
    /// <summary>
    /// 缩放
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Vector3D Scale(Vector3D a, Vector3D b)
    {
        return new Vector3D(a.x * b.x, a.y * b.y, a.z * b.z);
    }

    /// <summary>
    /// 两个向量是否平行
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool Parallel(Vector3D a, Vector3D b)
    {
        return Cross(a, b).IsZero;
    }

    /// <summary>
    /// 两个向量之间的线性插值
    /// </summary>
    /// <param name="from">向量from</param>
    /// <param name="to">向量to</param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Vector3D Lerp(Vector3D from, Vector3D to, float t)
    {
        if (t <= 0)
        {
            return from;
        }
        else if (t >= 1)
        {
            return to;
        }
        return t * to + (1 - t) * from;
    }

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
        n.Normalize();

        //旋转矩阵
        Matrix4x4 rotateMatrix = new Matrix4x4();

        //旋转的弧度
        double radian = angle * Math.PI / 180;
        float cosAngle = (float)Math.Cos(radian);
        float sinAngle = (float)Math.Sin(radian);
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

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    #endregion
}
