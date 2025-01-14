using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ItemCopy : MonoBehaviour
{
    [Header("Duplication Settings")]
    public float twoHandDistance = 1f; // ���� ���� �Ÿ�
    public float minSpeedForDuplication = 2f; // ���� �ּ� �ӵ�
    public Color grayColor = Color.gray; // ����� ������Ʈ�� ����
    public float spawnDistanceFromCamera = 1f; // ī�޶� �� ���� �Ÿ�

    //[Header("Hand References")]
    //[SerializeField] private Transform leftHand;
    //[SerializeField] private Transform rightHand;

    private PlayerManager playerMng = null;

    private XRGrabInteractable grabInteractable;
    private Renderer objectRenderer;
    private Rigidbody objectRigidbody;

    private NetworkObjectManager networkObjectMng = null;

    private float previousDistance = -1f; // ���� ������ �� �� �� �Ÿ�
    private float currentSpeed = 0f; // �� ���� ��� �ӵ�

    private bool hasDuplicated = false;

    void Awake()
    {
        // �ʼ� ������Ʈ ��������
        grabInteractable = GetComponent<XRGrabInteractable>();
        objectRenderer = GetComponent<Renderer>();
        objectRigidbody = GetComponent<Rigidbody>();

        networkObjectMng = FindAnyObjectByType<NetworkObjectManager>();

        ValidateComponents();

        NetworkManager networkMng = FindAnyObjectByType<NetworkManager>();

        networkMng.OnPlayerSpawned += new NetworkManager.OnPlayerSpawnedDelegate(InitPlayerManager);
    }

    void Update()
    {
        if (!grabInteractable.isSelected) return;
        if (IsHeldByBothHands())
        {
            UpdateSpeed();

            if (CanDuplicate() && !hasDuplicated)
            {
                StartCoroutine(DuplicateObject());
                hasDuplicated = true; // ���� �Ϸ�
            }
        }
        else
        {
            ResetDuplication(); // �� ������ ��� ���� ���� ��� �ʱ�ȭ
        }
    }

    private void InitPlayerManager()
    {
        playerMng = FindAnyObjectByType<PlayerManager>();
    }

    private void ValidateComponents()
    {
        if (grabInteractable == null)
            Debug.LogError("XRGrabInteractable ������Ʈ�� �ʿ��մϴ�.");

        //if (leftHand == null || rightHand == null)
        //    Debug.LogError("LeftHand �Ǵ� RightHand Transform�� �����ؾ� �մϴ�.");
    }

    private bool IsHeldByBothHands() => grabInteractable.interactorsSelecting.Count == 2;

    private void UpdateSpeed()
    {
        float currentDistance = playerMng.GetHandDistance();

        if (previousDistance >= 0f)
        {
            currentSpeed = Mathf.Abs(currentDistance - previousDistance) / Time.deltaTime;
        }

        previousDistance = currentDistance;
    }

    private bool CanDuplicate() =>
        playerMng.GetHandDistance() >= twoHandDistance && currentSpeed >= minSpeedForDuplication;


    private IEnumerator DuplicateObject()
    {
        // ī�޶� �������� ���� ��ġ ���
        Transform cameraTransform = Camera.main.transform;
        Vector3 spawnPosition = cameraTransform.position + cameraTransform.forward * spawnDistanceFromCamera;

        //GameObject duplicatedObject = Instantiate(gameObject, spawnPosition, transform.rotation);
        GameObject duplicatedObject = networkObjectMng.InstantiateObject(name, spawnPosition, transform.rotation);

        yield return new WaitForSeconds(0.75f);

        // �ߺ� ������ ���� ����� ������Ʈ���� �� ��ũ��Ʈ ����
        Destroy(duplicatedObject.GetComponent<ItemCopy>());

        ApplyGrayColor(duplicatedObject);
        EnableRigidbody(duplicatedObject);
    }

    private void ApplyGrayColor(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = grayColor;
        }
    }

    private void EnableRigidbody(GameObject obj)
    {
        Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
        }

        // ī�޶��� ���� �������� ���� ����
        Vector3 forceDirection = Camera.main.transform.forward;
        rigidbody.AddForce(forceDirection * 1f, ForceMode.Impulse); // ƨ��� ��
    }

    private void ResetDuplication()
    {
        hasDuplicated = false; // �ʱ�ȭ
        previousDistance = -1f; // �Ÿ� �ʱ�ȭ
        currentSpeed = 0f; // �ӵ� �ʱ�ȭ
    }
}
