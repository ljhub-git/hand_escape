using UnityEngine;

public class PuzzleTileExtruder : MonoBehaviour
{
    public float thickness = 2f; // ������ �β� ����

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
        // ������ ����(vertices) ����
        Vector3[] originalVertices = mesh.vertices;
        int vertexCount = originalVertices.Length;
        Vector3[] extrudedVertices = new Vector3[vertexCount * 2];

        // ������ ���Ʒ��� Ȯ��
        for (int i = 0; i < vertexCount; i++)
        {
            Vector3 vertex = originalVertices[i];
            extrudedVertices[i] = vertex; // �ո�
            extrudedVertices[i + vertexCount] = vertex - new Vector3(0, 0, depth); // �޸�
        }

        // ���� �ﰢ�� ����
        int[] originalTriangles = mesh.triangles;
        int triangleCount = originalTriangles.Length;
        int[] extrudedTriangles = new int[triangleCount * 2 + vertexCount * 6];

        // �ո�� �޸� �߰�
        for (int i = 0; i < triangleCount; i += 3)
        {
            extrudedTriangles[i] = originalTriangles[i];
            extrudedTriangles[i + 1] = originalTriangles[i + 1];
            extrudedTriangles[i + 2] = originalTriangles[i + 2];

            extrudedTriangles[triangleCount + i] = originalTriangles[i] + vertexCount;
            extrudedTriangles[triangleCount + i + 1] = originalTriangles[i + 1] + vertexCount;
            extrudedTriangles[triangleCount + i + 2] = originalTriangles[i + 2] + vertexCount;
        }

        // ���� �߰�
        int sideIndex = triangleCount * 2;
        for (int i = 0; i < vertexCount; i++)
        {
            int next = (i + 1) % vertexCount;

            // �ð� ���� ����
            extrudedTriangles[sideIndex++] = i;
            extrudedTriangles[sideIndex++] = next;
            extrudedTriangles[sideIndex++] = i + vertexCount;

            // �ݽð� ���� ����
            extrudedTriangles[sideIndex++] = next;
            extrudedTriangles[sideIndex++] = next + vertexCount;
            extrudedTriangles[sideIndex++] = i + vertexCount;
        }

        // ���ο� Mesh ����
        Mesh extrudedMesh = new Mesh();
        extrudedMesh.vertices = extrudedVertices;
        extrudedMesh.triangles = extrudedTriangles;
        extrudedMesh.RecalculateNormals();

        return extrudedMesh;
    }
}
