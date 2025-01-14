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
    /// Ready 텍스트를 토글한다.
    /// </summary>
    public void SetPlayerReady(bool _isReady)
    {
        text_Ready.gameObject.SetActive(_isReady);
    }
}
