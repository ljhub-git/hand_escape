using System.Collections;

using UnityEngine;
using UnityEngine.Networking;

public class DatabaseManager : MonoBehaviour
{

    #region URI Paths
    private const string loginUri = "http://192.168.2.109/login.php";

    private const string connectTestUri = "http://192.168.2.109/connectTest.php";

    private const string signUpUri = "http://192.168.2.109/signup.php";
    #endregion

    private void Start()
    {
        ConnectToDBTest();
    }


    /// <summary>
    /// DB�� �� ����Ǿ����� Ȯ���ϴ� �Լ�.
    /// �� ����Ǿ��ٸ� DB Connect Success! ����Ƽ �α׸� ���.
    /// </summary>
    private void ConnectToDBTest()
    {
        StartCoroutine(ConnectToDBTestCoroutine());
    }

    /// <summary>
    /// Login �õ��� ��.
    /// </summary>
    public void LoginCheck(string _id, string _pw)
    {
        StartCoroutine(LoginCoroutine(_id, _pw));
    }

    /// <summary>
    /// ���ڷ� ���޹��� UnityWebRequest ��ü�� ������ üũ�ϴ� �Լ�.
    /// </summary>
    /// <param name="_www">Ȯ���� UnityWebRequest ��ü</param>
    /// <returns></returns>
    private bool CheckConnectError(UnityWebRequest _www)
    {
        return _www.result == UnityWebRequest.Result.ConnectionError ||
                _www.result == UnityWebRequest.Result.DataProcessingError;
    }

    #region SQL Coroutine
    /// <summary>
    /// DB���� ���� ���θ� �α׷� ����ִ� �ڷ�ƾ.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ConnectToDBTestCoroutine()
    {
        WWWForm form = new WWWForm();

        using(UnityWebRequest www = UnityWebRequest.Post(connectTestUri, form))
        {
            yield return www.SendWebRequest();

            if(CheckConnectError(www))
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("DB Connect Success!");
            }
        }

    }

    /// <summary>
    /// DB���� �α��� ����� ������ �ڷ�ƾ
    /// </summary>
    /// <param name="_id">���̵�</param>
    /// <param name="_pw">��й�ȣ</param>
    /// <returns></returns>
    private IEnumerator LoginCoroutine(string _id, string _pw)
    {
        WWWForm form = new WWWForm();

        form.AddField("loginUser", _id);
        form.AddField("loginPass", _pw);

        using (UnityWebRequest www = UnityWebRequest.Post(loginUri, form))
        {
            yield return www.SendWebRequest();

            if (CheckConnectError(www))
            {
                Debug.Log(www.error);
            }
            else
            {
                // �α��� ����
                if(www.downloadHandler.text == "1")
                {
                    Debug.Log("Login Try Succed");
                    FindAnyObjectByType<TitleSceneManager>()?.OnLoginSuccess();
                }
                // �н����尡 Ʋ���� ���
                else if(www.downloadHandler.text == "0")
                {
                    Debug.Log("Password Incorrect");
                    FindAnyObjectByType<TitleSceneManager>()?.OnLoginFailed();
                }
                // ���̵� ���̺� ���� ���
                else if(www.downloadHandler.text == "2")
                {
                    Debug.Log("SignUp!");
                    StartCoroutine(SignUpCoroutine(_id, _pw));
                }
            }
        }
    }

    private IEnumerator SignUpCoroutine(string _id, string _pw)
    {
        WWWForm form = new WWWForm();

        form.AddField("signUpUser", _id);
        form.AddField("signUpPass", _pw);

        using (UnityWebRequest www = UnityWebRequest.Post(signUpUri, form))
        {
            yield return www.SendWebRequest();

            if (CheckConnectError(www))
            {
                Debug.Log(www.error);
            }
            else
            {
                FindAnyObjectByType<TitleSceneManager>()?.OnLoginSuccess();
            }
        }
    }
    #endregion
}
