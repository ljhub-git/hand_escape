using UnityEngine;
using TMPro;
using WebSocketSharp;

public class LoginManager : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField idInput = null;
    [SerializeField]
    private TMP_InputField passwordInput = null;
    [SerializeField]
    private TextMeshProUGUI loginStatus = null;

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

        if(enteredUsername.IsNullOrEmpty() || enteredPassword.IsNullOrEmpty())
        {
            loginStatus.color = Color.red;
            loginStatus.text = "Fill all inputs!";

            return;
        }

        if(titleSceneMng != null)
            titleSceneMng.TryLogin(enteredUsername, enteredPassword);
    }

    public void OnLoginFailed()
    {
        loginStatus.color = Color.red;
        loginStatus.text = "Password does not match!";
    }

    private void Awake()
    {
        titleSceneMng = FindAnyObjectByType<TitleSceneManager>();
    }
}

