using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI instance;

    [SerializeField]
    private Transform invenContentTr = null;
    [SerializeField]
    private GameObject invenItemPrefab = null;

    private List<InventoryItem> inventoryItems = new List<InventoryItem>();
    [SerializeField]
    private float m_MaxScale = 1.0f;

    [SerializeField]
    private float m_MinScale = 1.0f;

    [SerializeField]
    private float m_MaxDistance = 3.5f;

    [SerializeField]
    private float m_MinDistance = 0.25f;

    private Vector3 m_StartingScale = Vector3.one;

    public float distance = 0.3f;
    public float verticalOffset = -0.2f;

    public Transform positionSource;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // Singleton 설정
        }
        else
        {
            Destroy(gameObject); // 이미 존재하는 경우 삭제
        }
    }

    private void OnDestroy()
    {
        if (instance == this) instance = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Items"))
        {
            // 충돌한 객체에 Item_Test 컴포넌트가 있는지 확인
            Item_Test item = other.GetComponent<Item_Test>();

            //if (item != null && item.isBeingDragged)
            if (item != null && 
                (item.XRGrabInteractable.isSelected || item.XRGrabInteractable.isHovered))
            {
                // 충돌한 객체가 Item_Test를 가지고 있으면 인벤토리에 아이템을 추가
                item.isBeingDragged = false;
                //SpawnInvenItem(item);
                AddItemToInventory(item);
                other.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            OpenKeyboard();
        }
    }
    public void OpenKeyboard()
    {
        Vector3 direction = positionSource.forward;
        direction.y = 0;
        direction.Normalize();

        Vector3 targetPosition = positionSource.position + direction * distance + Vector3.up * verticalOffset;
        RepositionKeyboard(targetPosition);
    }

    public void RepositionKeyboard(Vector3 kbPos, float verticalOffset = 0.0f)
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

    public void AddItemToInventory(Item_Test itemInfo)
    {
        // 아이템 UI 생성
        GameObject invenItemGo = Instantiate(invenItemPrefab, invenContentTr);
        InventoryItem inventoryItem = invenItemGo.GetComponent<InventoryItem>();

        // 초기화
        inventoryItem.Init(itemInfo);

        // 리스트에 아이템 추가
        inventoryItems.Add(inventoryItem);
    }

    public void RemoveItemFromInventory(InventoryItem inventoryItem)
    {
        if (inventoryItems.Contains(inventoryItem))
        {
            inventoryItems.Remove(inventoryItem);
            Destroy(inventoryItem.gameObject); // UI에서 아이템 제거
        }
    }

    public void SpawnItemInScene(InventoryItem inventoryItem)
    {
        if (inventoryItem != null && inventoryItem.ItemInfo != null)
        {
            // 씬으로 아이템을 배치하고, UI에서 삭제
            inventoryItem.ItemInfo.gameObject.SetActive(true);
            //inventoryItem.ItemInfo.transform.position = new Vector3(0, 1, 0); // 원하는 위치에 배치
            RemoveItemFromInventory(inventoryItem); // UI에서 아이템 삭제
        }
    }
}
