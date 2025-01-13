using UnityEngine;

public class PuzzleReact_SetActiveAndAudioPlay : PuzzleReactObject
{
    private AudioSource audiosource = null;
    private void Awake()
    {
        audiosource = GetComponent<AudioSource>();
    }
    public override void OnPuzzleSolved()
    {
        base.OnPuzzleSolved();
        Debug.Log("½ÇÇàµÊ");
        gameObject.SetActive(true);
        audiosource.Play();
        audiosource = null;
    }

    public override void OnPuzzleReset()
    {
        base.OnPuzzleReset();

        gameObject.SetActive(false);
    }
}
