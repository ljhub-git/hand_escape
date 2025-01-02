using UnityEngine;

public class Tile : MonoBehaviour
{
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public void Initialize(Vector3[] vertices, int[] triangles, Vector2[] uv, Texture2D texture)
    {
        // �޽� ����
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();

        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;

        // ���� ���͸���
        if (meshRenderer.materials.Length == 0)
        {
            meshRenderer.materials = new Material[2]; // ����� ����
        }
        meshRenderer.materials[0] = frontMaterial;

        // ���� ó�� (�⺻ �β� ����)
        Mesh sideMesh = GenerateSideMesh(vertices);
        GameObject sideObject = new GameObject("SideMesh");
        sideObject.transform.parent = this.transform;

        MeshFilter sideMeshFilter = sideObject.AddComponent<MeshFilter>();
        MeshRenderer sideMeshRenderer = sideObject.AddComponent<MeshRenderer>();

        sideMeshFilter.mesh = sideMesh;
        sideMeshRenderer.material = sideMaterial;
    }

    // ���� �޽� ���� (������ �β� �߰�)
    private Mesh GenerateSideMesh(Vector3[] vertices)
    {
        // ������ �����ϴ� vertices�� triangles ����
        // (���� �޽��� ������ �������� offset�� �� ���ο� vertex ����)
        Mesh sideMesh = new Mesh();
        // ... ���� (��ü�� ���� �߰� �ʿ�)
        return sideMesh;
    }
}
