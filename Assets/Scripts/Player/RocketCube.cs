using ExitGames.Client.Photon.StructWrapping;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class RocketCube : MonoBehaviour
{
    public bool isFired = false; // ������ġ�� �ߴ��� Ȯ���ϴ� bool
    public bool iscatched = false; // ������ �ڽ����� ��������� Ȯ���ϴ� bool
    private BoxCollider BoxCollider = null; //�� ������Ʈ �ڽ� �ݶ��̴� ������
    private GameObject catchedObject = null; // Trigger ������Ʈ�� �ڽ����� ����� ���� ���۳�Ʈ ������
    public Vector3 catchedObjectPosition = Vector3.zero; // Trigger ������Ʈ position �� �����ϱ� ����
    private List<Rigidbody> catchedObjectRb = new List<Rigidbody>(); // Rigidbody useGravitiy�� �¿��� �ϱ����� ���۳�Ʈ ������
    private List<bool> isChangedGravity = new List<bool>(); //�� ���۳�Ʈ���� �߷��� ��Ȱ��ȭ �ߴ��� Ȯ���ϴ� bool��
    private Vector3 catcherPosition = Vector3.zero;
    private AudioSource fireSound = null;
    private ParticleSystem fireParticleSystem = null;
    private ParticleSystem catchParticleSystem = null;
    private void Awake()
    {
        BoxCollider = GetComponent<BoxCollider>();
        BoxCollider.enabled = true; // �ڽ� �ݶ��̴� Ȱ��ȭ
        BoxCollider.isTrigger = true; // �ڽ� �ݶ��̴� isTrigger Ȱ��ȭ
        fireSound = GetComponent<AudioSource>();
        fireParticleSystem = GetComponentInChildren<ParticleSystem>();
        catchParticleSystem = GetComponentsInChildren<ParticleSystem>()[1];

    }
    private void OnTriggerEnter(Collider other) // Ʈ���� �߻��ϸ�
    {
        Debug.Log("Rocket Cube OnTriggerEnter");

        if (other.gameObject.layer == LayerMask.NameToLayer("Right Hand Physics") || other.gameObject.layer == LayerMask.NameToLayer("Left Hand Physics"))
        {
            iscatched = true;
            catchedObject = null;
            catchedObjectPosition = transform.position;
            Debug.Log("������ġ�� �� �����ɴϴ�");
            return;
        }

        if (catchedObject == null && isFired) // ���� ������Ʈ�� ���� ������ġ�� ������
        {
            Transform currentParent = other.transform; // �浹 �� ������Ʈ �θ� ����
            Transform finalParent = null; //�ֻ��� �θ� ����
            while (currentParent.parent && !currentParent.parent.GetComponent<PuzzleObject>()) // �θ� �ְ� �θ� ���� ������Ʈ�� ������ ���� �ʴ� ���� ���� 
            {
                currentParent = currentParent.parent; //�θ� ������
            }
            if (currentParent.parent.GetComponent<MazeBoxManager>() != null)
            {
                currentParent = currentParent.parent; //�θ� ������                
            }
            if (currentParent.parent == null || currentParent.parent.GetComponent<PuzzleObject>()) // �θ� ���ų� �θ� ���� ������Ʈ�� ������
            {
                finalParent = currentParent; // �ֻ��� �θ� ������
                finalParent.transform.SetParent(transform);// ��ü�� �ڽ����� �ٲ�
            }
            catchParticleSystem.Play();
            iscatched = true; 
            catchedObject = finalParent.gameObject;
            catchedObjectPosition = other.transform.position;
            // GetComponentsInChildren<Rigidbody>()�� �迭�� ��������, �̸� List�� ��ȯ
            catchedObjectRb = catchedObject.GetComponentsInChildren<Rigidbody>().ToList();
            XRGrabInteractable catchedObjectGrabInteractable = null;
            catchedObjectGrabInteractable = GetComponentInChildren<XRGrabInteractable>();
            if (catchedObjectGrabInteractable != null)
            {
                // XRGrabInteractable�� ��Ȱ��ȭ�Ǿ� ���� ���, Ȱ��ȭ��ŵ�ϴ�.
                if (!catchedObjectGrabInteractable.enabled)
                {
                    catchedObjectGrabInteractable.enabled = true;
                }
            }
            isChangedGravity.Clear();  // ���� �� ����
            for (int n = 0; n < catchedObjectRb.Count; n++)
            {
                Rigidbody rb = catchedObjectRb[n];
                if (rb.useGravity)
                {
                    rb.useGravity = false; // �߷��� �������̸� �߷� ��Ȱ��ȭ
                    isChangedGravity.Add(true); // �ش� �ε����� �߷� ��Ȱ��ȭ ���
                }
                else
                {
                    isChangedGravity.Add(false); // �߷��� ������� ������ false �߰�
                }
            }

        }
        if (catchedObject) // ���� ������Ʈ�� ������
        {
            Debug.Log("�ΰ� �̻� �� ������"); 
        }
    }
    public void ParentNull() // �ڽ��� �ڽ��� �ƴ� ���·� �����
    {
        if (catchedObject != null)
        {
            catchedObject.transform.SetParent(null);
        }
    }
    public bool IsChangedGravityListClear()
    {
        if (isChangedGravity == null || isChangedGravity.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UseGravity()
    {
        if (IsChangedGravityListClear())
        { return; }
        for (int n = 0; n < catchedObjectRb.Count; n++)
        {
            Rigidbody rb = catchedObjectRb[n];

            // �߷��� ��Ȱ��ȭ�Ǿ� �ְ�, ������ �߷��� ��Ȱ��ȭ�ߴ� ���
            if (!rb.useGravity && isChangedGravity[n])
            {
                rb.useGravity = true;  // �߷� ��Ȱ��ȭ
            }
        }
        isChangedGravity.Clear(); //�߷� ��Ȱ��ȭ ������ Ȯ���ϴ� ����Ʈ Ŭ����
        isChangedGravity = new List<bool>();
    } 
    public void RemeberCatcherPosition(Vector3 _catcherPosition)
    {
        catcherPosition = _catcherPosition;
    }
    public void FireSound()
    {
        fireSound.Play();
        fireParticleSystem.Play();
    }
    public Vector3 WhoCatchMe()
    {
        return catcherPosition;
    }
}
