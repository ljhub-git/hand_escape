using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DuplicatedTriggerHandMannager : PuzzleObject
{
    public GameObject duplicatedHandR;
    public GameObject duplicatedHandL;

    public TextMeshPro TextMeshPro;

    public bool R_HandTrigger;
    public bool L_HandTrigger;

    public bool R_HandCorrect { get; set; }
    public bool L_HandCorrect { get; set; }

    private void Awake()
    {
        duplicatedHandR?.SetActive(false);
        duplicatedHandL?.SetActive(false);
    }

    public void OnTriggerEnter(Collider other) // 손이 들어오면
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Right Hand Physics"))
        {
            if (duplicatedHandR) 
                duplicatedHandR.SetActive(true); 

            R_HandTrigger = true;

        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Left Hand Physics"))
        {
            if (duplicatedHandL) 
                duplicatedHandL.SetActive(true);

            L_HandTrigger = true;
        }

    }
    public void OnTriggerExit(Collider other) // 손이 나가면 
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Right Hand Physics"))
        {
            if (duplicatedHandR) 
                duplicatedHandR.SetActive(false);

            R_HandTrigger = false;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Left Hand Physics"))
        {
            if (duplicatedHandL) 
                duplicatedHandL.SetActive(false);

            L_HandTrigger = false;
        }
    }
    public void CheckAllHands()
    {
        if (R_HandTrigger && L_HandTrigger && R_HandCorrect && L_HandCorrect)
        {
            TextMeshPro.text = ("GOOD!");
            SolvePuzzle();
        }
    }
}
