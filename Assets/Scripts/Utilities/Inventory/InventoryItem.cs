using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    private Item_Test itemInfo;

    [SerializeField]
    private Button itemButton;

    public Item_Test ItemInfo => itemInfo;

    private void Awake()
    {
        // 버튼에 클릭 이벤트 연결
        itemButton.onClick.AddListener(OnItemButtonClick);
    }

    public void Init(Item_Test _itemInfo)
    {
        Image img = GetComponent<Image>();
        img.color = _itemInfo.itemColor;
        itemInfo = _itemInfo;
    }


    // 버튼 클릭 시 호출되는 함수
    private void OnItemButtonClick()
    {
        // 버튼 클릭 시 아이템을 씬에 배치하는 요청
        InventoryManager.instance?.SpawnItemInScene(this);
    }
}