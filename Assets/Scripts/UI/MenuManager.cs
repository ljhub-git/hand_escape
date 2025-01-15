using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    static public float currentHeight = 1.36144f;

    public GameObject loginUI;
    public GameObject menuUI;   // ���� �޴� (Resume, Option, Quit)
    public GameObject optionUI; // �ɼ� �޴� (�����̴�, �ڷΰ��� ��ư)
    public GameObject checkUI;

    public PlayerManager playerMng = null;

    public bool isLogin = false;

    public Transform menuUIPosition;

    public Transform positionSource;

    [SerializeField] private float distance = 0.5f;
    [SerializeField] private float verticalOffset = 0.2f;

    [SerializeField] private SliderManager heightSliderMng = null;

    //public Slider soundSlider;    // ���� �����̴�
    //public AudioSource gameAudio; // ������ ����� �ҽ� (���� ȿ�� ������)

    private TitleSceneManager titleSceneManager;

    // ���� �޴��� �̵�
    public void ShowMainMenu()
    {
        Debug.Log("111");
        if (isLogin)
        {
            loginUI.SetActive(false);
            menuUI.SetActive(true);   // ���� �޴� Ȱ��ȭ
            optionUI.SetActive(false); // �ɼ� �޴� ��Ȱ��ȭ
            return;
        }
        OpenMenuIngame();
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
        menuUI.SetActive(false);  // ���� �޴� ��Ȱ��ȭ
    }

    public void Check()
    {
        menuUI.SetActive(false);
        checkUI.SetActive(true);
    }
    // ���� ����
    public void QuitGame()
    {
        Application.Quit(); // ���� ����
    }

    // �ڷΰ��� ��ư�� ������ ���� �޴��� ���ư���
    public void BackToMainMenu()
    {
        //ShowMainMenu();
        menuUI.SetActive(true);   // ���� �޴� Ȱ��ȭ
        optionUI.SetActive(false); // �ɼ� �޴� ��Ȱ��ȭ
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
