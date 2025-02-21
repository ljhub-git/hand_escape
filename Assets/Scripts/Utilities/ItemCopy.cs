using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ItemCopy : MonoBehaviour
{
    [Header("Duplication Settings")]
    public float twoHandDistance = 1f; // 복사 기준 거리
    public float minSpeedForDuplication = 2f; // 복사 최소 속도
    public Color grayColor = Color.gray; // 복사된 오브젝트의 색상
    public float spawnDistanceFromCamera = 1f; // 카메라 앞 복제 거리

    //[Header("Hand References")]
    //[SerializeField] private Transform leftHand;
    //[SerializeField] private Transform rightHand;

    private PlayerManager playerMng = null;

    private XRGrabInteractable grabInteractable;
    private Renderer objectRenderer;
    private Rigidbody objectRigidbody;

    private NetworkObjectManager networkObjectMng = null;

    private float previousDistance = -1f; // 이전 프레임 두 손 간 거리
    private float currentSpeed = 0f; // 두 손의 상대 속도

    private bool hasDuplicated = false;

    void Awake()
    {
        // 필수 컴포넌트 가져오기
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
                hasDuplicated = true; // 복사 완료
            }
        }
        else
        {
            ResetDuplication(); // 두 손으로 잡고 있지 않을 경우 초기화
        }
    }

    private void InitPlayerManager()
    {
        playerMng = FindAnyObjectByType<PlayerManager>();
    }

    private void ValidateComponents()
    {
        if (grabInteractable == null)
            Debug.LogError("XRGrabInteractable 컴포넌트가 필요합니다.");

        //if (leftHand == null || rightHand == null)
        //    Debug.LogError("LeftHand 또는 RightHand Transform을 설정해야 합니다.");
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
        // 카메라 기준으로 복제 위치 계산
        Transform cameraTransform = Camera.main.transform;
        Vector3 spawnPosition = cameraTransform.position + cameraTransform.forward * spawnDistanceFromCamera;

        GameObject duplicatedObject = Instantiate(gameObject, spawnPosition, transform.rotation);
        //GameObject duplicatedObject = networkObjectMng.InstantiateObject(name, spawnPosition, transform.rotation);

        {
            PhotonView photonView = duplicatedObject.GetComponent<PhotonView>();
            photonView.ViewID = 6000;
            if (PhotonNetwork.AllocateViewID(photonView))
            {
                object[] data = new object[]
                {
                    duplicatedObject.transform.position, duplicatedObject.transform.rotation, photonView.ViewID
                };

                RaiseEventOptions raiseEventOptions = new RaiseEventOptions
                {
                    Receivers = ReceiverGroup.Others,
                    CachingOption = EventCaching.AddToRoomCache
                };

                SendOptions sendOptions = new SendOptions
                {
                    Reliability = true
                };

                PhotonNetwork.RaiseEvent(1, data, raiseEventOptions, sendOptions);
            }
            else
            {
                Debug.LogError("Failed to allocate a ViewId.");

                Destroy(duplicatedObject);
            }
        }
        

        yield return new WaitForSeconds(0.75f);

        // 중복 방지를 위해 복사된 오브젝트에서 이 스크립트 제거
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

        // 카메라의 전방 방향으로 힘을 적용
        Vector3 forceDirection = Camera.main.transform.forward;
        rigidbody.AddForce(forceDirection * 1f, ForceMode.Impulse); // 튕기는 힘
    }

    private void ResetDuplication()
    {
        hasDuplicated = false; // 초기화
        previousDistance = -1f; // 거리 초기화
        currentSpeed = 0f; // 속도 초기화
    }
}
