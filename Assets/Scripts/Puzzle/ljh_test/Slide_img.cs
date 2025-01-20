using System;
using System.Collections;
using System.Threading;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UIElements;

public class Slide_img : MonoBehaviour
{
    public int index = 0;  // 퍼즐 조각의 인덱스
    private float x = 0;  // X 위치
    private float y = 0;  // Y 위치
    [SerializeField]
    private float x_wid = 2; // 퍼즐 조각의 가로 크기
    [SerializeField]
    private float y_hei = 1.12f; // 퍼즐 조각의 세로 크기
    public float vr_scale = 0.25f; //vr에 맞춘 퍼즐조각 크기(프리팹 크기 조절이 되어야함)

    private Action<int, int> swapFunc = null; // 클릭 델리게이트
    private BoxCollider boxCollider;  // BoxCollider 컴포넌트

    private bool isMoving;

    private Collider firstCollider = null;  // 최초 충돌한 Collider

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider>();
        }

        boxCollider.size = new Vector3(x_wid / 2, y_hei / 2, 0.5f);
    }
    //퍼즐조각 초기화
    public void Init(int i, int j, int index, Sprite sprite, Action<int, int> swapFunc, GameObject puzzleParent)
    {
        this.index = index;
        this.GetComponent<SpriteRenderer>().sprite = sprite; 
        transform.SetParent(puzzleParent.transform);
        UpdatePos(i, j, true);
        this.swapFunc = swapFunc;

        // index가 16번일 경우 특별한 처리
        if (this.index == 16)
        {
            // 자식 오브젝트 cube를 찾음
            Transform cubeTransform = transform.Find("Cube");

            if (cubeTransform != null)
            {
                // 1. cube의 머티리얼을 투명한 머티리얼로 변경
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
                        Debug.LogWarning("투명 머티리얼을 Resources에서 찾을 수 없습니다.");
                    }
                }

                // 2. cube의 조명(Lighting)을 off로 설정
                if (cubeRenderer != null)
                {
                    cubeRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                }
            }
            else
            {
                Debug.LogWarning("cube 오브젝트를 찾을 수 없습니다.");
            }
        }
    }
    //퍼즐조각 위치 업데이트
    public void UpdatePos(int i, int j, bool immediate = false)
    {
        x = i * x_wid * vr_scale;
        y = j * y_hei * vr_scale;

        if (immediate)
        {
            Debug.Log($"i:{i}, j:{j}");
            Debug.Log($"x:{x}, y:{y}");
            // 즉시 위치 갱신
            this.transform.localPosition = new Vector3(x, y);
        }
        else if (!IsInvoking("Move"))
        {
            DisableTileCollider(); // 이동시작시 콜라이더 비활성화
            // 부드럽게 이동
            StartCoroutine(Move());
        }
    }


    IEnumerator Move() // 이동 애니메이션, 안써도 됨(지우고 UpdatePos 수정하기, 인보크도 지우면 됨)
    {
        //if (isMoving) yield break; // 이미 이동 중이면 중단
        //isMoving = true; // 이동 시작

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
        EnableColiider(); // 이동 완료시 콜라이더 활성화
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
        // 클릭된 콜라이더를 지닌 오브젝트의 이름을 출력합니다.
        Debug.Log("Clicked on object: " + gameObject.transform.localPosition);

        if (Input.GetMouseButtonDown(0) && swapFunc != null)
        {
            swapFunc((int)(x / (x_wid * vr_scale)), (int)(y / (y_hei * vr_scale)));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        swapFunc((int)(x / (x_wid * vr_scale)), (int)(y / (y_hei * vr_scale)));
        //Debug.Log("닿인object: " + gameObject.name);

        //Slide_puzzle moving = new Slide_puzzle();

        //isMoving = moving.GetMove();

        //if ((other.gameObject.layer == LayerMask.NameToLayer("Right Hand Physics") || other.gameObject.layer == LayerMask.NameToLayer("Left Hand Physics")) 
        //    && (swapFunc != null) && (isMoving == false) && (firstCollider == null))
        //{
        //    firstCollider = other;  // 첫 번째 충돌이 발생했을 때 저장
        //    Debug.Log($"First Trigger Entered with {other.name}");
        //    moving.SetMove(isMoving);

        //}
        //else
        //{
        //    // 첫 번째 외의 다른 Collider가 충돌해도 무시
        //    Debug.Log($"Ignoring Trigger with {other.name}");
        //}
    }

    void OnTriggerExit(Collider other)
    {
        // 충돌이 끝나면 상태 리셋
        if (other == firstCollider)
        {
            Debug.Log($"Trigger Exited with {other.name}");
            firstCollider = null;  // 충돌이 끝나면 첫 번째 Collider를 초기화
        }
    }
}
