using UnityEngine;

public class FollowTargetPosRot : MonoBehaviour
{
    public Transform target = null;
    void Update()
    {
        //position 
        transform.position = target.position;
        //rotatoin 
        transform.rotation = target.rotation;
    }
}
