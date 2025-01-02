using UnityEngine;

public class FollowTargetRot : MonoBehaviour
{
    public Transform target = null;
    void Update()
    {
        //rotatoin 
        transform.rotation = target.rotation;
    }
}
