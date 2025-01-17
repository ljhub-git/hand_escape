using UnityEngine;

public class PuzzleReact_PlayParticleOnce : PuzzleReactObject
{
    [SerializeField]
    private ParticleSystem particle = null;

    public override void OnPuzzleSolved()
    {
        base.OnPuzzleSolved();

        if (particle != null)
            particle.Play();
    }
}
