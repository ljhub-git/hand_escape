using UnityEngine;

using Photon.Pun;

public class NetworkObserverPlayer : MonoBehaviourPun
{
    [SerializeField]
    private GameObject cameraGo = null;
    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private float mouseSensitivity = 10f;
    private void Start()
    {
        if(!photonView.IsMine)
            cameraGo.SetActive(false);
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 dir = (transform.right * xAxis + transform.forward * yAxis).normalized;

        transform.position += dir * speed * Time.deltaTime;

        transform.Rotate(0f, Input.GetAxis("Mouse X") * mouseSensitivity, 0f, Space.World);
        transform.Rotate(Mathf.Clamp(-Input.GetAxis("Mouse Y") * mouseSensitivity,-90f,90f), 0f, 0f);
    }
}
