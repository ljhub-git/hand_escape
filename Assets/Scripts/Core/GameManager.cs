using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool isDebugMode = false;

    [SerializeField]
    private string nextLevelName = string.Empty;

    private NetworkManager networkMng = null;

    public bool IsDebugMode
    {
        get { return isDebugMode; }
    }

    public void StartDebugMode()
    {
        isDebugMode = true;
    }

    public void EndDebugMode()
    {
        isDebugMode = false;
    }

    public void LoadNextLevel()
    {
        networkMng.LoadScene(nextLevelName);
    }

    private void Awake()
    {
        networkMng = FindAnyObjectByType<NetworkManager>();
    }
}
