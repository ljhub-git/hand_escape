using UnityEngine;

public class LightOnOffReact : PuzzleReactObject
{
    [SerializeField]
    private GameObject lightObj = null;

    private AudioSource switchAudio = null;

    public bool isLightOn = false;

    private void Awake()
    {
        switchAudio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        lightObj.SetActive(isLightOn);
    }

    public override void OnPuzzleSolved()
    {
        base.OnPuzzleSolved();

        isLightOn = true;

        if(lightObj != null)
            lightObj.SetActive(true);

        if (switchAudio != null)
            switchAudio.PlayOneShot(switchAudio.clip);
    }

    public override void OnPuzzleReset()
    {
        base.OnPuzzleReset();

        isLightOn = false;

        if (lightObj != null)
            lightObj.SetActive(false);

        if (switchAudio != null)
            switchAudio.PlayOneShot(switchAudio.clip);
    }
}
