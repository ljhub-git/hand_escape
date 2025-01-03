using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject loginUI;
    public GameObject menuUI;   // 메인 메뉴 (Resume, Option, Quit)
    public GameObject optionUI; // 옵션 메뉴 (슬라이더, 뒤로가기 버튼)

    //public Slider soundSlider;    // 사운드 슬라이더
    //public AudioSource gameAudio; // 게임의 오디오 소스 (사운드 효과 조정용)

    TitleSceneManager titleSceneManager;

    private bool isLogin = false;
    //LoginManager loginManager;

    // Start는 초기화 작업을 하는 함수
    void Start()
    {
        // 게임 오디오의 초기 볼륨을 슬라이더 값에 맞게 설정
        //soundSlider.value = gameAudio.volume;
    }

    // 메인 메뉴로 이동
    public void ShowMainMenu()
    {
        if (!isLogin)
        //if (!titleSceneManager.isLogin)
        {
            loginUI.SetActive(false);
        }
        menuUI.SetActive(true);   // 메인 메뉴 활성화
        optionUI.SetActive(false); // 옵션 메뉴 비활성화
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
        if (!isLogin)
        //if (!titleSceneManager.isLogin)
        {
            loginUI.SetActive(true);
        }
        menuUI.SetActive(false);  // 메인 메뉴 비활성화
    }

    // 게임 종료
    public void QuitGame()
    {
        Application.Quit(); // 게임 종료
    }

    // 뒤로가기 버튼을 누르면 메인 메뉴로 돌아가기
    public void BackToMainMenu()
    {
        ShowMainMenu();
    }
}
