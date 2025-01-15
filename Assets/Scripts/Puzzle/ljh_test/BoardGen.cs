using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BoardGen : MonoBehaviour
{
    public string imageFilename;
    Sprite mBaseSpriteOpaque;
    Sprite mBaseSpriteTransparent;

    GameObject mGameObjectQpaque;
    GameObject mGameObjectTransparent;

    public float ghostTransparency = 0.1f;

    public static float puzzle_Scale;
    [SerializeField] 
    private float puzzle_Scale_Instance = 0.5f;

    public int numTileX { get; private set; }
    public int numTileY { get; private set; }

    Tile[,] mTiles = null;
    GameObject[,] mTileGameObjects = null;

    public Transform parentForTiles = null;

    [SerializeField]
    private Jigsaw_PuzzleManager puzzleManager;

    [Range(0, 64)]
    public int numRandomTiles = 2;  // ���� ��ġ�� Ÿ���� ������ ����

    //public Vector3[] randomTilePositions; //�������� ��ġ�� Ÿ���� �������� ���� �Է��ϴ� �뵵


    Sprite LoadBaseTexture()
    {
        Texture2D tex = SpriteUtils.LoadTexture(imageFilename);
        if(!tex.isReadable)
        {
            return null;
        }

        if(tex.width % Tile.tileSize != 0 || tex.height % Tile.tileSize != 0)
        {
            return null;
        }

        Texture2D newTex = new Texture2D(
            tex.width + Tile.padding * 2,
            tex.height + Tile.padding * 2,
            TextureFormat.ARGB32,
            false);

        for(int i = 0; i< tex.width; i++)
        {
            for(int j=0; j< tex.height; j++)
            {
                newTex.SetPixel(i, j, Color.white);
            }
        }

        for (int x = 0; x < tex.width; x++)
        {
            for (int y = 0; y < tex.height; y++)
            {
                Color color = tex.GetPixel(x, y);
                color.a = 1.0f;
                newTex.SetPixel(x,y,color);
            }
        }
        newTex.Apply();

        Sprite sprite = SpriteUtils.CreateSpriteFromTexture2D(
            newTex,
            0,
            0,
            newTex.width,
            newTex.height);
        return sprite;
    }

    private void Awake()
    {
        // �ν��Ͻ� ������ ���� static ������ ����ȭ
        puzzle_Scale = puzzle_Scale_Instance;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //if (!PhotonNetwork.IsMasterClient)
        //    return;

        mBaseSpriteOpaque = LoadBaseTexture();
        //�ϼ��׸�(��Ȱ��ȭ ��ų��)
        mGameObjectQpaque = new GameObject();
        mGameObjectQpaque.name = imageFilename + "_Opaque";
        mGameObjectQpaque.AddComponent<SpriteRenderer>().sprite = mBaseSpriteOpaque;
        mGameObjectQpaque.GetComponent<SpriteRenderer>().sortingLayerName = "Opaque";

        mBaseSpriteTransparent = CreateTransparentView(mBaseSpriteOpaque.texture);
        mGameObjectTransparent = new GameObject();
        mGameObjectTransparent.name = imageFilename + "_Opaque";
        mGameObjectTransparent.AddComponent<SpriteRenderer>().sprite = mBaseSpriteTransparent;
        mGameObjectTransparent.GetComponent<SpriteRenderer>().sortingLayerName = "Transparent";
        mGameObjectTransparent.transform.localPosition = new Vector3(
            gameObject.transform.localPosition.x,
            gameObject.transform.localPosition.y,
            gameObject.transform.localPosition.z);

        mGameObjectTransparent.transform.localScale = new Vector3(
            mGameObjectTransparent.transform.localScale.x * puzzle_Scale,
            mGameObjectTransparent.transform.localScale.y * puzzle_Scale,
            mGameObjectTransparent.transform.localScale.z);

        mGameObjectQpaque.gameObject.SetActive(false);

        //SetCameraPosition();

        puzzleManager.SetTotalTiles(numRandomTiles);

        //CreateJigsawTiles();
        StartCoroutine(Coroutine_CreateJigsawTiles());

        mGameObjectQpaque.transform.SetParent(parentForTiles);
        mGameObjectTransparent.transform.SetParent(parentForTiles);
    }

    Sprite CreateTransparentView(Texture2D tex)
    {
        Texture2D newTex = new Texture2D(
            tex.width,
            tex.height,
            TextureFormat.ARGB32,
            false);

        for (int x = 0; x < newTex.width; x++)
        {
            for (int y = 0; y < newTex.height; y++) 
            {
                Color c = tex.GetPixel(x, y);
                if(x > Tile.padding && 
                    x < (newTex.width - Tile.padding) &&
                    y > Tile.padding && 
                    y < (newTex.height - Tile.padding))
                {
                    c.a = ghostTransparency;
                }
                newTex.SetPixel(x, y, c);
            }
        }
        newTex.Apply();

        Sprite sprite = SpriteUtils.CreateSpriteFromTexture2D(
            newTex,
            0,
            0,
            newTex.width,
            newTex.height);
        return sprite;
    }

    void SetCameraPosition()
    {
        Camera.main.transform.position = new Vector3(mBaseSpriteOpaque.texture.width / 2, mBaseSpriteOpaque.texture.height / 2, -650.0f);
        Camera.main.orthographicSize = mBaseSpriteOpaque.texture.width / 2;
    }

    public static Mesh CreateMeshFromSprite(Sprite sprite)
    {

        Mesh mesh = new Mesh();
        Vector3[] vertices2D = Array.ConvertAll(sprite.vertices, v => new Vector3(v.x, v.y, 0));

        // ���(face) �޽�
        List<Vector3> vertices3D = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uv = new List<Vector2>();  // UV ��ǥ ����Ʈ �߰�

        foreach (var vert in vertices2D) // �ո�
        {
            vertices3D.Add(vert);
        }
        foreach (var vert in vertices2D) // �޸�
        {
            vertices3D.Add(new Vector3(vert.x, vert.y));
        }

        // �ﰢ�� ����
        for (int i = 0; i < sprite.triangles.Length; i += 3)
        {
            triangles.Add(sprite.triangles[i]);  // ù ��°, �� ��°, �� ��° ������ ����
            triangles.Add(sprite.triangles[i + 1]);
            triangles.Add(sprite.triangles[i + 2]);
        }


        // UV ��ǥ ����
        for (int i = 0; i < sprite.uv.Length; i++)
        {
            uv.Add(sprite.uv[i]);
        }

        // UV ��ǥ�� �� �� ��� �����ϰ� ����
        for (int i = 0; i < sprite.uv.Length; i++)
        {
            uv.Add(sprite.uv[i]);
        }


        mesh.vertices = vertices3D.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();  // UV ��ǥ�� �޽��� ����
        mesh.RecalculateNormals();

        return mesh;
    }

    public static GameObject CreateGameObjectFromTile(Tile tile)
    {
        GameObject obj = new GameObject();

        obj.name = "TileGameObe_" + tile.xIndex + "_" + tile.yIndex;
        obj.transform.position = new Vector3(tile.xIndex * Tile.tileSize, tile.yIndex * Tile.tileSize, 0.0f);

        // MeshRenderer�� MeshFilter �߰�
        MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = obj.AddComponent<MeshFilter>();

        // Sprite���� Mesh�� ��ȯ
        Sprite sprite = SpriteUtils.CreateSpriteFromTexture2D(
            tile.finalCut,
            0,
            0,
            Tile.padding * 2 + Tile.tileSize,
            Tile.padding * 2 + Tile.tileSize
        );

        // ������ Sprite�� ����Ͽ� Mesh ���� (�β� ����)
        Mesh tileMesh = CreateMeshFromSprite(sprite);
        meshFilter.mesh = tileMesh;

        // Material ���� (Sprite�� �⺻ ���̴� ���)
        Material material = new Material(Shader.Find("Sprites/Default"));
        material.mainTexture = sprite.texture;  // Sprite�� �ؽ�ó�� Material�� ����
        meshRenderer.material = material;

        return obj;
    }




    IEnumerator Coroutine_CreateJigsawTiles()
    {
        Texture2D baseTexture = mBaseSpriteOpaque.texture;
        numTileX = baseTexture.width / Tile.tileSize;
        numTileY = baseTexture.height / Tile.tileSize;

        if (numTileX <= 0 || numTileY <= 0)
        {
            Debug.LogError("Invalid tile size calculation. numTileX or numTileY is less than or equal to zero.");
            yield break; // or handle the error as needed
        }

        mTiles = new Tile[numTileX, numTileY];
        mTileGameObjects = new GameObject[numTileX, numTileY];

        List<Vector2Int> randomTileIndices = new List<Vector2Int>();

        // ���� Ÿ�� �ε��� ����
        while (randomTileIndices.Count < numRandomTiles)
        {
            int randomX = UnityEngine.Random.Range(0, numTileX);
            int randomY = UnityEngine.Random.Range(0, numTileY);

            Vector2Int randomIndex = new Vector2Int(randomX, randomY);
            if (!randomTileIndices.Contains(randomIndex)) // �ߺ����� �ʵ��� Ȯ��
            {
                randomTileIndices.Add(randomIndex);
            }
        }

        // ���� ��ġ ��ġ ����
        List<Vector2> randomPositions = new List<Vector2>();

        for (int i = 0; i < numTileX; i++) 
        {
            for (int j = 0; j < numTileY; j++) 
            {

                mTiles[i, j] = CreateTile(i, j, baseTexture);
                // ���ο� ������Ʈ ����(3D����ȭ)
                GameObject puzzle_Tile_3D = new GameObject($"TileGroup_{i}_{j}");
                Vector3 originalPosition = new Vector3();

                for (int z = 0; z<20; ++z)
                {
                    mTileGameObjects[i, j] = CreateGameObjectFromTile(mTiles[i, j]);

                    mTileGameObjects[i, j].transform.localPosition =
                        new Vector3(mTileGameObjects[i, j].transform.localPosition.x * puzzle_Scale,
                        mTileGameObjects[i, j].transform.localPosition.y * puzzle_Scale,
                        mTileGameObjects[i, j].transform.localPosition.z -(z * 0.1f));

                    mTileGameObjects[i, j].transform.localScale =
                        new Vector3(mTileGameObjects[i, j].transform.localScale.x * puzzle_Scale,
                        mTileGameObjects[i, j].transform.localScale.y * puzzle_Scale,
                        mTileGameObjects[i, j].transform.localScale.z);

                    if (z == 0) 
                    { 
                        originalPosition = mTileGameObjects[i, j].transform.localPosition; //���� ��ġ ����
                    }

                    mTileGameObjects[i, j].transform.localPosition =
                        new Vector3(0,0,0 - (z * puzzle_Scale_Instance/2));
                    // 3D Ÿ�� ������Ʈ�� �ڽ����� ����
                    mTileGameObjects[i, j].transform.SetParent(puzzle_Tile_3D.transform);
                }
                // 3D Ÿ�Ϸ� �ڽĵ��� �޽� ����
                CombineMeshes(puzzle_Tile_3D);

                puzzle_Tile_3D.name = "TileGameObe_" + i + "_" + j;
                if (parentForTiles != null)
                {
                    puzzle_Tile_3D.transform.SetParent(parentForTiles);
                }
                puzzle_Tile_3D.transform.localPosition = originalPosition;

                BoxCollider box = puzzle_Tile_3D.AddComponent<BoxCollider>();

                // TileMovement �߰� �� ����
                TileMovement tileMovement = puzzle_Tile_3D.AddComponent<TileMovement>();
                tileMovement.tile = mTiles[i, j];
                tileMovement.SetPuzzleManager(puzzleManager); // PuzzleManager ����

                Rigidbody rigidbody = puzzle_Tile_3D.AddComponent<Rigidbody>();
                rigidbody.useGravity = false;
                rigidbody.isKinematic = true;

                XRGrabInteractable xrGrabInteractable = puzzle_Tile_3D.AddComponent<XRGrabInteractable>();
                xrGrabInteractable.useDynamicAttach = true;
                xrGrabInteractable.selectMode = InteractableSelectMode.Multiple;

                // �������� ���õ� Ÿ�ϸ� ���� ��ġ�� ��ġ
                if (randomTileIndices.Contains(new Vector2Int(i, j)))
                {
                    Vector2 randomPosition = GenerateRandomPosition(randomPositions);
                    randomPositions.Add(randomPosition);
                    puzzle_Tile_3D.transform.localPosition = new Vector3(randomPosition.x, randomPosition.y, 0);
                }

                // �����ϰ� ���õ� Ÿ���� �ν����Ϳ��� ������ ��ġ�� ��ġ
                //if (randomTileIndices.Contains(new Vector2Int(i, j)))
                //{
                //    int randomIndex = randomTileIndices.IndexOf(new Vector2Int(i, j));
                //    if (randomIndex < randomTilePositions.Length)
                //    {
                //        puzzle_Tile_3D.transform.localPosition = randomTilePositions[randomIndex];
                //    }
                //    else
                //    {
                //        Debug.LogError("randomTilePositions �迭�� ���̰� ������� �ʽ��ϴ�.");
                //    }
                //}
                else
                {
                    // ��Ȱ��ȭ�� Ÿ���� �̵� �Ұ���
                    tileMovement.DisableTileCollider();
                }

                

                yield return null;
            }
        }
    }

    Vector2 GenerateRandomPosition(List<Vector2> existingPositions)
    {
        Vector2 randomPosition = Vector2.zero; // �⺻��
        bool positionValid = false;

        while (!positionValid)
        {
            // X�� ���� ����
            float x = UnityEngine.Random.Range(-1.0f, 1.5f);

            // Y�� ���� ����
            float y = UnityEngine.Random.Range(-0.55f, 1.2f);

            randomPosition = new Vector2(x, y);

            // ��ġ ���� Ȯ��: X�� Y ��� 0~0.8 ������ ������ �ٽ� ����
            if (x >= 0 && x <= 0.8f && y >= 0 && y <= 0.8f)
            {
                continue; // ���ǿ� ���� �����Ƿ� �ٽ� ���� ����
            }

            // ���� ��ġ�� ��ħ ���� Ȯ��
            positionValid = true; // �⺻��: ��ȿ
            foreach (Vector2 existing in existingPositions)
            {
                // X, Y���� �Ÿ� ���
                float distanceX = Mathf.Abs(existing.x - randomPosition.x);
                float distanceY = Mathf.Abs(existing.y - randomPosition.y);

                // ��ħ ���� �Ǵ�: 0.05���� ���� ��ġ�� �ٽ� ����
                if (distanceX < 0.05f && distanceY < 0.05f)
                {
                    positionValid = false; // �ߺ� �߻�
                    break; // �ٸ� ���� ��ġ�͵� Ȯ������ �ʰ� �ٽ� ���� ����
                }
            }
        }

        return randomPosition;
    }



    private void CombineMeshes(GameObject parentObject)
    {
        MeshFilter[] meshFilters = parentObject.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }

        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine);

        MeshFilter meshFilter = parentObject.AddComponent<MeshFilter>();
        meshFilter.mesh = combinedMesh;

        MeshRenderer meshRenderer = parentObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = meshFilters[0].GetComponent<MeshRenderer>().sharedMaterial;
    }


    Tile CreateTile(int i, int j, Texture2D baseTexture)
    {
        Tile tile = new Tile(baseTexture);
        tile.xIndex = i;
        tile.yIndex = j;

        // Left side tiles.
        if (i == 0)
        {
            tile.SetCurveType(Tile.Direction.LEFT, Tile.PosNegType.NONE);
        }
        else
        {
            // We have to create a tile that has LEFT direction opposite curve type.
            Tile leftTile = mTiles[i - 1, j];
            Tile.PosNegType rightOp = leftTile.GetCurveType(Tile.Direction.RIGHT);
            tile.SetCurveType(Tile.Direction.LEFT, rightOp == Tile.PosNegType.NEG ?
              Tile.PosNegType.POS : Tile.PosNegType.NEG);
        }

        // Bottom side tiles
        if (j == 0)
        {
            tile.SetCurveType(Tile.Direction.DOWN, Tile.PosNegType.NONE);
        }
        else
        {
            Tile downTile = mTiles[i, j - 1];
            Tile.PosNegType upOp = downTile.GetCurveType(Tile.Direction.UP);
            tile.SetCurveType(Tile.Direction.DOWN, upOp == Tile.PosNegType.NEG ?
              Tile.PosNegType.POS : Tile.PosNegType.NEG);
        }

        // Right side tiles.
        if (i == numTileX - 1)
        {
            tile.SetCurveType(Tile.Direction.RIGHT, Tile.PosNegType.NONE);
        }
        else
        {
            float toss = UnityEngine.Random.Range(0f, 1f);
            if (toss < 0.5f)
            {
                tile.SetCurveType(Tile.Direction.RIGHT, Tile.PosNegType.POS);
            }
            else
            {
                tile.SetCurveType(Tile.Direction.RIGHT, Tile.PosNegType.NEG);
            }
        }

        // Up side tile.
        if (j == numTileY - 1)
        {
            tile.SetCurveType(Tile.Direction.UP, Tile.PosNegType.NONE);
        }
        else
        {
            float toss = UnityEngine.Random.Range(0f, 1f);
            if (toss < 0.5f)
            {
                tile.SetCurveType(Tile.Direction.UP, Tile.PosNegType.POS);
            }
            else
            {
                tile.SetCurveType(Tile.Direction.UP, Tile.PosNegType.NEG);
            }
        }

        tile.Apply();
        return tile;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
