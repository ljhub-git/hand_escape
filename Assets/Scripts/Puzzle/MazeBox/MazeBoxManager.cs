using UnityEngine;

using Photon.Pun;

public class MazeBoxManager : PuzzleObject
{
    private NetworkObjectManager networkObjectMng = null;

    private MazeBoxBall ball = null;

    public override void SolvePuzzle()
    {
        base.SolvePuzzle();

        // �� �ı��ϰ� ���ο� �� ����. (��Ʈ��ũ ������Ʈ �Ŵ����� ���ؼ�)
        Vector3 spawnPos = ball.transform.position;

        networkObjectMng.DestroyObject(ball.GetComponent<PhotonView>());

        networkObjectMng.InstantiateObject("P_Ball", spawnPos, Quaternion.identity);
    }

    private void Start()
    {
        networkObjectMng = FindAnyObjectByType<NetworkObjectManager>();
        ball = GetComponentInChildren<MazeBoxBall>();
    }
}
