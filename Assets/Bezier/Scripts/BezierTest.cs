using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BezierTest : MonoBehaviour
{

    public float Duration = 0.02f;

    public List<Transform> PointList = new List<Transform>();

    private List<Vector3> m_Points = new List<Vector3>();

    private float m_Timer;

    private int m_Index = 1;

    void Awake()
    {

    }

    void Start()
    {
        for (int i = 0; i < 200; i++)
        {
            m_Points.Add(Bezier(i, PointList));
            Debug.LogError(m_Points[i]);
        }
        Debug.LogError(m_Points.Count);
    }

    void Update()
    {
        if (PointList.Count <= 0)
            return;

        m_Timer += Time.deltaTime;
        if (m_Timer > Duration)
        {
            m_Timer = 0;
            transform.localPosition = Vector3.Lerp(m_Points[m_Index - 1], m_Points[m_Index], 1f);
            m_Index++;

            if (m_Index >= m_Points.Count)
                m_Index = 1;

        }

    }

    /// <summary>
    /// n阶曲线，递归实现
    /// </summary>
    /// <param name="t"></param>
    /// <param name="p"></param>
    /// <returns></returns>
    public Vector3 Bezier(float t, List<Vector3> p)
    {
        if (p.Count < 2)
            return p[0];

        List<Vector3> newp = new List<Vector3>();
        for (int i = 0; i < p.Count - 1; i++)
        {
            Debug.DrawLine(p[i], p[i + 1]);
            Vector3 p0p1 = (1 - t) * p[i] + t * p[i + 1];
            newp.Add(p0p1);
        }

        return Bezier(t, newp);
    }

    /// <summary>
    /// transform转换为vector3，在调用参数为List<Vector3>的Bezier函数
    /// </summary>
    /// <param name="t"></param>
    /// <param name="p"></param>
    /// <returns></returns>
    public Vector3 Bezier(float t, List<Transform> p)
    {
        if (p.Count < 2)
            return p[0].position;

        List<Vector3> newp = new List<Vector3>();
        for (int i = 0; i < p.Count; i++)
        {
            newp.Add(p[i].position);
        }

        return Bezier(t, newp);
    }

}
