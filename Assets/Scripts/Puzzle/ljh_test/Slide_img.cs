using System;
using System.Collections;
using UnityEngine;

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
    private BoxCollider2D boxCollider;  // BoxCollider2D ������Ʈ

    private bool isMoving = false;


    private void Awake()
    {
        // BoxCollider2D ������Ʈ�� �����ɴϴ�. ������ �߰��մϴ�.
        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider2D>();
        }

        // �ݶ��̴��� ũ�⸦ x_wid�� y_hei ������ ����
        boxCollider.size = new Vector2(x_wid, y_hei);
    }

    public void Init(int i, int j, int index, Sprite sprite, Action<int, int> swapFunc)
    {
        this.index = index;
        this.GetComponent<SpriteRenderer>().sprite = sprite;
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

    public void UpdatePos(int i, int j, bool immediate = false)
    {
        x = i * x_wid * vr_scale;
        y = j * y_hei * vr_scale;

        if (immediate)
        {
            // ��� ��ġ ����
            this.transform.localPosition = new Vector2(x, y);
        }
        else if (!IsInvoking("Move"))
        {
            // �ڷ�ƾ���� �ε巴�� �̵�
            StartCoroutine(Move());
        }
    }


    IEnumerator Move() // �̵� �ִϸ��̼�, �Ƚᵵ ��(����� UpdatePos �����ϱ�, �κ�ũ�� ����� ��)
    {
        if (isMoving) yield break; // �̹� �̵� ���̸� �ߴ�
        isMoving = true; // �̵� ����

        float elapsedTime = 0;
        float duration = 0.2f;
        Vector2 startpos = this.gameObject.transform.localPosition;
        Vector2 endpos = new Vector2(x, y);


        while (elapsedTime < duration)
        {
            this.gameObject.transform.localPosition = Vector2.Lerp(startpos, endpos, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        this.gameObject.transform.localPosition = endpos;
        isMoving = false;
    }

    public bool IsEmpty()
    {
        return index == 16;
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && swapFunc != null)
        {
            swapFunc((int)(x / (x_wid * vr_scale)), (int)(y / (y_hei * vr_scale)));
        }
    }
}
