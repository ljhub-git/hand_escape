using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using TMPro;

using Photon.Pun;

public class DuplicatedTriggerHandMannager : MonoBehaviour
{
    public TextMeshPro TextMeshPro;

    [SerializeField]
    private HandCorrectCheck leftHandPuzzleObj = null;
    [SerializeField]
    private HandCorrectCheck rightHandPuzzleObj = null;

    private bool R_HandTrigger;
    private bool L_HandTrigger;

    private int rightHandLayer = 0;
    private int leftHandLayer = 0;

    //private HandCheckTrigger leftHandCheck = null;
    //private HandCheckTrigger rightHandCheck = null;

    private NetworkObjectManager networkObjectMng = null;

    private LightOnOffReact lightOnOff = null;

    //public bool R_HandCorrect { get; set; }
    //public bool L_HandCorrect { get; set; }

    private void Awake()
    {
        networkObjectMng = FindAnyObjectByType<NetworkObjectManager>();

        lightOnOff = GetComponent<LightOnOffReact>();
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
            rightHandPuzzleObj.ResetPuzzle();
        }
        if (other.gameObject.layer == leftHandLayer)
        {
            // leftHandCheck = null;
            L_HandTrigger = false;
            leftHandPuzzleObj.ResetPuzzle();
        }
    }

    public void CheckLeftHand()
    {
        if(leftHandPuzzleObj == null)
        {
            Debug.LogWarning("Left Hand Puzzle is null!");
            return;
        }

        if (lightOnOff && L_HandTrigger)
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

        if (lightOnOff && R_HandTrigger)
        {
            // TextMeshPro.text = ("GOOD!");
            rightHandPuzzleObj.SolvePuzzle();
        }
        else
        {
            rightHandPuzzleObj.ResetPuzzle();
        }
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
