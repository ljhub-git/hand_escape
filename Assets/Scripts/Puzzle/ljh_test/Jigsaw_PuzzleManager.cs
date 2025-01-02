using System.Collections.Generic;
using UnityEngine;

public class Jigsaw_PuzzleManager : MonoBehaviour
{
    private HashSet<TileMovement> placedTiles; // 올바른 위치에 배치된 타일들
    private int totalTiles; // 전체 타일 개수
    private int correctTiles; // 올바른 위치에 배치된 타일 개수

    // 퍼즐이 완성되었을 때 호출되는 이벤트
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
            Debug.LogError("TileMovement가 null입니다. 타일을 등록할 수 없습니다.");
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
                tileMovement.DisableTileCollider(); // 타일 고정
                CheckPuzzleCompletion();
            }
        }
    }

    public void CheckPuzzleCompletion()
    {
        if (correctTiles >= totalTiles)
        {
            Debug.Log("퍼즐이 완성되었습니다!");
            OnPuzzleCompleted?.Invoke();
        }
    }

    public void SetTotalTiles(int total)
    {
        totalTiles = total;
    }
}
