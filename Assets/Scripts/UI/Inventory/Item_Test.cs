using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Item_Test : MonoBehaviour
{
    public XRGrabInteractable XRGrabInteractable;
    public bool isBeingDragged = false;

    Vector3 previousPosition;
    public Color itemColor;

    private void Awake()
    {
        XRGrabInteractable = GetComponent<XRGrabInteractable>();
    }
    private void Start()
    {
        MeshRenderer mr =
            GetComponentInChildren<MeshRenderer>();
        itemColor = mr.material.color;
    }

    void OnMouseDrag()
    {
        isBeingDragged = true;
        float distance = Camera.main.WorldToScreenPoint(transform.position).z;

        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        Vector3 objPos = Camera.main.ScreenToWorldPoint(mousePos);

        // 이동할 축을 선택하는 조건
        if (Input.GetKey(KeyCode.A))
        {
            previousPosition = transform.position;
            objPos.y = previousPosition.y;
        }
        if (Input.GetKey(KeyCode.S))
        {
            previousPosition = transform.position;
            objPos.z = previousPosition.z;
        }
        transform.position = objPos;
    }

    private void OnMouseUp()
    {
        isBeingDragged = false;
        //Debug.Log(isBeingDragged);
    }
}
