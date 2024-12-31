using UnityEngine;

public class PuzzleTileExtruder : MonoBehaviour
{
    public float thickness = 2f; // 퍼즐의 두께 설정

    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            Mesh originalMesh = meshFilter.mesh;
            Mesh extrudedMesh = ExtrudeMesh(originalMesh, thickness);
            meshFilter.mesh = extrudedMesh;
        }
    }

    Mesh ExtrudeMesh(Mesh mesh, float depth)
    {
        // 기존의 정점(vertices) 복사
        Vector3[] originalVertices = mesh.vertices;
        int vertexCount = originalVertices.Length;
        Vector3[] extrudedVertices = new Vector3[vertexCount * 2];

        // 정점을 위아래로 확장
        for (int i = 0; i < vertexCount; i++)
        {
            Vector3 vertex = originalVertices[i];
            extrudedVertices[i] = vertex; // 앞면
            extrudedVertices[i + vertexCount] = vertex - new Vector3(0, 0, depth); // 뒷면
        }

        // 기존 삼각형 복사
        int[] originalTriangles = mesh.triangles;
        int triangleCount = originalTriangles.Length;
        int[] extrudedTriangles = new int[triangleCount * 2 + vertexCount * 6];

        // 앞면과 뒷면 추가
        for (int i = 0; i < triangleCount; i += 3)
        {
            extrudedTriangles[i] = originalTriangles[i];
            extrudedTriangles[i + 1] = originalTriangles[i + 1];
            extrudedTriangles[i + 2] = originalTriangles[i + 2];

            extrudedTriangles[triangleCount + i] = originalTriangles[i] + vertexCount;
            extrudedTriangles[triangleCount + i + 1] = originalTriangles[i + 1] + vertexCount;
            extrudedTriangles[triangleCount + i + 2] = originalTriangles[i + 2] + vertexCount;
        }

        // 측면 추가
        int sideIndex = triangleCount * 2;
        for (int i = 0; i < vertexCount; i++)
        {
            int next = (i + 1) % vertexCount;

            // 시계 방향 측면
            extrudedTriangles[sideIndex++] = i;
            extrudedTriangles[sideIndex++] = next;
            extrudedTriangles[sideIndex++] = i + vertexCount;

            // 반시계 방향 측면
            extrudedTriangles[sideIndex++] = next;
            extrudedTriangles[sideIndex++] = next + vertexCount;
            extrudedTriangles[sideIndex++] = i + vertexCount;
        }

        // 새로운 Mesh 생성
        Mesh extrudedMesh = new Mesh();
        extrudedMesh.vertices = extrudedVertices;
        extrudedMesh.triangles = extrudedTriangles;
        extrudedMesh.RecalculateNormals();

        return extrudedMesh;
    }
}
