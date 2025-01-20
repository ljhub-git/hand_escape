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

    // �ڵ� ���̷��� ����̹����� ���� �� ����Ʈ ������ ������ �� �̸� ���� ����ȭ�� ����Ʈ Ʈ�������� �����Ѵ�.
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

    // �� ������Ʈ�� ���� �𵨿��Լ� �� �Է��� ��Ȱ��ȭ�Ѵ�.
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
