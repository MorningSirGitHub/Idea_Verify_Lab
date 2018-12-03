using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombineMesh : MonoBehaviour
{

    void Start()
    {
        // 获取自身和所有子物体中所有MeshRenderer组件
        MeshRenderer[] meshRenders = GetComponentsInChildren<MeshRenderer>();
        // 新建材质球数组
        Material[] mats = new Material[meshRenders.Length];
        for (int i = 0; i < meshRenders.Length; i++)
        {
            // 获取材质球列表
            mats[i] = meshRenders[i].sharedMaterial;
        }

        // 获取自身和所有子物体中所有MeshFilter组件
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        // 新建CombineInstance数组
        CombineInstance[] combines = new CombineInstance[meshFilters.Length];

        for (int j = 0; j < meshFilters.Length; j++)
        {
            combines[j].mesh = meshFilters[j].sharedMesh;
            // 矩阵(Matrix)自身空间坐标的点转换成世界空间坐标的点   
            combines[j].transform = meshFilters[j].transform.localToWorldMatrix;
            // 变换矩阵的问题，要保持相对位置不变，要转换为父节点的本地坐标
            //combines[j].transform = transform.worldToLocalMatrix * meshFilters[j].transform.localToWorldMatrix;
            //meshFilters[j].gameObject.SetActive(false);
            // 除了根物体，其他物体统统销毁
            Destroy(meshFilters[j].gameObject);
        }

        MeshRenderer meshRender = gameObject.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();
        // 为mesh.CombineMeshes添加一个 false 参数，表示并不是合并为一个网格，而是一个子网格列表，可以让拥有多个材质球，如果要合并的网格用的是同一材质，false改为true，同时将上面的获取Material的代码去掉
        meshFilter.mesh.CombineMeshes(combines, false);
        gameObject.SetActive(true);
        // 为合并后的GameObject指定材质球数组
        meshRender.sharedMaterials = mats;
    }

    void Update()
    {

    }

}
