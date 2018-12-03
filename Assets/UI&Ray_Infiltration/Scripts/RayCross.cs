using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCross : MonoBehaviour
{
    public Transform RayStartPoint;

    private Ray ray;

    public LayerMask mask = 1 << 8;

    void Start()
    {
        ray = new Ray(RayStartPoint.position, transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 500, mask))
        {
            // 打印碰撞信息
            print(hit.collider.name);

            // 画线，使其射线可视化
            Debug.DrawLine(RayStartPoint.position, hit.point, Color.red, 100);
        }
    }

}
