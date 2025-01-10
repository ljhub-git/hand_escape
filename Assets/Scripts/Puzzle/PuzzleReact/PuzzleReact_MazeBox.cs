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
        // �� �ı��ϰ� ���ο� �� ����. (��Ʈ��ũ ������Ʈ �Ŵ����� ���ؼ�)
        Vector3 spawnPos = ball.transform.position;

        networkObjectMng.DestroyObject(ball.GetComponent<PhotonView>());

        yield return new WaitForSeconds(0.5f);

        networkObjectMng.InstantiateObject("P_Ball", spawnPos, Quaternion.identity);

    }
}
