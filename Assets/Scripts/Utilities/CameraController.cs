using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;  // ī�޶� �̵� �ӵ�
    public float rotateSpeed = 100f; // ī�޶� ȸ�� �ӵ�

    private void Update()
    {
        // WASD�� �̵�
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveY = 0;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        // R/F�� ���� �̵�
        if (Input.GetKey(KeyCode.R))
        {
            moveY = moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.F))
        {
            moveY = -moveSpeed * Time.deltaTime;
        }

        // Q/E�� �¿� ȸ��
        float rotateX = 0;
        if (Input.GetKey(KeyCode.Q))
        {
            rotateX = -rotateSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            rotateX = rotateSpeed * Time.deltaTime;
        }

        // T/G�� ���� ȸ��
        float rotateY = 0;
        if (Input.GetKey(KeyCode.T))
        {
            rotateY = -rotateSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.G))
        {
            rotateY = rotateSpeed * Time.deltaTime;
        }

        // ī�޶� ��ġ �̵�
        transform.Translate(moveX, moveY, moveZ);

        // ī�޶� ȸ��
        transform.Rotate(Vector3.up, rotateX);  // �¿� ȸ��
        transform.Rotate(Vector3.right, rotateY);  // ���� ȸ��
    }
}
