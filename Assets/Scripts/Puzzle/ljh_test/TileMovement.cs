
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

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
    public float positionTolerance = 0.016f; // ��ġ ���� ����
    public float rotationTolerance = 14f; // ���� ���� ����


    void Start()
    {
        mSpriteRenderer = GetComponent<MeshRenderer>();

        xrGrabInteractable = GetComponent<XRGrabInteractable>();

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
        Debug.Log("���콺��");
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("���콺��?");
            return;
        }

        //mOffset = transform.position - Camera.main.ScreenToWorldPoint(
        //  new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z));
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
            float dist = (transform.localPosition - GetCorrectPosition()).magnitude;
            float angle = Quaternion.Angle(transform.localRotation, Quaternion.Euler(0, 0, 0));

            Debug.Log($"Current Position: {transform.localPosition}, Correct Position: {GetCorrectPosition()}, Distance: {dist}");
            Debug.Log($"Current Rotation: {transform.localRotation.eulerAngles}, Correct Rotation: (0, 0, 0), Angle: {angle}");

            if (dist <= positionTolerance && angle <= rotationTolerance)  // ��Ȯ�� ��ġ�� ������� Ȯ��
            {
                // Ÿ���� �ùٸ� ��ġ�� ���̸�
                transform.localPosition = GetCorrectPosition();
                transform.localRotation = Quaternion.Euler(0, 0, 0); // ������ (0, 0, 0)���� ����
                isInCorrectPosition = true;  // Ÿ���� ��Ȯ�� ��ġ�� �����ٰ� ����

                puzzleManager.OnTilePlaced(this);  // ���� �Ŵ����� �˸�
                puzzleManager.Succes_Puzzle_Sound();
            }
        }
    }

    //private void checkandsetcorrectposition()
    //{
    //    // �ùٸ� ��ġ�� �ִ��� Ȯ��
    //    if (!isincorrectposition)
    //    {
    //        float dist = (transform.position - getcorrectposition()).magnitude;
    //        if (dist < 1f)  // ��Ȯ�� ��ġ�� ������� Ȯ��
    //        {
    //            // Ÿ���� �ùٸ� ��ġ�� ���̸�
    //            transform.localposition = getcorrectposition();
    //            isincorrectposition = true;  // Ÿ���� ��Ȯ�� ��ġ�� �����ٰ� ����

    //            puzzlemanager.ontileplaced(this);  // ���� �Ŵ����� �˸�
    //        }
    //    }
    //}

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

    // Update is called once per frame
    void Update()
    {
    }
}