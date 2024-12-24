using System.Collections.Generic;
using UnityEngine;

public class Jigsaw_PuzzleManager : MonoBehaviour
{
    private HashSet<TileMovement> placedTiles; // �̹� ��Ȯ�� ��ġ�� ���� Ÿ���� ����

    private int totalTiles; // ��ü Ÿ�� ��
    private int correctTiles; // ��Ȯ�� ��ġ�� ���� Ÿ�� ��

    // ������ �ϼ��Ǿ��� �� ȣ��Ǵ� �̺�Ʈ (�ɼ�)
    public delegate void PuzzleCompleted();
    public event PuzzleCompleted OnPuzzleCompleted;

    private void Awake()
    {
        placedTiles = new HashSet<TileMovement>();
        correctTiles = 0;
    }

    public void RegisterTile(TileMovement tileMovement)
    {
        // ���ο� Ÿ���� ���
        if (!placedTiles.Contains(tileMovement))
        {
            placedTiles.Add(tileMovement);
        }
    }

    public void CheckPuzzleCompletion()
    {
        if (correctTiles == totalTiles)
        {
            // ���� �ϼ�
            OnPuzzleCompleted?.Invoke();
            Debug.Log("����ϼ�");
        }
    }

    public void OnTilePlaced(TileMovement tileMovement)
    {
        if (tileMovement != null && tileMovement.IsCorrectPosition())
        {
            correctTiles++;
            // �ùٸ� ��ġ�� ������ Ÿ�� �� ����
            //if (!placedTiles.Contains(tileMovement))
            //{
            //    //placedTiles.Add(tileMovement);

            //}
            tileMovement.DisableTileCollider();
            CheckPuzzleCompletion();
        }
    }

    public void SetTotalTiles(int total)
    {
        totalTiles = total;
    }
}
