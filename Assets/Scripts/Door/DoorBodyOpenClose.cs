using System.Collections;
using UnityEngine;

public class DoorBodyOpenClose : MonoBehaviour
{
    private Quaternion openedDoorRot = new Quaternion(0f, 0f, 0f, 0f);
    private Quaternion closedDoorRot = new Quaternion(0f, 90f, 0f, 0f);
    private Coroutine DoorMoveCoroutine = null; // 코루틴을 추적할 변수
    public void OpenDoor()
    {
        if (DoorMoveCoroutine != null)
        {
            StopCoroutine(DoorMoveCoroutine);
        }
        DoorMoveCoroutine = StartCoroutine(DoorMove(openedDoorRot));
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
        Quaternion curRot = Quaternion.identity;
        while (true)
        {
            curRot = transform.rotation;
            Quaternion.Lerp(curRot, _goalRot, t);
            t += Time.deltaTime;
            yield return null;
        }
    }
}
