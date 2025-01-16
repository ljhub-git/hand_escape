using UnityEngine;

public class GoalEffect : MonoBehaviour
{
    public ParticleSystem particleEffect;

    private void OnTriggerEnter(Collider other)
    {
        // 리지드바디가 있는 오브젝트만 처리
        if (other.attachedRigidbody != null)
        {
            // 파티클 이펙트를 스폰 포인트에서 재생
            if (particleEffect != null)
            {
                particleEffect.transform.position = other.transform.position;
                particleEffect.Play(); // 파티클 재생
            }
        }
    }
}
