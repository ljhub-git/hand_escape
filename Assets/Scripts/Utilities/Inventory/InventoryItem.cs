using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    private Item_Test itemInfo;

    [SerializeField]
    private Button itemButton;

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
        // �������� ���� ������ �ٽ� ��ġ
        SpawnItemInScene();
    }

    // ���� ���� �������� ��ġ�ϴ� �޼���
    private void SpawnItemInScene()
    {
        if (itemInfo != null)
        {
            // ���ο� ������ ������Ʈ�� �����ؼ� ���� ���� ��ġ
            GameObject itemInScene = Instantiate(itemInfo.gameObject);
            itemInScene.transform.position = new Vector3(0, 0, 0); // ���ϴ� ��ġ�� ��ġ
            itemInfo.gameObject.SetActive(true);
            // �������� �ٽ� ���� ��Ÿ�����Ƿ�, UI���� �ش� �������� ����
            Destroy(gameObject);
        }
    }
}