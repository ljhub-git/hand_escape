using UnityEngine;

public class LightOnOffReact : PuzzleReactObject
{
    [SerializeField]
    private GameObject lightObj = null;

    public bool isLightOn = false;

    private void Start()
    {
        lightObj.SetActive(isLightOn);
    }

    public override void OnPuzzleSolved()
    {
        base.OnPuzzleSolved();

        isLightOn = true;

        lightObj.SetActive(true);
    }

    public override void OnPuzzleReset()
    {
        base.OnPuzzleReset();

        isLightOn = false;

        lightObj.SetActive(false);

    }
}
