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

        // ���� �ð��� ���� �� ��� �����ϰ�, Ȱ��ȭ�� ������Ʈ�� curActivatedObject�� �Ҵ�
        if (next > 3)
        {
            // ������ Ȱ��ȭ�� ������Ʈ�� ��Ȱ��ȭ
            if (curActivatedObject != null)
            {
                curActivatedObject.SetActive(false);
            }

            // handPose ����Ʈ���� ���� �ε����� ������Ʈ�� Ȱ��ȭ
            if (handPose.Count > 0)
            {
                curActivatedObject = handPose[currentPoseIndex];
                curActivatedObject.SetActive(true);  // �ش� ������Ʈ Ȱ��ȭ
                explainText.SetText(curActivatedObject.name); // �ؽ�Ʈ�� ������Ʈ �̸� ǥ��
            }

            // �ε����� �������Ѽ� ���� ������Ʈ�� Ȱ��ȭ, �ε����� ������ �ٽ� 0���� ����
            currentPoseIndex = (currentPoseIndex + 1) % handPose.Count;

            next = 0; // Ÿ�̸� �ʱ�ȭ
        }
    }
}
