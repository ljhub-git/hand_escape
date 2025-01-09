using UnityEngine;


public class MenuManager : MonoBehaviour
{
    public GameObject loginUI;
    public GameObject menuUI;   // ���� �޴� (Resume, Option, Quit)
    public GameObject optionUI; // �ɼ� �޴� (�����̴�, �ڷΰ��� ��ư)
    public GameObject checkUI;

    //public Slider soundSlider;    // ���� �����̴�
    //public AudioSource gameAudio; // ������ ����� �ҽ� (���� ȿ�� ������)

    TitleSceneManager titleSceneManager;

    private bool isLogin = false;
    //LoginManager loginManager;

    [SerializeField] private float distance = 0.5f;
    [SerializeField] private float verticalOffset = 0.2f;

    public Transform menuPosition;

    private Transform positionSource;

    // Start�� �ʱ�ȭ �۾��� �ϴ� �Լ�
    void Start()
    {
        if (loginUI == null)
        {
            //�׽�Ʈ�� �Ҵ�
            isLogin = true;
        }

        NetworkManager networkManager = FindAnyObjectByType<NetworkManager>();

        if (networkManager != null)
        {
            networkManager.OnPlayerSpawned += SetPositionSource;
        }

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
            menuUI.SetActive(true);   // ���� �޴� Ȱ��ȭ
            optionUI.SetActive(false); // �ɼ� �޴� ��Ȱ��ȭ
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

    public void Resume()
    {
        if (!isLogin)
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
        menuPosition.transform.position = kbPos;
        menuPosition.transform.LookAt(Camera.main.transform.position);
        menuPosition.transform.Rotate(Vector3.up, 180.0f);

    }

    public void SetPositionSource()
    {
        positionSource = Camera.main.transform;
    }
}
