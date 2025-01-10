using System.Collections;

using UnityEngine;
using Photon.Pun;

public class PuzzleReact_MazeBox : PuzzleReactObject
{
    private NetworkObjectManager networkObjectMng = null;

    private MazeBoxBall ball = null;

    private void Awake()
    {
        networkObjectMng = FindAnyObjectByType<NetworkObjectManager>();
        ball = GetComponentInChildren<MazeBoxBall>();
    }

    public override void OnPuzzleSolved()
    {
        base.OnPuzzleSolved();

        StartCoroutine(SpawnBallCoroutine());
    }

    private IEnumerator SpawnBallCoroutine()
    {
        // 공 파괴하고 새로운 공 생성. (네트워크 오브젝트 매니저를 통해서)
        Vector3 spawnPos = ball.transform.position;

        networkObjectMng.DestroyObject(ball.GetComponent<PhotonView>());

        yield return new WaitForSeconds(0.5f);

        networkObjectMng.InstantiateObject("P_Ball", spawnPos, Quaternion.identity);

    }
}
