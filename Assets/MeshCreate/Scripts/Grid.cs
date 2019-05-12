using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour
{

    public int xSize, ySize;
    public Material mat;

    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRender;

    private Vector4[] tangents;
    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] triangles;

    void Awake()
    {
        StartCoroutine(GenerateByIE());
    }

    private IEnumerator GenerateByIE()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f);

        meshFilter = GetComponent<MeshFilter>();
        meshRender = GetComponent<MeshRenderer>();

        meshRender.material = mat;

        mesh = new Mesh();
        mesh.name = "MorningSir";

        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        tangents = new Vector4[vertices.Length];
        uv = new Vector2[vertices.Length];
        var tangent = new Vector4(1f, 0f, 0f, -1f);
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
                tangents[i] = tangent;
                uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
                yield return wait;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.tangents = tangents;

        triangles = new int[xSize * ySize * 6];
        for (int y = 0, ti = 0, vi = 0; y < ySize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 2] = vi + 1;
                triangles[ti + 3] = vi + 1;
                triangles[ti + 4] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
                mesh.triangles = triangles;
                meshFilter.mesh = mesh;
                yield return wait;
            }
        }

        //mesh.triangles = triangles;

        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    private void Generate()
    {
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
            return;

        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }

}
