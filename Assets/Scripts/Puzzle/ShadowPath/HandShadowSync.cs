using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Interaction.Toolkit.Samples.Hands;

public class HandShadowSync : MonoBehaviour
{
    // �ڵ� ���̷��� ����̹����� ���� �� ����Ʈ ������ ������ �� �̸� ���� ����ȭ�� ����Ʈ Ʈ�������� �����Ѵ�.
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
