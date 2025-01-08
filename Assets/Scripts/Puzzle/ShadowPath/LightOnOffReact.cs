using UnityEngine;

public class LightOnOffReact : PuzzleReactObject
{
    [SerializeField]
    private GameObject lightObj = null;

    private bool isLightOn = false;

    public bool IsLightOn
    {
        get { return isLightOn; }
    }

    public override void OnPuzzleSolved()
    {
        base.OnPuzzleSolved();

        isLightOn = true;

        lightObj.SetActive(true);
    }
}
