using UnityEngine;

public class Player3DTest : MonoBehaviour
{
    private CharacterController controller = null;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        controller.Move(new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime, 0f, 0f));
    }
}
