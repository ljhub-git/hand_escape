using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private Transform invenContentTr = null;
    [SerializeField]
    private GameObject invenItemPrefab = null;


    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ��ü�� Item_Test ������Ʈ�� �ִ��� Ȯ��
        Item_Test item = other.GetComponent<Item_Test>();

        if (item != null && item.isBeingDragged)
        {
            // �浹�� ��ü�� Item_Test�� ������ ������ �κ��丮�� �������� �߰�
            SpawnInvenItem(item);
            other.gameObject.SetActive(false);
        }
    }

    public void SpawnInvenItem(Item_Test _itemInfo)
    {
        // Instantiate �� �⺻ ����
        GameObject invenItemGo = Instantiate(invenItemPrefab, invenContentTr);
        // InventoryItem ������Ʈ �ʱ�ȭ
        invenItemGo.GetComponent<InventoryItem>().Init(_itemInfo);
    }
}
