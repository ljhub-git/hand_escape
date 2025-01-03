using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DoorBodyOpenClose : MonoBehaviour
{
    private Quaternion openedDoorRot = Quaternion.Euler(0f, 5f, 0f);
    private Quaternion closedDoorRot = Quaternion.Euler(0f, 90f, 0f);
    private Coroutine DoorMoveCoroutine = null; // 코루틴을 추적할 변수
    private bool isLocked = false;

    public void OpenDoor()
    {
        if (DoorMoveCoroutine != null) 
        {
            StopCoroutine(DoorMoveCoroutine);
        }
        if (!isLocked)
        {
            DoorMoveCoroutine = StartCoroutine(DoorMove(openedDoorRot));
        }

        //transform.rotation = openedDoor;
    }
    public void CloseDoor()
    {
        if (DoorMoveCoroutine != null)
        {
            StopCoroutine(DoorMoveCoroutine);
        }
        DoorMoveCoroutine = StartCoroutine(DoorMove(closedDoorRot));
        //transform.rotation = closedDoor;

    }

    private IEnumerator DoorMove(Quaternion _goalRot)
    {
        float t = 0;
        Quaternion curRot = transform.rotation;
        while (t < 1)
        {
            curRot = transform.rotation;
            t += Time.deltaTime;
            t = Mathf.Min(t, 1);  // t 값이 1을 넘지 않도록 제한
            if (t > 0)
            {
                transform.rotation = Quaternion.Lerp(curRot, _goalRot, t);
            }
            yield return null;
        }
        transform.rotation = _goalRot;
    }
}
