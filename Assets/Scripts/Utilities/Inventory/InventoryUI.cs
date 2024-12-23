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

            if (item != null && item.isBeingDragged)
            {
                // 충돌한 객체가 Item_Test를 가지고 있으면 인벤토리에 아이템을 추가
                item.isBeingDragged = false;
                //SpawnInvenItem(item);
                AddItemToInventory(item);
                other.gameObject.SetActive(false);
            }
        }
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
