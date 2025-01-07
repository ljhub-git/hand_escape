using System.Collections;

using UnityEngine;

using Photon.Pun;

public class MazeBoxManager : PuzzleObject
{
    private NetworkObjectManager networkObjectMng = null;

    private MazeBoxBall ball = null;

    public override void SolvePuzzle()
    {
        StartCoroutine(SpawnBallCoroutine());
        base.SolvePuzzle();
    }

    private void Start()
    {
        networkObjectMng = FindAnyObjectByType<NetworkObjectManager>();
        ball = GetComponentInChildren<MazeBoxBall>();
    }

    private IEnumerator SpawnBallCoroutine()
    {
        // �� �ı��ϰ� ���ο� �� ����. (��Ʈ��ũ ������Ʈ �Ŵ����� ���ؼ�)
        Vector3 spawnPos = ball.transform.position;

        networkObjectMng.DestroyObject(ball.GetComponent<PhotonView>());

        yield return new WaitForSeconds(0.5f);

        networkObjectMng.InstantiateObject("P_Ball", spawnPos, Quaternion.identity);

    }
}
