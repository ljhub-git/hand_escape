using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject loginUI;
    public GameObject menuUI;   // ���� �޴� (Resume, Option, Quit)
    public GameObject optionUI; // �ɼ� �޴� (�����̴�, �ڷΰ��� ��ư)

    //public Slider soundSlider;    // ���� �����̴�
    //public AudioSource gameAudio; // ������ ����� �ҽ� (���� ȿ�� ������)

    TitleSceneManager titleSceneManager;

    private bool isLogin = false;
    //LoginManager loginManager;

    // Start�� �ʱ�ȭ �۾��� �ϴ� �Լ�
    void Start()
    {
        // ���� ������� �ʱ� ������ �����̴� ���� �°� ����
        //soundSlider.value = gameAudio.volume;
    }

    // ���� �޴��� �̵�
    public void ShowMainMenu()
    {
        if (!isLogin)
        //if (!titleSceneManager.isLogin)
        {
            loginUI.SetActive(false);
        }
        menuUI.SetActive(true);   // ���� �޴� Ȱ��ȭ
        optionUI.SetActive(false); // �ɼ� �޴� ��Ȱ��ȭ
    }

    // �ɼ� �޴��� �̵�
    public void ShowOptionMenu()
    {
        menuUI.SetActive(false);  // ���� �޴� ��Ȱ��ȭ
        optionUI.SetActive(true); // �ɼ� �޴� Ȱ��ȭ
    }

    // �ɼ� �����̴����� ���� ���� ����
    public void AdjustSound(float volume)
    {
        //gameAudio.volume = volume; // ����� �ҽ��� ������ �����̴� ������ ����
    }

    public void Resume()
    {
        if (!isLogin)
        //if (!titleSceneManager.isLogin)
        {
            loginUI.SetActive(true);
        }
        menuUI.SetActive(false);  // ���� �޴� ��Ȱ��ȭ
    }

    // ���� ����
    public void QuitGame()
    {
        Application.Quit(); // ���� ����
    }

    // �ڷΰ��� ��ư�� ������ ���� �޴��� ���ư���
    public void BackToMainMenu()
    {
        ShowMainMenu();
    }
}
