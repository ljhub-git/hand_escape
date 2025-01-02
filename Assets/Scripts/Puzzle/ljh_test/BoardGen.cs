using UnityEngine;

public class BoardGen : MonoBehaviour
{
    public Transform tilesParent;
    public Jigsaw_PuzzleManager puzzleManager;
    public Texture2D puzzleImage;
    public string imageFilename; // 퍼즐 이미지 파일 경로를 설정할 public 변수
    public int rows = 4;
    public int cols = 4;
    public float tileThickness = 0.1f;

    public Material FrontMaterial; // 정면용 메터리얼
    public Material SideMaterial;  // 옆면용 메터리얼

    private void Start()
    {
        GenerateBoard();
    }

    public void GenerateBoard()
    {
        Texture2D texture = Resources.Load<Texture2D>(imageFilename);
        if (texture == null)
        {
            Debug.LogError("Failed to load texture: " + imageFilename);
            return;
        }

        int totalTiles = rows * cols;
        puzzleManager.SetTotalTiles(totalTiles);

        int tileWidth = puzzleImage.width / cols;
        int tileHeight = puzzleImage.height / rows;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                GameObject tileObject = new GameObject($"Tile_{x}_{y}");
                tileObject.transform.SetParent(tilesParent, false);

                // 타일의 메시 및 텍스처 설정
                Tile tile = tileObject.AddComponent<Tile>();
                tile.meshFilter = tileObject.AddComponent<MeshFilter>();
                tile.meshRenderer = tileObject.AddComponent<MeshRenderer>();
                tile.Initialize(
                    GenerateVertices(x, y, tileWidth, tileHeight),
                    GenerateTriangles(),
                    GenerateUV(x, y, tileWidth, tileHeight),
                    GetTileTexture(x, y, tileWidth, tileHeight)
                );

                // 타일 이동 스크립트 설정
                TileMovement tileMovement = tileObject.AddComponent<TileMovement>();
                tileMovement.SetManager(puzzleManager);
                tileMovement.SetCorrectPosition(new Vector3(x * tileWidth, 0, y * tileHeight));
                puzzleManager.RegisterTile(tileMovement);
            }
        }
    }

    private Vector3[] GenerateVertices(int x, int y, int tileWidth, int tileHeight)
    {
        return new Vector3[]
        {
            new Vector3(x * tileWidth, 0, y * tileHeight),
            new Vector3((x + 1) * tileWidth, 0, y * tileHeight),
            new Vector3((x + 1) * tileWidth, 0, (y + 1) * tileHeight),
            new Vector3(x * tileWidth, 0, (y + 1) * tileHeight)
        };
    }

    private int[] GenerateTriangles()
    {
        return new int[] { 0, 1, 2, 0, 2, 3 };
    }

    private Vector2[] GenerateUV(int x, int y, int tileWidth, int tileHeight)
    {
        float uvX = x / (float)puzzleImage.width;
        float uvY = y / (float)puzzleImage.height;
        float uvWidth = tileWidth / (float)puzzleImage.width;
        float uvHeight = tileHeight / (float)puzzleImage.height;

        return new Vector2[]
        {
            new Vector2(uvX, uvY),
            new Vector2(uvX + uvWidth, uvY),
            new Vector2(uvX + uvWidth, uvY + uvHeight),
            new Vector2(uvX, uvY + uvHeight)
        };
    }

    private Texture2D GetTileTexture(int x, int y, int tileWidth, int tileHeight)
    {
        Texture2D tileTexture = new Texture2D(tileWidth, tileHeight);
        tileTexture.SetPixels(puzzleImage.GetPixels(x * tileWidth, y * tileHeight, tileWidth, tileHeight));
        tileTexture.Apply();
        return tileTexture;
    }
}
