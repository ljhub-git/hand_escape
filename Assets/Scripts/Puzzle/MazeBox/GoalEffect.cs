using UnityEngine;

public class GoalEffect : MonoBehaviour
{
    public ParticleSystem particleEffect;

    private void OnTriggerEnter(Collider other)
    {
        // ������ٵ� �ִ� ������Ʈ�� ó��
        if (other.attachedRigidbody != null)
        {
            // ��ƼŬ ����Ʈ�� ���� ����Ʈ���� ���
            if (particleEffect != null)
            {
                particleEffect.transform.position = other.transform.position;
                particleEffect.Play(); // ��ƼŬ ���
            }
        }
    }
}
