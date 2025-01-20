using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Hands;

public class MultiHandManager : MonoBehaviour
{
    [SerializeField]
    private TransformSync transformSync = null;

    private List<Transform> syncLeftHandJointTrs = new List<Transform>();
    private List<Transform> syncRightHandJointTrs = new List<Transform>();

    // 핸드 스켈레톤 드라이버에서 손의 각 조인트 정보를 가져온 후 이를 토대로 동기화할 조인트 트랜스폼을 설정한다.
    public void InitHandJointTrs()
    {
        var XRHandSkeletonDrivers = GetComponentsInChildren<XRHandSkeletonDriver>();

        var leftJoints = XRHandSkeletonDrivers[0].jointTransformReferences;
        var rightJoints = XRHandSkeletonDrivers[1].jointTransformReferences;

        foreach (var joint in leftJoints)
        {
            syncLeftHandJointTrs.Add(joint.jointTransform);
        }
        transformSync.AddTransforms(syncLeftHandJointTrs);

        foreach (var joint in rightJoints)
        {
            syncRightHandJointTrs.Add(joint.jointTransform);
        }
        transformSync.AddTransforms(syncRightHandJointTrs);
    }

    // 이 컴포넌트를 가진 모델에게서 손 입력을 비활성화한다.
    public void DisableHandInput()
    {
        var XRHandMeshConts = GetComponentsInChildren<XRHandMeshController>();
        foreach (var component in XRHandMeshConts)
        {
            component.enabled = false;
        }

        var XRHandSkeletonDrivers = GetComponentsInChildren<XRHandSkeletonDriver>();
        foreach (var component in XRHandSkeletonDrivers)
        {
            component.enabled = false;
        }

        var XRHandTrackingEventsComps = GetComponentsInChildren<XRHandTrackingEvents>();
        foreach(var component in XRHandTrackingEventsComps)
        {
            component.enabled = false;
        }
    }

    public void HiddenHandMesh()
    {
        transform.GetChild(0).GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        transform.GetChild(1).GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
    }
}
