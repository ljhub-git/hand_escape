using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

using Photon.Pun;

public class NearFarInteractEvents : MonoBehaviour
{
    private NetworkObjectManager networkObjMng = null;

    private void Start()
    {
        networkObjMng = FindAnyObjectByType<NetworkObjectManager>();
    }

    // ��ü���� ��ȣ�ۿ��� ���۵� �� ȣ��Ǵ� �̺�Ʈ.
    public void OnInteractSelectEntered(SelectEnterEventArgs selectEnterEvent)
    {
        Debug.LogFormat("From {0} : {1} Select Entered!", selectEnterEvent.interactorObject.transform.name, selectEnterEvent.interactableObject.transform.name);
        // ��ȣ�ۿ�� ������Ʈ���Լ� ���� �並 ��������, ��Ʈ��ũ ������Ʈ �Ŵ������� �߷� ���θ� �����ϰ� �Ѵ�.
        {
            PhotonView view = selectEnterEvent.interactableObject.transform.GetComponent<PhotonView>();
            
            if (view != null)
                networkObjMng.SetNetworkObjectGravityUsable(view, false);
        }
    }

    // ��ü���� ��ȣ�ۿ��� ���� �� ȣ��Ǵ� �̺�Ʈ.
    public void OnInteractSelectExited(SelectExitEventArgs selectExitEvent)
    {
        Debug.LogFormat("From {0} : {1} Select Exit!", selectExitEvent.interactorObject.transform.name, selectExitEvent.interactableObject.transform.name);
        // ��ȣ�ۿ��� ���� ������Ʈ���Լ� ���� �並 ��������, ��Ʈ��ũ ������Ʈ �Ŵ������� �߷� ���θ� �����ϰ� �Ѵ�.
        {
            PhotonView view = selectExitEvent.interactableObject.transform.GetComponent<PhotonView>();
            
            if(view != null)
                networkObjMng.SetNetworkObjectGravityUsable(view, true);
        }
    }
}