using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

using Photon.Pun;
using Unity.VisualScripting;
using static Photon.Pun.PhotonView;

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

    public int numRandomTiles = 2;  // 랜덤 배치할 타일의 개수를 설정

    public Vector3[] randomTilePositions;
    private Vector3[] initialTilePositions;
    private PhotonView photonView;


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

    void OnValidate()
    { 
        // 인스턴스 변수의 값을 static 변수에 동기화
        //puzzle_Scale = puzzle_Scale_Instance; 
    }

    private void Awake()
    {
        puzzle_Scale = puzzle_Scale_Instance;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        photonView = GetComponent<PhotonView>();

        if (photonView == null)
        {
            Debug.LogError("photonView가 null입니다.");
            return; // 이후 코드 실행 방지
        }


        mBaseSpriteOpaque = LoadBaseTexture();
        //완성그림(비활성화 시킬것)
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

        puzzleManager.SetTotalTiles(numRandomTiles);

        // MasterClient 여부 확인
        if (PhotonNetwork.IsMasterClient)
        {
            // 배경 그림 및 타일 초기 위치 설정
            //photonView.RPC("InitializeTiles", RpcTarget.AllBuffered, initialTilePositions);
            StartCoroutine(Coroutine_CreateJigsawTiles());
        }
        else 
        {
            Debug.LogWarning("photonView.IsMine이 false이며, MasterClient가 아닙니다. 타일을 생성하지 않습니다.");
        }
    }
    void SetInitialTilePositions()
    {
        initialTilePositions = new Vector3[numTileX * numTileY];

        for (int i = 0; i < numTileX; i++)
        {
            for (int j = 0; j < numTileY; j++)
            {
                initialTilePositions[i * numTileY + j] = new Vector3(
                    i * Tile.tileSize * BoardGen.puzzle_Scale,
                    j * Tile.tileSize * BoardGen.puzzle_Scale,
                    0.0f
                );
            }
        }
    }

    //[PunRPC]
    //void InitializeTiles(Vector3[] positions)
    //{
    //    if (positions == null)
    //    {
    //        Debug.LogError("positions 배열이 null입니다."); 
    //        return; // 이후 코드 실행 방지
    //    }

    //    Debug.Log("InitializeTiles 호출");
    //    int index = 0;
    //    for (int i = 0; i < numTileX; i++)
    //    {
    //        for (int j = 0; j < numTileY; j++)
    //        {
    //            if (mTileGameObjects[i, j] == null) 
    //            { 
    //                Debug.LogError($"mTileGameObjects[{i},{j}]가 null입니다."); 
    //                continue; 
    //            }

    //            GameObject tileObject = new GameObject($"TileGroup_{i}_{j}");
    //            mTileGameObjects[i, j] = tileObject;
    //            tileObject.transform.position = positions[index];

    //            // PhotonView 설정
    //            PhotonView view = tileObject.AddComponent<PhotonView>();

    //            // 추가 컴포넌트 설정
    //            tileObject.AddComponent<BoxCollider>();
    //            Rigidbody rigidbody = tileObject.AddComponent<Rigidbody>();
    //            rigidbody.useGravity = false;
    //            rigidbody.isKinematic = true;

    //            XRGrabInteractable xrGrabInteractable = tileObject.AddComponent<XRGrabInteractable>();
    //            xrGrabInteractable.useDynamicAttach = true;

    //            TileMovement tileMovement = tileObject.AddComponent<TileMovement>();
    //            tileMovement.tile = mTiles[i, j];
    //            tileMovement.SetPuzzleManager(puzzleManager);

    //            index++;
    //        }
    //    }
    //}

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

        // 상단(face) 메쉬
        List<Vector3> vertices3D = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uv = new List<Vector2>();  // UV 좌표 리스트 추가

        foreach (var vert in vertices2D) // 앞면
        {
            vertices3D.Add(vert);
        }
        foreach (var vert in vertices2D) // 뒷면
        {
            vertices3D.Add(new Vector3(vert.x, vert.y));
        }

        // 삼각형 설정
        for (int i = 0; i < sprite.triangles.Length; i += 3)
        {
            triangles.Add(sprite.triangles[i]);  // 첫 번째, 세 번째, 두 번째 순서로 변경
            triangles.Add(sprite.triangles[i + 1]);
            triangles.Add(sprite.triangles[i + 2]);
        }


        // UV 좌표 설정
        for (int i = 0; i < sprite.uv.Length; i++)
        {
            uv.Add(sprite.uv[i]);
        }

        // UV 좌표는 두 면 모두 동일하게 설정
        for (int i = 0; i < sprite.uv.Length; i++)
        {
            uv.Add(sprite.uv[i]);
        }


        mesh.vertices = vertices3D.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();  // UV 좌표를 메쉬에 설정
        mesh.RecalculateNormals();

        return mesh;
    }

    public static GameObject CreateGameObjectFromTile(Tile tile)
    {
        GameObject obj = new GameObject();

        obj.name = "TileGameObe_" + tile.xIndex + "_" + tile.yIndex;
        obj.transform.position = new Vector3(tile.xIndex * Tile.tileSize, tile.yIndex * Tile.tileSize, 0.0f);

        // MeshRenderer와 MeshFilter 추가
        MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
        MeshFilter meshFilter = obj.AddComponent<MeshFilter>();

        // Sprite에서 Mesh로 변환
        Sprite sprite = SpriteUtils.CreateSpriteFromTexture2D(
            tile.finalCut,
            0,
            0,
            Tile.padding * 2 + Tile.tileSize,
            Tile.padding * 2 + Tile.tileSize
        );

        // 생성된 Sprite를 사용하여 Mesh 생성 (두께 포함)
        Mesh tileMesh = CreateMeshFromSprite(sprite);
        meshFilter.mesh = tileMesh;

        // Material 설정 (Sprite용 기본 셰이더 사용)
        Material material = new Material(Shader.Find("Sprites/Default"));
        material.mainTexture = sprite.texture;  // Sprite의 텍스처를 Material에 설정
        meshRenderer.material = material;

        return obj;
    }




    IEnumerator Coroutine_CreateJigsawTiles()
    {
        Debug.Log("Coroutine_CreateJigsawTiles 시작");

        if (mBaseSpriteOpaque == null)
        {
            Debug.LogError("mBaseSpriteOpaque가 null입니다.");
            yield break;
        }

        Texture2D baseTexture = mBaseSpriteOpaque.texture;
        Debug.Log($"baseTexture.width: {baseTexture.width}, baseTexture.height: {baseTexture.height}");

        numTileX = baseTexture.width / Tile.tileSize;
        numTileY = baseTexture.height / Tile.tileSize;

        Debug.Log($"numTileX: {numTileX}, numTileY: {numTileY}");

        if (numTileX <= 0 || numTileY <= 0)
        {
            Debug.LogError("Invalid tile size calculation. numTileX or numTileY is less than or equal to zero.");
            yield break;
        }

        // 초기 타일 위치 설정
        SetInitialTilePositions();

        Debug.Log("타일 배열 초기화");

        mTiles = new Tile[numTileX, numTileY];
        mTileGameObjects = new GameObject[numTileX, numTileY];
        List<Vector3> randomTileIndices = new List<Vector3>();
        //List<Vector3> initialPositions = new List<Vector3>();

        

        Debug.Log("타일 생성 시작");

        for (int i = 0; i < numTileX; i++)
        {
            for (int j = 0; j < numTileY; j++)
            {
                Debug.Log($"타일 생성 중: {i}, {j}");

                mTiles[i, j] = CreateTile(i, j, baseTexture);
                GameObject puzzle_Tile_3D = new GameObject($"TileGameObe_{i}_{j}");
                Vector3 originalPosition = new Vector3();

                for (int z = 0; z < 20; ++z)
                {
                    mTileGameObjects[i, j] = CreateGameObjectFromTile(mTiles[i, j]);
                    mTileGameObjects[i, j].transform.localPosition = new Vector3(
                        mTileGameObjects[i, j].transform.localPosition.x * puzzle_Scale,
                        mTileGameObjects[i, j].transform.localPosition.y * puzzle_Scale,
                        mTileGameObjects[i, j].transform.localPosition.z - (z * 0.1f));
                    mTileGameObjects[i, j].transform.localScale = new Vector3(
                        mTileGameObjects[i, j].transform.localScale.x * puzzle_Scale,
                        mTileGameObjects[i, j].transform.localScale.y * puzzle_Scale,
                        mTileGameObjects[i, j].transform.localScale.z);

                    if (z == 0)
                    {
                        originalPosition = mTileGameObjects[i, j].transform.localPosition;
                    }

                    mTileGameObjects[i, j].transform.localPosition = new Vector3(0, 0, 0 - (z * puzzle_Scale_Instance / 2));
                    mTileGameObjects[i, j].transform.SetParent(puzzle_Tile_3D.transform);
                }

                CombineMeshes(puzzle_Tile_3D);
                puzzle_Tile_3D.name = $"TileGameObe_{i}_{j}";

                if (parentForTiles != null)
                {
                    puzzle_Tile_3D.transform.SetParent(parentForTiles);
                }
                puzzle_Tile_3D.transform.localPosition = originalPosition;

                // PhotonView 및 ViewID 설정
                PhotonView photonView = puzzle_Tile_3D.AddComponent<PhotonView>();
                PhotonTransformView photonTransformView = puzzle_Tile_3D.AddComponent<PhotonTransformView>();
                photonView.observableSearch = ObservableSearch.AutoFindAll;
                photonTransformView.m_SynchronizePosition = true;
                photonTransformView.m_SynchronizeRotation = true;
                photonTransformView.m_SynchronizeScale = true;
                photonView.OwnershipTransfer = OwnershipOption.Takeover; // Ownership 설정 추가
                photonView.ObservedComponents = new List<Component> { photonTransformView }; // Observed Components 설정
                int viewID = PhotonNetwork.AllocateViewID(true);
                photonView.ViewID = viewID;
                Debug.Log($"PhotonView ID 할당: {viewID}");

                // 추가 컴포넌트 설정
                BoxCollider box = puzzle_Tile_3D.AddComponent<BoxCollider>();
                Rigidbody rigidbody = puzzle_Tile_3D.AddComponent<Rigidbody>();
                rigidbody.useGravity = false;
                rigidbody.isKinematic = true;

                XRGrabInteractable xrGrabInteractable = puzzle_Tile_3D.AddComponent<XRGrabInteractable>();
                xrGrabInteractable.useDynamicAttach = true;
                xrGrabInteractable.selectMode = InteractableSelectMode.Multiple;

                TileMovement tileMovement = puzzle_Tile_3D.AddComponent<TileMovement>();
                tileMovement.tile = mTiles[i, j];
                tileMovement.SetPuzzleManager(puzzleManager);

                //Debug.Log("랜덤 타일 위치 설정");

                //while (randomTileIndices.Count < numRandomTiles)
                //{
                //    int randomX = UnityEngine.Random.Range(0, numTileX);
                //    int randomY = UnityEngine.Random.Range(0, numTileY);

                //    Vector3 randomIndex = new Vector3(randomX, randomY, 0);
                //    if (!randomTileIndices.Contains(randomIndex))
                //    {
                //        randomTileIndices.Add(randomIndex);
                //        Debug.Log($"RPC 호출 전: SyncRandomTilePosition - {randomIndex} - {randomTilePositions[randomTileIndices.Count - 1]}");
                //        photonView.RPC("SyncRandomTilePosition", RpcTarget.AllBuffered, randomIndex, randomTilePositions[randomTileIndices.Count - 1]);
                //        Debug.Log($"랜덤 타일 위치 설정: {randomIndex} - {randomTilePositions[randomTileIndices.Count - 1]}");
                //    }
                //}

                //// 랜덤 타일 위치 설정
                //if (randomTileIndices.Contains(new Vector3(i, j, 0)))
                //{
                //    int randomIndex = randomTileIndices.IndexOf(new Vector3(i, j, 0));
                //    if (randomIndex < randomTilePositions.Length)
                //    {
                //        puzzle_Tile_3D.transform.localPosition = randomTilePositions[randomIndex];
                //        Debug.Log($"타일 위치 설정: {puzzle_Tile_3D.name} - {randomTilePositions[randomIndex]}");
                //    }
                //    else
                //    {
                //        Debug.LogError("randomTilePositions 배열의 길이가 충분하지 않습니다.");
                //    }
                //}
                //else
                //{
                //    tileMovement.DisableTileCollider();
                //}

                //initialPositions.Add(puzzle_Tile_3D.transform.position);
                photonView.RPC("SyncTilePosition", RpcTarget.All, puzzle_Tile_3D.transform.position);
                Debug.Log($"퍼즐 타일 생성 및 초기화 완료: {puzzle_Tile_3D.name}, ViewID: {viewID}");

                yield return null;
            }
        }
    }
    //[PunRPC]
    //void SyncTilePosition(Vector3 position)
    //{
    //    transform.position = position;
    //}

    [PunRPC]
    void SyncRandomTilePosition(Vector3 index, Vector3 position)
    {
        Debug.Log($"SyncRandomTilePosition 호출: {index} - {position}");

        // mTileGameObjects 배열의 특정 요소가 null인지 확인
        if (mTileGameObjects[(int)index.x, (int)index.y] != null)
        {
            mTileGameObjects[(int)index.x, (int)index.y].transform.position = position;
        }
        else
        {
            Debug.LogError($"mTileGameObjects[{(int)index.x},{(int)index.y}]가 null입니다.");
        }
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
