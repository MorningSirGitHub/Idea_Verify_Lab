using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Text富文本的顶点优化
/// </summary>
public class UIVertexOptimize : BaseMeshEffect
{
    //NOTE: 当文本中的富文本标签非常多时，会发现虽然显示的字符数很少，但是生成的顶点数却非常多，有时候会多到报错
    //      搜索发现是unity的问题，在根据字符生成顶点数时，没有对富文本做优化，导致富文本标签也会生成顶点数，生成了大量重复的顶点
    //      一个字符会生成6个顶点，6个顶点构成2个三角面，可以将下面的脚本挂到Text组件下，将重复的三角面过滤掉

    private struct Triangle
    {
        public UIVertex v1;
        public UIVertex v2;
        public UIVertex v3;
    }

    private List<UIVertex> verts = new List<UIVertex>();

    public override void ModifyMesh(VertexHelper vh)
    {
        vh.GetUIVertexStream(verts);
        Debug.LogError(verts.Count);
        OptimizeVert(ref verts);
        Debug.LogError(verts.Count);
        vh.Clear();
        vh.AddUIVertexTriangleStream(verts);
    }

    private void OptimizeVert(ref List<UIVertex> vertices)
    {
        List<Triangle> tris = new List<Triangle>(vertices.Count / 3 + 1);
        for (int i = 0; i < vertices.Count - 3; i += 3)
            tris.Add(new Triangle() { v1 = vertices[i], v2 = vertices[i + 1], v3 = vertices[i + 2] });

        //去除重复元素
        vertices = tris.Distinct().SelectMany(tri => new[] { tri.v1, tri.v2, tri.v3 }).ToList();
        //TODO: 下划线需要进行优化
    }

}
