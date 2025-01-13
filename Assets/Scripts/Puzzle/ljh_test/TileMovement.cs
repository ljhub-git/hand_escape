
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
    private bool isInCorrectPosition = false; // Ÿ���� ��Ȯ�� ��ġ�� �ִ��� Ȯ���ϴ� �÷���

    private MeshRenderer mSpriteRenderer;

    public delegate void DelegateOnTileInPlace(TileMovement tm);
    public DelegateOnTileInPlace onTileInPlace;

    private Jigsaw_PuzzleManager puzzleManager;  // PuzzleManager ����

    private XRGrabInteractable xrGrabInteractable;

    // ���� ���� ����
    public float positionTolerance = 12f; // ��ġ ���� ����
    public float rotationTolerance = 13f; // ���� ���� ����

    private PhotonView photonView;

    void Start()
    {
        mSpriteRenderer = GetComponent<MeshRenderer>();

        xrGrabInteractable = GetComponent<XRGrabInteractable>();

        photonView = GetComponent<PhotonView>();

        // XRGrabInteractable �̺�Ʈ �ڵ鷯 ���
        if (xrGrabInteractable != null)
        {
            xrGrabInteractable.selectExited.AddListener(OnSelectExited);
        }
    }

    // PuzzleManager �������� ����
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
        // �̺�Ʈ�� Ʈ���ŵ� ���� ������ ����
        CheckAndSetCorrectPosition();
    }

    private void CheckAndSetCorrectPosition()
    {
        // �ùٸ� ��ġ�� �ִ��� Ȯ��
        if (!isInCorrectPosition)
        {
            float dist = (transform.position - GetCorrectPosition()).magnitude;
            float angle = Quaternion.Angle(transform.localRotation, Quaternion.Euler(0, 0, 0));

            Debug.Log($"Current Position: {transform.position}, Correct Position: {GetCorrectPosition()}, Distance: {dist}");
            Debug.Log($"Current Rotation: {transform.localRotation.eulerAngles}, Correct Rotation: (0, 0, 0), Angle: {angle}");

            if (dist <= positionTolerance && angle <= rotationTolerance)  // ��Ȯ�� ��ġ�� ������� Ȯ��
            {
                // Ÿ���� �ùٸ� ��ġ�� ���̸�
                transform.localPosition = GetCorrectPosition();
                transform.localRotation = Quaternion.Euler(0, 0, 0); // ������ (0, 0, 0)���� ����
                isInCorrectPosition = true;  // Ÿ���� ��Ȯ�� ��ġ�� �����ٰ� ����

                puzzleManager.OnTilePlaced(this);  // ���� �Ŵ����� �˸�
                puzzleManager.Succes_Puzzle_Sound();

                photonView.RPC("SyncTilePositionAndRotation", RpcTarget.Others, transform.position, transform.rotation); // �ٸ� �÷��̾�� ����ȭ
            }
        }
    }

    [PunRPC] public void SyncTilePositionAndRotation(Vector3 position, Quaternion rotation) 
    { 
        transform.position = position; 
        transform.rotation = rotation; 
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