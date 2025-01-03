using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public InventoryUI inventoryUI;

    private bool isInventoryOpen = false;

    // InventoryItem 리스트 추가
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;

        if (isInventoryOpen)
        {
            inventoryUI.OpenInventory();
        }
        else
        {
            inventoryUI.CloseInventory();
        }
    }

    public void AddItemToInventory(Item_Test itemInfo)
    {
        // 아이템 UI 생성
        GameObject invenItemGo = Instantiate(inventoryUI.invenItemPrefab, inventoryUI.invenContentTr);
        InventoryItem inventoryItem = invenItemGo.GetComponent<InventoryItem>();

        // 초기화
        inventoryItem.Init(itemInfo);

        // 리스트에 추가
        inventoryItems.Add(inventoryItem);
    }

    public void RemoveItemFromInventory(InventoryItem inventoryItem)
    {
        if (inventoryItem != null && inventoryItems.Contains(inventoryItem))
        {
            // 리스트에서 제거
            inventoryItems.Remove(inventoryItem);

            // UI 오브젝트 제거
            Destroy(inventoryItem.gameObject);
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

    public InventoryItem GetItemFromInventory(int index)
    {
        return index >= 0 && index < inventoryItems.Count ? inventoryItems[index] : null;
    }
}
