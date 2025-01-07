using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Interaction.Toolkit.Samples.Hands;

public class HandShadowSync : MonoBehaviour
{
    // 핸드 스켈레톤 드라이버에서 손의 각 조인트 정보를 가져온 후 이를 토대로 동기화할 조인트 트랜스폼을 설정한다.
    public void InitHandJointTrs()
    {
        var XRHandSkeletonDrivers = GetComponentsInChildren<XRHandSkeletonDriver>();

        var leftJoints = XRHandSkeletonDrivers[0].jointTransformReferences;
        var rightJoints = XRHandSkeletonDrivers[1].jointTransformReferences;
    }

    public void SyncHand(List<JointToTransformReference> _sourceJoints, bool isLeftHand)
    {
        var XRHandSkeletonDrivers = GetComponentsInChildren<XRHandSkeletonDriver>();

        if (XRHandSkeletonDrivers == null)
            return;

        int idx = 0;

        if(isLeftHand)
        {
            idx = 0;
        }
        else
        {
            idx = 1;
        }

        var targetJoints = XRHandSkeletonDrivers[idx].jointTransformReferences;

        for(int i = 0; i < targetJoints.Count; ++i)
        {
            targetJoints[i].jointTransform.rotation = _sourceJoints[i].jointTransform.rotation;
        }
    }
}
