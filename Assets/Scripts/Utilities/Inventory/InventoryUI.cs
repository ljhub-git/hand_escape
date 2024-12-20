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
        // 충돌한 객체에 Item_Test 컴포넌트가 있는지 확인
        Item_Test item = other.GetComponent<Item_Test>();

        if (item != null && item.isBeingDragged)
        {
            // 충돌한 객체가 Item_Test를 가지고 있으면 인벤토리에 아이템을 추가
            SpawnInvenItem(item);
            other.gameObject.SetActive(false);
        }
    }

    public void SpawnInvenItem(Item_Test _itemInfo)
    {
        // Instantiate 후 기본 설정
        GameObject invenItemGo = Instantiate(invenItemPrefab, invenContentTr);
        // InventoryItem 컴포넌트 초기화
        invenItemGo.GetComponent<InventoryItem>().Init(_itemInfo);
    }
}
