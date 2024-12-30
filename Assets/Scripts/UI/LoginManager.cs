using UnityEngine;
using TMPro;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField idInput = null;
    [SerializeField] private TMP_InputField passwordInput = null;
    [SerializeField] private TextMeshProUGUI loginStatus = null;

    //public bool isLogin = false;
    private TitleSceneManager titleSceneMng = null;

    public string CurrentID
    {
        get { return idInput.text; }
    }

    public void OnLoginButtonClicked()
    {
        Debug.Log("Button Clicked!");

        string enteredUsername = idInput.text;
        string enteredPassword = passwordInput.text;

        titleSceneMng.TryLogin(enteredUsername, enteredPassword);
    }

    private void Awake()
    {
        titleSceneMng = FindAnyObjectByType<TitleSceneManager>();
    }
}

