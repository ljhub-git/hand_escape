using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using TMPro;

using Photon.Pun;

public class DuplicatedTriggerHandMannager : PuzzleObject
{

    public GameObject duplicatedHandR;
    public GameObject duplicatedHandL;

    public TextMeshPro TextMeshPro;

    public bool R_HandTrigger;
    public bool L_HandTrigger;

    [SerializeField]
    private HandShadowSync handShadowSync = null;

    public bool R_HandCorrect { get; set; }
    public bool L_HandCorrect { get; set; }

    private int rightHandLayer = 0;
    private int leftHandLayer = 0;

    private HandCheckTrigger leftHandCheck = null;
    private HandCheckTrigger rightHandCheck = null;

    private NetworkObjectManager networkObjectMng = null;

    private void Start()
    {
        duplicatedHandR?.SetActive(false);
        duplicatedHandL?.SetActive(false);

        rightHandLayer = LayerMask.NameToLayer("Right Hand Check");
        leftHandLayer = LayerMask.NameToLayer("Left Hand Check");

        networkObjectMng = FindAnyObjectByType<NetworkObjectManager>();
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == rightHandLayer)
        {
            if(R_HandCorrect)
            {
                //duplicatedHandR?.SetActive(true);
                SetShowingDuplicatedHand(true, false);
            }
            else
            {
                //duplicatedHandR?.SetActive(false);
                SetShowingDuplicatedHand(false, false);
            }
        }

        if (other.gameObject.layer == leftHandLayer)
        {
            if (L_HandCorrect)
            {
                //duplicatedHandL?.SetActive(true);
                SetShowingDuplicatedHand(true, true);
            }
            else
            {
                //duplicatedHandL?.SetActive(false);
                SetShowingDuplicatedHand(false, true);
            }
        }
    }

    private void OnTriggerExit(Collider other) // 손이 나가면 
    {
        if (other.gameObject.layer == rightHandLayer)
        {
            rightHandCheck = null;
            R_HandTrigger = false;
            //duplicatedHandR?.SetActive(true);
            SetShowingDuplicatedHand(false, false);
        }
        if (other.gameObject.layer == leftHandLayer)
        {
            leftHandCheck = null;
            //L_HandTrigger = false;
            SetShowingDuplicatedHand(false, true);
        }
    }

    private void SetShowingDuplicatedHand(bool _show, bool _isLeftHand)
    {
        if (networkObjectMng == null)
        {
            Debug.LogWarning("NetworkObjectManager is null!");
            return;
        }

        if(_isLeftHand)
        {
            if (_show && duplicatedHandL.activeInHierarchy) 
                return;

            networkObjectMng.SetObjectActive(duplicatedHandL.GetComponent<PhotonView>(), _show);
        }
        else
        {
            if (_show && duplicatedHandR.activeInHierarchy) 
                return;

            networkObjectMng.SetObjectActive(duplicatedHandR.GetComponent<PhotonView>(), _show);
        }
    }

    //private void HandSync()
    //{
    //    if (handShadowSync == null)
    //        return;

    //    handShadowSync.SyncHand()
    //}

    public void CheckAllHands()
    {
        if (R_HandTrigger && L_HandTrigger && R_HandCorrect && L_HandCorrect)
        {
            TextMeshPro.text = ("GOOD!");
            SolvePuzzle();
        }
    }
}
