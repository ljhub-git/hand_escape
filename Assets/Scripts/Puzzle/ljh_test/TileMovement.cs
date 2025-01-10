using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TileMovement : MonoBehaviour
{
    public Tile tile { get; set; }
    private Vector3 mOffset = new Vector3(0.0f, 0.0f, 0.0f);
    private bool isInCorrectPosition = false; // 타일이 정확한 위치에 있는지 확인하는 플래그

    private MeshRenderer mSpriteRenderer;

    public delegate void DelegateOnTileInPlace(TileMovement tm);
    public DelegateOnTileInPlace onTileInPlace;

    private Jigsaw_PuzzleManager puzzleManager;  // PuzzleManager 참조

    void Start()
    {
        mSpriteRenderer = GetComponent<MeshRenderer>();

    }

    // PuzzleManager 동적으로 설정
    public void SetPuzzleManager(Jigsaw_PuzzleManager manager)
    {
        puzzleManager = manager;
        puzzleManager.RegisterTile(this);
    }

    private Vector3 GetCorrectPosition()
    {
        Debug.Log("tile.xIndex : " + tile.xIndex + "Tile.tileSize : " + Tile.tileSize + "BoardGen.puzzle_Scale : " + BoardGen.puzzle_Scale);
        return new Vector3(tile.xIndex * Tile.tileSize * BoardGen.puzzle_Scale, tile.yIndex * Tile.tileSize * BoardGen.puzzle_Scale, 0f);
    }

    private void OnMouseDown()
    {
        Debug.Log("마우스질");
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("마우스질?");
            return;
        }

        mOffset = transform.position - Camera.main.ScreenToWorldPoint(
          new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f));
            

        // For sorting of tiles.
        Tile.tilesSorting.BringToTop(mSpriteRenderer);
    }

    private void OnMouseDrag()
    {

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
        //Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + mOffset;
        transform.position = curPosition;
        Debug.Log("마우스질!!");
    }

    private void OnMouseUp()
    {

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        // 올바른 위치에 있는지 확인
        if (!isInCorrectPosition)
        {
            float dist = (transform.position - GetCorrectPosition()).magnitude;
            if (dist < (20.0f* BoardGen.puzzle_Scale))  // 정확한 위치에 가까운지 확인
            {
                // 타일이 올바른 위치에 놓이면
                transform.position = GetCorrectPosition();
                isInCorrectPosition = true;  // 타일이 정확한 위치에 놓였다고 설정

                puzzleManager.OnTilePlaced(this);  // 퍼즐 매니저에 알림
            }
        }
    }
    // 타일이 정확한 위치에 있는지 여부를 반환
    public bool IsCorrectPosition()
    {
        return isInCorrectPosition;
    }

    public void DisableTileCollider()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        if (collider != null)
        {
            collider.enabled = false;  // Collider 비활성화
        }
    }

    //public void EnableTileCollider()
    //{
    //    BoxCollider2D collider = GetComponent<BoxCollider2D>();
    //    if (collider != null)
    //    {
    //        collider.enabled = true;  // Collider 활성화
    //    }
    //}

    // Update is called once per frame
    void Update()
    {

    }
}