using System.Collections.Generic;
using UnityEngine;

public class Jigsaw_PuzzleManager : MonoBehaviour
{
    private HashSet<TileMovement> placedTiles; // �ùٸ� ��ġ�� ��ġ�� Ÿ�ϵ�
    private int totalTiles; // ��ü Ÿ�� ����
    private int correctTiles; // �ùٸ� ��ġ�� ��ġ�� Ÿ�� ����

    // ������ �ϼ��Ǿ��� �� ȣ��Ǵ� �̺�Ʈ
    public delegate void PuzzleCompleted();
    public event PuzzleCompleted OnPuzzleCompleted;

    private void Awake()
    {
        placedTiles = new HashSet<TileMovement>();
        correctTiles = 0;
    }

    public void RegisterTile(TileMovement tileMovement)
    {
        if (tileMovement == null)
        {
            Debug.LogError("TileMovement�� null�Դϴ�. Ÿ���� ����� �� �����ϴ�.");
            return;
        }

        if (!placedTiles.Contains(tileMovement))
        {
            placedTiles.Add(tileMovement);
        }
    }

    public void OnTilePlaced(TileMovement tileMovement)
    {
        if (tileMovement != null && tileMovement.IsCorrectPosition())
        {
            if (!placedTiles.Contains(tileMovement))
            {
                placedTiles.Add(tileMovement);
                correctTiles++;
                tileMovement.DisableTileCollider(); // Ÿ�� ����
                CheckPuzzleCompletion();
            }
        }
    }

    public void CheckPuzzleCompletion()
    {
        if (correctTiles >= totalTiles)
        {
            Debug.Log("������ �ϼ��Ǿ����ϴ�!");
            OnPuzzleCompleted?.Invoke();
        }
    }

    public void SetTotalTiles(int total)
    {
        totalTiles = total;
    }
}
