using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using TMPro;

using Photon.Pun;

public class DuplicatedTriggerHandMannager : PuzzleObject
{
    public TextMeshPro TextMeshPro;

    public bool R_HandTrigger;
    public bool L_HandTrigger;

    public bool R_HandCorrect { get; set; }
    public bool L_HandCorrect { get; set; }

    private int rightHandLayer = 0;
    private int leftHandLayer = 0;

    public HandCheckTrigger leftHandCheck = null;
    public HandCheckTrigger rightHandCheck = null;

    private NetworkObjectManager networkObjectMng = null;

    private LightOnOffReact lightOnOff = null;

    private void Start()
    {
        rightHandLayer = LayerMask.NameToLayer("Right Hand Check");
        leftHandLayer = LayerMask.NameToLayer("Left Hand Check");

        networkObjectMng = FindAnyObjectByType<NetworkObjectManager>();

        lightOnOff = GetComponent<LightOnOffReact>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == rightHandLayer)
        {
            rightHandCheck = other.GetComponent<HandCheckTrigger>();
            R_HandTrigger = true;
        }
        if (other.gameObject.layer == leftHandLayer)
        {
            leftHandCheck = other.GetComponent<HandCheckTrigger>();
            L_HandTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other) // 손이 나가면 
    {
        if (other.gameObject.layer == rightHandLayer)
        {
            rightHandCheck = null;
            R_HandTrigger = false;
        }
        if (other.gameObject.layer == leftHandLayer)
        {
            leftHandCheck = null;
            L_HandTrigger = false;
        }
    }

    public void CheckAllHands()
    {
        if (lightOnOff && R_HandTrigger && L_HandTrigger && R_HandCorrect && L_HandCorrect)
        {
            // TextMeshPro.text = ("GOOD!");
            SolvePuzzle();
        }
        else
        {
            ResetPuzzle();
        }
    }
}
