using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TileMovement : MonoBehaviour
{
    public Tile tile { get; set; }
    private Vector3 mOffset = new Vector3(0.0f, 0.0f, 0.0f);
    private bool isInCorrectPosition = false; // Ÿ���� ��Ȯ�� ��ġ�� �ִ��� Ȯ���ϴ� �÷���

    private MeshRenderer mSpriteRenderer;

    public delegate void DelegateOnTileInPlace(TileMovement tm);
    public DelegateOnTileInPlace onTileInPlace;

    private Jigsaw_PuzzleManager puzzleManager;  // PuzzleManager ����

    void Start()
    {
        mSpriteRenderer = GetComponent<MeshRenderer>();

    }

    // PuzzleManager �������� ����
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
        Debug.Log("���콺��");
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("���콺��?");
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
        Debug.Log("���콺��!!");
    }

    private void OnMouseUp()
    {

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        // �ùٸ� ��ġ�� �ִ��� Ȯ��
        if (!isInCorrectPosition)
        {
            float dist = (transform.position - GetCorrectPosition()).magnitude;
            if (dist < (20.0f* BoardGen.puzzle_Scale))  // ��Ȯ�� ��ġ�� ������� Ȯ��
            {
                // Ÿ���� �ùٸ� ��ġ�� ���̸�
                transform.position = GetCorrectPosition();
                isInCorrectPosition = true;  // Ÿ���� ��Ȯ�� ��ġ�� �����ٰ� ����

                puzzleManager.OnTilePlaced(this);  // ���� �Ŵ����� �˸�
            }
        }
    }
    // Ÿ���� ��Ȯ�� ��ġ�� �ִ��� ���θ� ��ȯ
    public bool IsCorrectPosition()
    {
        return isInCorrectPosition;
    }

    public void DisableTileCollider()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        if (collider != null)
        {
            collider.enabled = false;  // Collider ��Ȱ��ȭ
        }
    }

    //public void EnableTileCollider()
    //{
    //    BoxCollider2D collider = GetComponent<BoxCollider2D>();
    //    if (collider != null)
    //    {
    //        collider.enabled = true;  // Collider Ȱ��ȭ
    //    }
    //}

    // Update is called once per frame
    void Update()
    {

    }
}