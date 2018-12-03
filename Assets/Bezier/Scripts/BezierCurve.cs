using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class BezierCurve : MonoBehaviour
{

    public Transform[] controlPoints;
    public LineRenderer lineRenderer;

    private int layerOrder = 0;
    private int _segmentNum = 50;


    void Start()
    {
        if (!lineRenderer)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        lineRenderer.sortingLayerID = layerOrder;
    }

    void Update()
    {

        DrawCurve();

    }

    void DrawCurve()
    {
        for (int i = 1; i <= _segmentNum; i++)
        {
            float t = i / (float)_segmentNum;
            int nodeIndex = 0;
            UnityEngine.Vector3 pixel = CalculateCubicBezierPoint(t, controlPoints[nodeIndex].position,
                controlPoints[nodeIndex + 1].position, controlPoints[nodeIndex + 2].position);
            lineRenderer.positionCount = i;
            lineRenderer.SetPosition(i - 1, pixel);
        }

    }

    Vector3 CalculateCubicBezierPoint(float t, UnityEngine.Vector3 p0, UnityEngine.Vector3 p1, UnityEngine.Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        UnityEngine.Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }

}
