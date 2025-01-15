using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public InventoryUI inventoryUI;

    private bool isInventoryOpen = false;

    private NetworkObjectManager networkObjectMng = null;

    //public Transform position;

    // InventoryItem ����Ʈ �߰�
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

        networkObjectMng = FindAnyObjectByType<NetworkObjectManager>();

        inventoryUI = FindAnyObjectByType<InventoryUI>(FindObjectsInactive.Include);

        //inventoryUI.positionSource = position;
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
        // ������ UI ����
        GameObject invenItemGo = Instantiate(inventoryUI.invenItemPrefab, inventoryUI.invenContentTr);
        InventoryItem inventoryItem = invenItemGo.GetComponent<InventoryItem>();

        // �ʱ�ȭ
        inventoryItem.Init(itemInfo);

        // ����Ʈ�� �߰�
        inventoryItems.Add(inventoryItem);
    }

    public void RemoveItemFromInventory(InventoryItem inventoryItem)
    {
        if (inventoryItem != null && inventoryItems.Contains(inventoryItem))
        {
            // ����Ʈ���� ����
            inventoryItems.Remove(inventoryItem);

            // UI ������Ʈ ����
            Destroy(inventoryItem.gameObject);
        }
    }

    public void SpawnItemInScene(InventoryItem inventoryItem)
    {
        if (inventoryItem != null && inventoryItem.ItemInfo != null)
        {
            var itemInfo = inventoryItem.ItemInfo;

            // ������ �������� ��ġ�ϰ�, UI���� ����
            itemInfo.gameObject.SetActive(true);
            
            Vector3 iPosition = inventoryUI.positionSource.position;

            itemInfo.transform.position = iPosition + Vector3.forward * 1f;

            // ���� ��Ʈ��ũ ������Ʈ �Ŵ����� �ִٸ� ��Ʈ��ũ ������Ʈ �Ŵ����� ���ؼ� �ٸ� Ŭ���̾�Ʈ���Ե� �̿� ���ؼ� �˸���.
            if (networkObjectMng != null)
            {
                PhotonView view = itemInfo.GetComponent<PhotonView>();
                networkObjectMng.SetObjectActive(view, true);
                networkObjectMng.SetObjectPosition(view, transform.position);
            }

            //inventoryItem.ItemInfo.transform.position = new Vector3(0, 1, 0); // ���ϴ� ��ġ�� ��ġ
            RemoveItemFromInventory(inventoryItem); // UI���� ������ ����
        }
    }

    public InventoryItem GetItemFromInventory(int index)
    {
        return index >= 0 && index < inventoryItems.Count ? inventoryItems[index] : null;
    }
}
