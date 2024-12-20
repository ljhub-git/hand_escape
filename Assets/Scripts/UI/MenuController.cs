using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject mainMenu;   // ���� �޴� (Resume, Option, Quit)
    public GameObject optionMenu; // �ɼ� �޴� (�����̴�, �ڷΰ��� ��ư)
    public Slider soundSlider;    // ���� �����̴�
    public AudioSource gameAudio; // ������ ����� �ҽ� (���� ȿ�� ������)

    // Start�� �ʱ�ȭ �۾��� �ϴ� �Լ�
    void Start()
    {
        // ���� ������� �ʱ� ������ �����̴� ���� �°� ����
        soundSlider.value = gameAudio.volume;
    }

    // ���� �޴��� �̵�
    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);   // ���� �޴� Ȱ��ȭ
        optionMenu.SetActive(false); // �ɼ� �޴� ��Ȱ��ȭ
    }

    // �ɼ� �޴��� �̵�
    public void ShowOptionMenu()
    {
        mainMenu.SetActive(false);  // ���� �޴� ��Ȱ��ȭ
        optionMenu.SetActive(true); // �ɼ� �޴� Ȱ��ȭ
    }

    // �ɼ� �����̴����� ���� ���� ����
    public void AdjustSound(float volume)
    {
        gameAudio.volume = volume; // ����� �ҽ��� ������ �����̴� ������ ����
    }

    public void ResumeGame()
    {
        mainMenu.SetActive(false);  // ���� �޴� ��Ȱ��ȭ
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
