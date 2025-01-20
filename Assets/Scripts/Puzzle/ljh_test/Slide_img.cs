using System;
using System.Collections;
using System.Threading;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UIElements;

public class Slide_img : MonoBehaviour
{
    public int index = 0;  // ���� ������ �ε���
    private float x = 0;  // X ��ġ
    private float y = 0;  // Y ��ġ
    [SerializeField]
    private float x_wid = 2; // ���� ������ ���� ũ��
    [SerializeField]
    private float y_hei = 1.12f; // ���� ������ ���� ũ��
    public float vr_scale = 0.25f; //vr�� ���� �������� ũ��(������ ũ�� ������ �Ǿ����)

    private Action<int, int> swapFunc = null; // Ŭ�� ��������Ʈ
    private BoxCollider boxCollider;  // BoxCollider ������Ʈ

    private bool isMoving;

    private Collider firstCollider = null;  // ���� �浹�� Collider

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider>();
        }

        boxCollider.size = new Vector3(x_wid / 2, y_hei / 2, 0.5f);
    }
    //�������� �ʱ�ȭ
    public void Init(int i, int j, int index, Sprite sprite, Action<int, int> swapFunc, GameObject puzzleParent)
    {
        this.index = index;
        this.GetComponent<SpriteRenderer>().sprite = sprite; 
        transform.SetParent(puzzleParent.transform);
        UpdatePos(i, j, true);
        this.swapFunc = swapFunc;

        // index�� 16���� ��� Ư���� ó��
        if (this.index == 16)
        {
            // �ڽ� ������Ʈ cube�� ã��
            Transform cubeTransform = transform.Find("Cube");

            if (cubeTransform != null)
            {
                // 1. cube�� ��Ƽ������ ������ ��Ƽ����� ����
                MeshRenderer cubeRenderer = cubeTransform.GetComponent<MeshRenderer>();
                if (cubeRenderer != null)
                {
                    Material transparentMaterial = Resources.Load<Material>("shader/transparencys");
                    if (transparentMaterial != null)
                    {
                        cubeRenderer.material = transparentMaterial;
                    }
                    else
                    {
                        Debug.LogWarning("���� ��Ƽ������ Resources���� ã�� �� �����ϴ�.");
                    }
                }

                // 2. cube�� ����(Lighting)�� off�� ����
                if (cubeRenderer != null)
                {
                    cubeRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                }
            }
            else
            {
                Debug.LogWarning("cube ������Ʈ�� ã�� �� �����ϴ�.");
            }
        }
    }
    //�������� ��ġ ������Ʈ
    public void UpdatePos(int i, int j, bool immediate = false)
    {
        x = i * x_wid * vr_scale;
        y = j * y_hei * vr_scale;

        if (immediate)
        {
            Debug.Log($"i:{i}, j:{j}");
            Debug.Log($"x:{x}, y:{y}");
            // ��� ��ġ ����
            this.transform.localPosition = new Vector3(x, y);
        }
        else if (!IsInvoking("Move"))
        {
            DisableTileCollider(); // �̵����۽� �ݶ��̴� ��Ȱ��ȭ
            // �ε巴�� �̵�
            StartCoroutine(Move());
        }
    }


    IEnumerator Move() // �̵� �ִϸ��̼�, �Ƚᵵ ��(����� UpdatePos �����ϱ�, �κ�ũ�� ����� ��)
    {
        //if (isMoving) yield break; // �̹� �̵� ���̸� �ߴ�
        //isMoving = true; // �̵� ����

        float elapsedTime = 0;
        float duration = 0.2f;
        Vector2 startpos = this.gameObject.transform.localPosition;
        Vector2 endpos = new Vector2(x, y);
        Slide_puzzle moving = new Slide_puzzle();


        while (elapsedTime < duration)
        {
            this.gameObject.transform.localPosition = Vector2.Lerp(startpos, endpos, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        this.gameObject.transform.localPosition = endpos;
        moving.SetMove(isMoving);
        EnableColiider(); // �̵� �Ϸ�� �ݶ��̴� Ȱ��ȭ
    }

    public void EnableColiider()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        if (collider != null)
        {
            collider.enabled = true;
        }
    }
    public void DisableTileCollider()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        if (collider != null)
        {
            collider.enabled = false;
        }
    }

    public bool IsEmpty()
    {
        return index == 16;
    }

    private void OnMouseDown()
    {
        // Ŭ���� �ݶ��̴��� ���� ������Ʈ�� �̸��� ����մϴ�.
        Debug.Log("Clicked on object: " + gameObject.transform.localPosition);

        if (Input.GetMouseButtonDown(0) && swapFunc != null)
        {
            swapFunc((int)(x / (x_wid * vr_scale)), (int)(y / (y_hei * vr_scale)));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        swapFunc((int)(x / (x_wid * vr_scale)), (int)(y / (y_hei * vr_scale)));
        //Debug.Log("����object: " + gameObject.name);

        //Slide_puzzle moving = new Slide_puzzle();

        //isMoving = moving.GetMove();

        //if ((other.gameObject.layer == LayerMask.NameToLayer("Right Hand Physics") || other.gameObject.layer == LayerMask.NameToLayer("Left Hand Physics")) 
        //    && (swapFunc != null) && (isMoving == false) && (firstCollider == null))
        //{
        //    firstCollider = other;  // ù ��° �浹�� �߻����� �� ����
        //    Debug.Log($"First Trigger Entered with {other.name}");
        //    moving.SetMove(isMoving);

        //}
        //else
        //{
        //    // ù ��° ���� �ٸ� Collider�� �浹�ص� ����
        //    Debug.Log($"Ignoring Trigger with {other.name}");
        //}
    }

    void OnTriggerExit(Collider other)
    {
        // �浹�� ������ ���� ����
        if (other == firstCollider)
        {
            Debug.Log($"Trigger Exited with {other.name}");
            firstCollider = null;  // �浹�� ������ ù ��° Collider�� �ʱ�ȭ
        }
    }
}
