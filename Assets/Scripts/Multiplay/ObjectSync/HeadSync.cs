using System.Collections;
using UnityEngine;

public class HeadSync : MonoBehaviour
{
    [SerializeField]
    private Transform targetTr = null;

    public void StartFollowTransformCoroutine()
    {
        StartCoroutine(FollowTransformCoroutine());
    }

    private IEnumerator FollowTransformCoroutine()
    {
        while(true)
        {
            transform.position = targetTr.position;
            transform.rotation = targetTr.rotation;

            yield return null;
        }
    }
}
