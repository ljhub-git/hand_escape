using System;
using System.Collections;
using UnityEngine;

public class Slide_img : MonoBehaviour
{
    public int index = 0;  // 퍼즐 조각의 인덱스
    private float x = 0;  // X 위치
    private float y = 0;  // Y 위치
    [SerializeField]
    private float x_wid = 2; // 퍼즐 조각의 가로 크기
    [SerializeField]
    private float y_hei = 1.12f; // 퍼즐 조각의 세로 크기

    private Action<int, int> swapFunc = null; // 클릭 델리게이트
    private BoxCollider2D boxCollider;  // BoxCollider2D 컴포넌트

    private bool isMoving = false;


    private void Awake()
    {
        // BoxCollider2D 컴포넌트를 가져옵니다. 없으면 추가합니다.
        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider2D>();
        }

        // 콜라이더의 크기를 x_wid와 y_hei 값으로 설정
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
            // 즉시 위치 갱신
            this.transform.localPosition = new Vector2(x, y);
            Debug.Log($"UpdatePos Immediate: Index {index}, New Pos: {transform.localPosition}");
        }
        else if (!IsInvoking("Move"))
        {
            // 코루틴으로 부드럽게 이동
            StartCoroutine(Move());
        }
    }


    IEnumerator Move() // 이동 애니메이션, 안써도 됨(지우고 UpdatePos 수정하기, 인보크도 지우면 됨)
    {
        if (isMoving) yield break; // 이미 이동 중이면 중단
        isMoving = true; // 이동 시작

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
