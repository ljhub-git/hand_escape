using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class TutorialGhost : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> handPose = new List<GameObject>();
    private GameObject curActivatedObject = null;
    private int currentPoseIndex = 0;
    private TextMeshPro explainText = null;
    private float next = 0;
    private void Awake()
    {
        explainText = GetComponentInChildren<TextMeshPro>();
        explainText.SetText("");
    }
    private void Update()
    {
        next += Time.deltaTime;

        // 일정 시간이 지난 후 포즈를 변경하고, 활성화된 오브젝트를 curActivatedObject에 할당
        if (next > 3)
        {
            // 이전에 활성화된 오브젝트를 비활성화
            if (curActivatedObject != null)
            {
                curActivatedObject.SetActive(false);
            }

            // handPose 리스트에서 현재 인덱스의 오브젝트를 활성화
            if (handPose.Count > 0)
            {
                curActivatedObject = handPose[currentPoseIndex];
                curActivatedObject.SetActive(true);  // 해당 오브젝트 활성화
                explainText.SetText(curActivatedObject.name); // 텍스트로 오브젝트 이름 표시
            }

            // 인덱스를 증가시켜서 다음 오브젝트를 활성화, 인덱스가 끝나면 다시 0으로 설정
            currentPoseIndex = (currentPoseIndex + 1) % handPose.Count;

            next = 0; // 타이머 초기화
        }
    }
}
