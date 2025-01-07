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

        networkObjectMng.InstantiateObject("P_Ball", spawnPos, Quaternion.identity);

        networkObjectMng.DestroyObject(ball.GetComponent<PhotonView>());
    }

    private void Start()
    {
        networkObjectMng = FindAnyObjectByType<NetworkObjectManager>();
        ball = GetComponentInChildren<MazeBoxBall>();
    }
}
