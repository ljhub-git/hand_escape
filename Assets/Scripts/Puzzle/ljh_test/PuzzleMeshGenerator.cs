using UnityEngine;

public class PuzzleMeshGenerator : MonoBehaviour
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uv;

    public void GenerateMesh(Vector3[] topVertices)
    {
        // Top face generation
        vertices = topVertices;
        triangles = GenerateTopTriangles(topVertices.Length);
        uv = GenerateUV(topVertices);

        Mesh topMesh = new Mesh
        {
            vertices = vertices,
            triangles = triangles,
            uv = uv
        };
        topMesh.RecalculateNormals();

        // Side face generation
        Mesh sideMesh = GenerateSideMesh(topVertices);

        // Combine top and side faces
        CombineInstance[] combine = new CombineInstance[2];
        combine[0].mesh = topMesh;
        combine[1].mesh = sideMesh;

        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine, true, false);

        GetComponent<MeshFilter>().mesh = combinedMesh;
    }

    private Mesh GenerateSideMesh(Vector3[] topVertices)
    {
        int vertexCount = topVertices.Length;
        Vector3[] sideVertices = new Vector3[vertexCount * 2];
        int[] sideTriangles = new int[(vertexCount - 1) * 6];

        for (int i = 0; i < vertexCount; i++)
        {
            sideVertices[i] = topVertices[i]; // Top vertices
            sideVertices[i + vertexCount] = topVertices[i] - Vector3.up * 0.1f; // Bottom vertices
        }

        int triIndex = 0;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            sideTriangles[triIndex++] = i;
            sideTriangles[triIndex++] = i + vertexCount;
            sideTriangles[triIndex++] = i + 1;

            sideTriangles[triIndex++] = i + 1;
            sideTriangles[triIndex++] = i + vertexCount;
            sideTriangles[triIndex++] = i + 1 + vertexCount;
        }

        Mesh sideMesh = new Mesh
        {
            vertices = sideVertices,
            triangles = sideTriangles
        };
        sideMesh.RecalculateNormals();

        return sideMesh;
    }

    private int[] GenerateTopTriangles(int vertexCount)
    {
        int[] triangles = new int[(vertexCount - 2) * 3];
        for (int i = 0; i < vertexCount - 2; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }
        return triangles;
    }

    private Vector2[] GenerateUV(Vector3[] vertices)
    {
        Vector2[] uv = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            uv[i] = new Vector2(vertices[i].x, vertices[i].z);
        }
        return uv;
    }
}
