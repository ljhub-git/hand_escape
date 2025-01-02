using UnityEngine;

public class Tile : MonoBehaviour
{
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public void Initialize(Vector3[] vertices, int[] triangles, Vector2[] uv, Texture2D texture)
    {
        // 메시 생성
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();

        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;

        // 정면 메터리얼
        if (meshRenderer.materials.Length == 0)
        {
            meshRenderer.materials = new Material[2]; // 정면과 옆면
        }
        meshRenderer.materials[0] = frontMaterial;

        // 옆면 처리 (기본 두께 생성)
        Mesh sideMesh = GenerateSideMesh(vertices);
        GameObject sideObject = new GameObject("SideMesh");
        sideObject.transform.parent = this.transform;

        MeshFilter sideMeshFilter = sideObject.AddComponent<MeshFilter>();
        MeshRenderer sideMeshRenderer = sideObject.AddComponent<MeshRenderer>();

        sideMeshFilter.mesh = sideMesh;
        sideMeshRenderer.material = sideMaterial;
    }

    // 옆면 메시 생성 (간단한 두께 추가)
    private Mesh GenerateSideMesh(Vector3[] vertices)
    {
        // 옆면을 구성하는 vertices와 triangles 생성
        // (기존 메시의 끝점을 기준으로 offset을 준 새로운 vertex 생성)
        Mesh sideMesh = new Mesh();
        // ... 생략 (구체적 구현 추가 필요)
        return sideMesh;
    }
}
