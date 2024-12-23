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
            instance = this; // Singleton ����
        }
        else
        {
            Destroy(gameObject); // �̹� �����ϴ� ��� ����
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
            // �浹�� ��ü�� Item_Test ������Ʈ�� �ִ��� Ȯ��
            Item_Test item = other.GetComponent<Item_Test>();

            if (item != null && item.isBeingDragged)
            {
                // �浹�� ��ü�� Item_Test�� ������ ������ �κ��丮�� �������� �߰�
                item.isBeingDragged = false;
                //SpawnInvenItem(item);
                AddItemToInventory(item);
                other.gameObject.SetActive(false);
            }
        }
    }

    public void AddItemToInventory(Item_Test itemInfo)
    {
        // ������ UI ����
        GameObject invenItemGo = Instantiate(invenItemPrefab, invenContentTr);
        InventoryItem inventoryItem = invenItemGo.GetComponent<InventoryItem>();

        // �ʱ�ȭ
        inventoryItem.Init(itemInfo);

        // ����Ʈ�� ������ �߰�
        inventoryItems.Add(inventoryItem);
    }

    public void RemoveItemFromInventory(InventoryItem inventoryItem)
    {
        if (inventoryItems.Contains(inventoryItem))
        {
            inventoryItems.Remove(inventoryItem);
            Destroy(inventoryItem.gameObject); // UI���� ������ ����
        }
    }

    public void SpawnItemInScene(InventoryItem inventoryItem)
    {
        if (inventoryItem != null && inventoryItem.ItemInfo != null)
        {
            // ������ �������� ��ġ�ϰ�, UI���� ����
            inventoryItem.ItemInfo.gameObject.SetActive(true);
            //inventoryItem.ItemInfo.transform.position = new Vector3(0, 1, 0); // ���ϴ� ��ġ�� ��ġ
            RemoveItemFromInventory(inventoryItem); // UI���� ������ ����
        }
    }
}
