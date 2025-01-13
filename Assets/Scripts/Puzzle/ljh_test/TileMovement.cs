
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using Photon.Pun;

public class TileMovement : MonoBehaviour
{
    public Tile tile { get; set; }
    private Vector3 mOffset = new Vector3(0.0f, 0.0f, 0.0f);
    private bool isInCorrectPosition = false; // 타일이 정확한 위치에 있는지 확인하는 플래그

    private MeshRenderer mSpriteRenderer;

    public delegate void DelegateOnTileInPlace(TileMovement tm);
    public DelegateOnTileInPlace onTileInPlace;

    private Jigsaw_PuzzleManager puzzleManager;  // PuzzleManager 참조

    private XRGrabInteractable xrGrabInteractable;

    // 오차 범위 설정
    public float positionTolerance = 12f; // 위치 오차 범위
    public float rotationTolerance = 13f; // 각도 오차 범위

    private PhotonView photonView;

    void Start()
    {
        mSpriteRenderer = GetComponent<MeshRenderer>();

        xrGrabInteractable = GetComponent<XRGrabInteractable>();

        photonView = GetComponent<PhotonView>();

        // XRGrabInteractable 이벤트 핸들러 등록
        if (xrGrabInteractable != null)
        {
            xrGrabInteractable.selectExited.AddListener(OnSelectExited);
        }
    }

    // PuzzleManager 동적으로 설정
    public void SetPuzzleManager(Jigsaw_PuzzleManager manager)
    {
        puzzleManager = manager;
        puzzleManager.RegisterTile(this);
    }

    private Vector3 GetCorrectPosition()
    {
        return new Vector3(tile.xIndex * Tile.tileSize * BoardGen.puzzle_Scale, tile.yIndex * Tile.tileSize * BoardGen.puzzle_Scale, 0.0f);
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        mOffset = transform.position - Camera.main.ScreenToWorldPoint(
          new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f));

        Tile.tilesSorting.BringToTop(mSpriteRenderer);
    }

    private void OnMouseDrag()
    {

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + mOffset;
        transform.position = curPosition;
    }

    private void OnMouseUp()
    {

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        CheckAndSetCorrectPosition();
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        // 이벤트가 트리거될 때의 동작을 정의
        CheckAndSetCorrectPosition();
    }

    private void CheckAndSetCorrectPosition()
    {
        // 올바른 위치에 있는지 확인
        if (!isInCorrectPosition)
        {
            float dist = (transform.position - GetCorrectPosition()).magnitude;
            float angle = Quaternion.Angle(transform.localRotation, Quaternion.Euler(0, 0, 0));

            Debug.Log($"Current Position: {transform.position}, Correct Position: {GetCorrectPosition()}, Distance: {dist}");
            Debug.Log($"Current Rotation: {transform.localRotation.eulerAngles}, Correct Rotation: (0, 0, 0), Angle: {angle}");

            if (dist <= positionTolerance && angle <= rotationTolerance)  // 정확한 위치에 가까운지 확인
            {
                // 타일이 올바른 위치에 놓이면
                transform.localPosition = GetCorrectPosition();
                transform.localRotation = Quaternion.Euler(0, 0, 0); // 각도를 (0, 0, 0)으로 설정
                isInCorrectPosition = true;  // 타일이 정확한 위치에 놓였다고 설정

                puzzleManager.OnTilePlaced(this);  // 퍼즐 매니저에 알림
                puzzleManager.Succes_Puzzle_Sound();

                photonView.RPC("SyncTilePositionAndRotation", RpcTarget.Others, transform.position, transform.rotation); // 다른 플레이어에게 동기화
            }
        }
    }

    [PunRPC] public void SyncTilePositionAndRotation(Vector3 position, Quaternion rotation) 
    { 
        transform.position = position; 
        transform.rotation = rotation; 
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) 
    { 
        if (stream.IsWriting) 
        { 
            stream.SendNext(transform.position); stream.SendNext(transform.rotation); 
        }
        else 
        { 
            transform.position = (Vector3)stream.ReceiveNext(); 
            transform.rotation = (Quaternion)stream.ReceiveNext(); 
        } 
    }

    // Update is called once per frame
    void Update()
    {
    }
}