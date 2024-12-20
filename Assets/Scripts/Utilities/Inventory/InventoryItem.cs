using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    private Item_Test itemInfo;

    [SerializeField]
    private Button itemButton;

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
        // 아이템을 게임 씬으로 다시 배치
        SpawnItemInScene();
    }

    // 게임 씬에 아이템을 배치하는 메서드
    private void SpawnItemInScene()
    {
        if (itemInfo != null)
        {
            // 새로운 아이템 오브젝트를 생성해서 게임 씬에 배치
            GameObject itemInScene = Instantiate(itemInfo.gameObject);
            itemInScene.transform.position = new Vector3(0, 0, 0); // 원하는 위치에 배치
            itemInfo.gameObject.SetActive(true);
            // 아이템이 다시 씬에 나타났으므로, UI에서 해당 아이템을 삭제
            Destroy(gameObject);
        }
    }
}