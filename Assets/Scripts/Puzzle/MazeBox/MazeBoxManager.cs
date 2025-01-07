using UnityEngine;

using Photon.Pun;

public class MazeBoxManager : PuzzleObject
{
    private NetworkObjectManager networkObjectMng = null;

    private MazeBoxBall ball = null;

    public override void SolvePuzzle()
    {
        base.SolvePuzzle();

        // 공 파괴하고 새로운 공 생성. (네트워크 오브젝트 매니저를 통해서)
        Vector3 spawnPos = ball.transform.position;

        networkObjectMng.InstantiateObject("P_Ball", spawnPos, Quaternion.identity);

        networkObjectMng.DestroyObject(ball.GetComponent<PhotonView>());
    }

    private void Start()
    {
        networkObjectMng = FindAnyObjectByType<NetworkObjectManager>();
        ball = GetComponentInChildren<MazeBoxBall>();
    }
}
