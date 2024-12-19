using UnityEngine;
using TMPro;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField idInput = null;
    [SerializeField] private TMP_InputField passwordInput = null;
    [SerializeField] private TextMeshProUGUI loginStatus = null;

    public void OnLoginButtonClicked()
    {
        Debug.Log("Button Clicked!");

        string enteredUsername = idInput.text;
        string enteredPassword = passwordInput.text;

        FindAnyObjectByType<DatabaseManager>().LoginCheck(enteredUsername, enteredPassword);
    }
}

