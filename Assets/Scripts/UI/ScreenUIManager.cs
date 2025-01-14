using UnityEngine;

public class ScreenUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playerLeaveConfirm = null;

    private NetworkManager networkMng = null;

    public void ShowPlayerLeaveConfirm()
    {
        playerLeaveConfirm.SetActive(true);
        Time.timeScale = 0f;
    }

    public void OnClickedLeaveRoom()
    {
        if (networkMng != null)
            networkMng.LeaveRoom();
    }

    private void Awake()
    {
        networkMng = FindAnyObjectByType<NetworkManager>();
    }

}
