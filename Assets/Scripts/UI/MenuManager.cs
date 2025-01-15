using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    static public float currentHeight = 1.36144f;

    public GameObject loginUI;
    public GameObject menuUI;   // 메인 메뉴 (Resume, Option, Quit)
    public GameObject optionUI; // 옵션 메뉴 (슬라이더, 뒤로가기 버튼)
    public GameObject checkUI;

    public PlayerManager playerMng = null;

    public bool isLogin = false;

    public Transform menuUIPosition;

    public Transform positionSource;

    [SerializeField] private float distance = 0.5f;
    [SerializeField] private float verticalOffset = 0.2f;

    [SerializeField] private SliderManager heightSliderMng = null;

    //public Slider soundSlider;    // 사운드 슬라이더
    //public AudioSource gameAudio; // 게임의 오디오 소스 (사운드 효과 조정용)

    private TitleSceneManager titleSceneManager;

    // 메인 메뉴로 이동
    public void ShowMainMenu()
    {
        Debug.Log("111");
        if (isLogin)
        {
            loginUI.SetActive(false);
            menuUI.SetActive(true);   // 메인 메뉴 활성화
            optionUI.SetActive(false); // 옵션 메뉴 비활성화
            return;
        }
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

    public void AdjustHeight(float height)
    {
        currentHeight = height;

        if (playerMng != null)
            playerMng.setPlayerHeight(height);
    }

    public void Resume()
    {
        if (isLogin)
        //if (!titleSceneManager.isLogin)
        {
            loginUI.SetActive(true);
        }
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

        if(positionSource != null)
        {
            Vector3 direction = positionSource.forward;
            direction.y = 0;
            direction.Normalize();

            Vector3 targetPosition = positionSource.position + direction * distance + Vector3.up * verticalOffset;
            RepositionMenu(targetPosition);
        }
    }

    public void RepositionMenu(Vector3 kbPos)
    {
        menuUIPosition.transform.position = kbPos;
        menuUIPosition.transform.LookAt(Camera.main.transform.position);
        menuUIPosition.transform.Rotate(Vector3.up, 180.0f);
    }

    private void Start()
    {
        AdjustHeight(currentHeight);
        if (heightSliderMng != null)
        {
            heightSliderMng.Init(currentHeight);
            heightSliderMng.OnValueChanged = AdjustHeight;
        }
    }

    public void LoadTitleScene()
    {
        SceneManager.LoadScene("S_Title");
    }
}
