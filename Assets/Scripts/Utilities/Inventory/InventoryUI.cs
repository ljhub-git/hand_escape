using Photon.Pun;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform invenContentTr = null;
    public GameObject invenItemPrefab = null;

    [SerializeField]
    private float m_MaxScale = 1.0f;

    [SerializeField]
    private float m_MinScale = 0.3f;

    [SerializeField]
    private float m_MaxDistance = 3.5f;

    [SerializeField]
    private float m_MinDistance = 0.5f;

    private Vector3 m_StartingScale = Vector3.one * 0.01f;

    private NetworkObjectManager networkObjectMng = null;

    public float distance = 0.3f;
    public float verticalOffset = -0.2f;

    public Transform positionSource;


    private void Awake()
    {
        networkObjectMng = FindAnyObjectByType<NetworkObjectManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Items"))
        {
            // 충돌한 객체에 Item_Test 컴포넌트가 있는지 확인
            Item_Test item = other.GetComponent<Item_Test>();

            if (item != null &&
                (item.XRGrabInteractable.isSelected || item.XRGrabInteractable.isHovered))
            {
                // 충돌한 객체가 Item_Test를 가지고 있으면 인벤토리에 아이템을 추가
                InventoryManager.instance.AddItemToInventory(item);
                
                other.gameObject.SetActive(false);
                if (networkObjectMng != null)
                    networkObjectMng.SetObjectActive(other.GetComponent<PhotonView>(), false);
            }
        }
    }

    public void OpenInventory()
    {
        Vector3 direction = positionSource.forward;
        direction.y = 0;
        direction.Normalize();
        Vector3 targetPosition = positionSource.position + direction * distance + Vector3.up * verticalOffset;
        RepositionInventory(targetPosition);
        gameObject.SetActive(true);
    }

    public void CloseInventory()
    {
        gameObject.SetActive(false);
    }

    public void RepositionInventory(Vector3 kbPos, float verticalOffset = 0.0f)
    {
        transform.position = kbPos;
        ScaleToSize();
        LookAtTargetOrigin();
    }

    private void ScaleToSize()
    {
        float distance = (transform.position - Camera.main.transform.position).magnitude;
        float distancePercent = (distance - m_MinDistance) / (m_MaxDistance - m_MinDistance);
        float scale = m_MinScale + (m_MaxScale - m_MinScale) * distancePercent;

        scale = Mathf.Clamp(scale, m_MinScale, m_MaxScale);
        transform.localScale = m_StartingScale * scale;

        Debug.LogFormat("Setting scale: {0} for distance: {1}", scale, distance);
    }

    private void LookAtTargetOrigin()
    {
        transform.LookAt(Camera.main.transform.position);
        transform.Rotate(Vector3.up, 180.0f);
    }
}
