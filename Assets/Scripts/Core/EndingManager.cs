using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;

public class EndingManager : MonoBehaviour
{
    public void Start()
    {
        PhotonNetwork.Disconnect();
    }

    public void LoadTitleScene()
    {
        SceneManager.LoadScene("S_Title");
    }
}
