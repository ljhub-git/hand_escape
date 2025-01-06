using UnityEngine;
using TMPro;

public class PlayerReadyUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text_NickName = null;

    [SerializeField]
    private TextMeshProUGUI text_Ready = null;

    public void SetNickName(string _nickName)
    {
        text_NickName.text = _nickName;
    }

    /// <summary>
    /// Ready �ؽ�Ʈ�� ����Ѵ�.
    /// </summary>
    public void ToggleReady()
    {
        text_Ready.gameObject.SetActive(!text_Ready.gameObject.activeInHierarchy);
    }
}
