using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using TMPro;

using Photon.Pun;

public class DuplicatedTriggerHandMannager : MonoBehaviour
{
    [SerializeField]
    private PuzzleObject leftHandPuzzleObj = null;
    [SerializeField]
    private PuzzleObject rightHandPuzzleObj = null;
    [SerializeField]
    private LightOnOffReact lightOnOff = null;

    private bool R_HandTrigger;
    private bool L_HandTrigger;

    private int rightHandLayer = 0;
    private int leftHandLayer = 0;

    private NetworkObjectManager networkObjectMng = null;

    private void Awake()
    {
        networkObjectMng = FindAnyObjectByType<NetworkObjectManager>();
    }

    private void Start()
    {
        rightHandLayer = LayerMask.NameToLayer("Right Hand Check");
        leftHandLayer = LayerMask.NameToLayer("Left Hand Check");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == rightHandLayer)
        {
            // rightHandCheck = other.GetComponent<HandCheckTrigger>();
            R_HandTrigger = true;
        }
        if (other.gameObject.layer == leftHandLayer)
        {
            // leftHandCheck = other.GetComponent<HandCheckTrigger>();
            L_HandTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other) // 손이 나가면 
    {
        if (other.gameObject.layer == rightHandLayer)
        {
            // rightHandCheck = null;
            R_HandTrigger = false;
            ResetRightHand();
        }
        if (other.gameObject.layer == leftHandLayer)
        {
            // leftHandCheck = null;
            L_HandTrigger = false;
            ResetLeftHand();
        }
    }

    public void CheckLeftHand()
    {
        if(leftHandPuzzleObj == null)
        {
            Debug.LogWarning("Left Hand Puzzle is null!");
            return;
        }

        if (lightOnOff.isLightOn && L_HandTrigger)
        {
            // TextMeshPro.text = ("GOOD!");
            leftHandPuzzleObj.SolvePuzzle();
        }
        else
        {
            leftHandPuzzleObj.ResetPuzzle();
        }
    }

    public void CheckRightHand()
    {
        if (rightHandPuzzleObj == null)
        {
            Debug.LogWarning("Right Hand Puzzle is null!");
            return;
        }

        if (lightOnOff.isLightOn && R_HandTrigger)
        {
            rightHandPuzzleObj.SolvePuzzle();
        }
        else
        {
            rightHandPuzzleObj.ResetPuzzle();
        }
    }

    public void ResetLeftHand()
    {
        if (leftHandPuzzleObj == null)
        {
            Debug.LogWarning("Left Hand Puzzle is null!");
            return;
        }

        leftHandPuzzleObj.ResetPuzzle();
    }

    public void ResetRightHand()
    {
        if (rightHandPuzzleObj == null)
        {
            Debug.LogWarning("Right Hand Puzzle is null!");
            return;
        }

        rightHandPuzzleObj.ResetPuzzle();
    }

    //public void CheckAllHands()
    //{
    //    if (lightOnOff && R_HandTrigger && L_HandTrigger && R_HandCorrect && L_HandCorrect)
    //    {
    //        // TextMeshPro.text = ("GOOD!");
    //        SolvePuzzle();
    //    }
    //    else
    //    {
    //        ResetPuzzle();
    //    }
    //}
}
