using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraView : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetMouseButton(0))
            transform.Translate(Vector3.left * Time.deltaTime * 5f);
        if (Input.GetMouseButton(1))
            transform.Translate(Vector3.right * Time.deltaTime * 5f);
    }

    private void OnBecameVisible()
    {
        Debug.Log("Scene 内部！！     我看见你了！！！！！");
    }

    private void OnRenderObject()
    {
        //Debug.Log("摄像机渲染场景后调用的函数~~ OnRenderObject    !!!!");
    }

    private void OnWillRenderObject()
    {
        //Debug.Log("如果对象可见，则每个摄像机都会调用一次 OnWillRenderObject!!!!");
    }

}
