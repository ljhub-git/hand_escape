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
    }

    public void UpdatePos(int i, int j, bool immediate = false)
    {
        x = i * x_wid;
        y = j * y_hei;

        if (immediate)
        {
            // ��� ��ġ ����
            this.transform.localPosition = new Vector2(x, y);
            Debug.Log($"UpdatePos Immediate: Index {index}, New Pos: {transform.localPosition}");
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
            swapFunc((int)(x / x_wid), (int)(y / y_hei));
        }
    }
}
