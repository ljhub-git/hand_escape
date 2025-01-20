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
    /// DB와 잘 연결되었는지 확인하는 함수.
    /// 잘 연결되었다면 DB Connect Success! 유니티 로그를 출력.
    /// </summary>
    private void ConnectToDBTest()
    {
        StartCoroutine(ConnectToDBTestCoroutine());
    }

    /// <summary>
    /// Login 시도를 함.
    /// </summary>
    public void LoginCheck(string _id, string _pw)
    {
        StartCoroutine(LoginCoroutine(_id, _pw));
    }

    /// <summary>
    /// 인자로 전달받은 UnityWebRequest 객체의 연결을 체크하는 함수.
    /// </summary>
    /// <param name="_www">확인할 UnityWebRequest 객체</param>
    /// <returns></returns>
    private bool CheckConnectError(UnityWebRequest _www)
    {
        return _www.result == UnityWebRequest.Result.ConnectionError ||
                _www.result == UnityWebRequest.Result.DataProcessingError;
    }

    #region SQL Coroutine
    /// <summary>
    /// DB와의 연결 여부를 로그로 찍어주는 코루틴.
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
    /// DB에게 로그인 명령을 보내는 코루틴
    /// </summary>
    /// <param name="_id">아이디</param>
    /// <param name="_pw">비밀번호</param>
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
                // 로그인 성공
                if(www.downloadHandler.text == "1")
                {
                    Debug.Log("Login Try Succed");
                    FindAnyObjectByType<TitleSceneManager>()?.OnLoginSuccess();
                }
                // 패스워드가 틀렸을 경우
                else if(www.downloadHandler.text == "0")
                {
                    Debug.Log("Password Incorrect");
                    FindAnyObjectByType<TitleSceneManager>()?.OnLoginFailed();
                }
                // 아이디가 테이블에 없을 경우
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
