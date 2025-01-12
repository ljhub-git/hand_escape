using UnityEngine;


public class MenuManager : MonoBehaviour
{
    private MenuUI menuUIParent = null;
    public GameObject loginUI;
    public GameObject menuUI;   // 메인 메뉴 (Resume, Option, Quit)
    public GameObject optionUI; // 옵션 메뉴 (슬라이더, 뒤로가기 버튼)
    public GameObject checkUI;

    //public Slider soundSlider;    // 사운드 슬라이더
    //public AudioSource gameAudio; // 게임의 오디오 소스 (사운드 효과 조정용)

    TitleSceneManager titleSceneManager;

    private bool isLogin = false;
    //LoginManager loginManager;

    [SerializeField] private float distance = 0.5f;
    [SerializeField] private float verticalOffset = 0.2f;

    public Transform menuUIPosition;

    public Transform positionSource;

    private void Awake()
    {
        //menuUIParent = FindAnyObjectByType<MenuUI>();

        ////if(menuUIParent != null)
        ////    menuUIPosition = FindAnyObjectByType<MenuUI>().transform;

        //menuUI = menuUIParent._menuUI;
        //optionUI = menuUIParent._optionUI;
        //checkUI = menuUIParent._checkUI;

        //positionSource = transform;
    }


    // 메인 메뉴로 이동
    public void ShowMainMenu()
    {
        Debug.Log("111");
        //if (!isLogin)
        ////if (!titleSceneManager.isLogin)
        //{
        //    loginUI.SetActive(false);
        //    menuUI.SetActive(true);   // 메인 메뉴 활성화
        //    optionUI.SetActive(false); // 옵션 메뉴 비활성화
        //    return;
        //}
        OpenMenuIngame();
    }

    // 옵션 메뉴로 이동
    public void ShowOptionMenu()
    {
        menuUI.SetActive(false);  // 메인 메뉴 비활성화
        optionUI.SetActive(true); // 옵션 메뉴 활성화
    }

    // 옵션 슬라이더에서 사운드 볼륨 조정
    public void AdjustSound(float volume)
    {
        //gameAudio.volume = volume; // 오디오 소스의 볼륨을 슬라이더 값으로 설정
    }

    public void Resume()
    {
        //if (!isLogin)
        ////if (!titleSceneManager.isLogin)
        //{
        //    loginUI.SetActive(true);
        //}
        menuUI.SetActive(false);  // 메인 메뉴 비활성화
    }

    public void Check()
    {
        menuUI.SetActive(false);
        checkUI.SetActive(true);
    }
    // 게임 종료
    public void QuitGame()
    {
        Application.Quit(); // 게임 종료
    }

    // 뒤로가기 버튼을 누르면 메인 메뉴로 돌아가기
    public void BackToMainMenu()
    {
        //ShowMainMenu();
        menuUI.SetActive(true);   // 메인 메뉴 활성화
        optionUI.SetActive(false); // 옵션 메뉴 비활성화
        checkUI.SetActive(false);
    }

    public void OpenMenuIngame()
    {
        menuUI.SetActive(true);
        optionUI.SetActive(false);

        Vector3 direction = positionSource.forward;
        direction.y = 0;
        direction.Normalize();

        Vector3 targetPosition = positionSource.position + direction * distance + Vector3.up * verticalOffset;
        RepositionMenu(targetPosition);
    }

    public void RepositionMenu(Vector3 kbPos)
    {
        menuUIPosition.transform.position = kbPos;
        menuUIPosition.transform.LookAt(Camera.main.transform.position);
        menuUIPosition.transform.Rotate(Vector3.up, 180.0f);

        //transform.position = kbPos;
        //transform.LookAt(Camera.main.transform.position);
        //transform.Rotate(Vector3.up, 180.0f);
    }
}
