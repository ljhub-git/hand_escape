using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    private bool isDebugMode = false;

    public static GameManager Instance
    {
        get { return instance; }
    }

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

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

}
