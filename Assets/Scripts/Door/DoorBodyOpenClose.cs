using System.Collections;
using UnityEngine;

public class DoorBodyOpenClose : PuzzleReactObject
{
    private Quaternion openedDoorRot = Quaternion.Euler(0f, 5f, 0f);
    private Quaternion closedDoorRot = Quaternion.Euler(0f, 90f, 0f);
    private Coroutine DoorMoveCoroutine = null; // 코루틴을 추적할 변수
    private bool isLocked = false;
    private AudioSource doorOpenSound = null;

    private void Awake()
    {
       doorOpenSound = GetComponent<AudioSource>();
        if (doorOpenSound == null)
        {
            Debug.LogError("doorOpenSound is not valid");
        }
    }
    public override void OnPuzzleSolved()
    {
        base.OnPuzzleSolved();
        OpenDoor();
    }

    public void OpenDoor()
    {
        doorOpenSound.Play();
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

        if (closedDoorRot == transform.rotation) // 닫힌문이면 사운드 출력
        {
            doorOpenSound.Play();
        }
    }
}
