using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;  // 카메라 이동 속도
    public float rotateSpeed = 100f; // 카메라 회전 속도

    private void Update()
    {
        // WASD로 이동
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveY = 0;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        // R/F로 상하 이동
        if (Input.GetKey(KeyCode.R))
        {
            moveY = moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.F))
        {
            moveY = -moveSpeed * Time.deltaTime;
        }

        // Q/E로 좌우 회전
        float rotateX = 0;
        if (Input.GetKey(KeyCode.Q))
        {
            rotateX = -rotateSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            rotateX = rotateSpeed * Time.deltaTime;
        }

        // T/G로 상하 회전
        float rotateY = 0;
        if (Input.GetKey(KeyCode.T))
        {
            rotateY = -rotateSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.G))
        {
            rotateY = rotateSpeed * Time.deltaTime;
        }

        // 카메라 위치 이동
        transform.Translate(moveX, moveY, moveZ);

        // 카메라 회전
        transform.Rotate(Vector3.up, rotateX);  // 좌우 회전
        transform.Rotate(Vector3.right, rotateY);  // 상하 회전
    }
}
