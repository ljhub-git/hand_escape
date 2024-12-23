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
        // ��ư�� Ŭ�� �̺�Ʈ ����
        itemButton.onClick.AddListener(OnItemButtonClick);
    }

    public void Init(Item_Test _itemInfo)
    {
        Image img = GetComponent<Image>();
        img.color = _itemInfo.itemColor;
        itemInfo = _itemInfo;
    }


    // ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    private void OnItemButtonClick()
    {
        // ��ư Ŭ�� �� �������� ���� ��ġ�ϴ� ��û
        InventoryManager.instance?.SpawnItemInScene(this);
    }
}