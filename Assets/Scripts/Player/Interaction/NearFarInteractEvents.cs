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

    // 물체와의 상호작용이 시작될 때 호출되는 이벤트.
    public void OnInteractSelectEntered(SelectEnterEventArgs selectEnterEvent)
    {
        Debug.LogFormat("From {0} : {1} Select Entered!", selectEnterEvent.interactorObject.transform.name, selectEnterEvent.interactableObject.transform.name);
        // 상호작용된 오브젝트에게서 포톤 뷰를 가져오고, 네트워크 오브젝트 매니저에게 중력 여부를 설정하게 한다.
        {
            PhotonView view = selectEnterEvent.interactableObject.transform.GetComponent<PhotonView>();
            
            if (view != null)
                networkObjMng.SetNetworkObjectGravityUsable(view, false);
        }
    }

    // 물체와의 상호작용이 끝날 때 호출되는 이벤트.
    public void OnInteractSelectExited(SelectExitEventArgs selectExitEvent)
    {
        Debug.LogFormat("From {0} : {1} Select Exit!", selectExitEvent.interactorObject.transform.name, selectExitEvent.interactableObject.transform.name);
        // 상호작용이 끝난 오브젝트에게서 포톤 뷰를 가져오고, 네트워크 오브젝트 매니저에게 중력 여부를 설정하게 한다.
        {
            PhotonView view = selectExitEvent.interactableObject.transform.GetComponent<PhotonView>();
            
            if(view != null)
                networkObjMng.SetNetworkObjectGravityUsable(view, true);
        }
    }
}