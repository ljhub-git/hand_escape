using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public void Init(Item_Test _itemInfo)
    {
        Image img = GetComponent<Image>();
        img.color = _itemInfo.itemColor;
    }
}