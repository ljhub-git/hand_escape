using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Jigsaw_PuzzleManager : MonoBehaviour
{
    private HashSet<TileMovement> placedTiles; // 이미 정확한 위치에 놓인 타일을 추적

    private int totalTiles; // 전체 타일 수
    private int correctTiles; // 정확한 위치에 놓인 타일 수

    // 퍼즐이 완성되었을 때 호출되는 이벤트
    public delegate void PuzzleCompleted();
    public event PuzzleCompleted OnPuzzleCompleted;

    public AudioClip succesSound; // 정답 사운드
    private AudioSource succesSource;

    public PuzzleObject puzzleObj;
    public BoardGen boardgen;

    private void Awake()
    {
        placedTiles = new HashSet<TileMovement>();
        correctTiles = 0;

        GameObject suceesSoundObject = new GameObject("SuccesSoundSource");
        suceesSoundObject.transform.parent = this.transform;
        succesSource = suceesSoundObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        succesSource.clip = succesSound;
    }

    public void Succes_Puzzle_Sound()
    {
        succesSource.Play();
    }


    public void RegisterTile(TileMovement tileMovement)
    {
        // 새로운 타일을 등록
        if (!placedTiles.Contains(tileMovement))
        {
            placedTiles.Add(tileMovement);
        }
    }

    public void CheckPuzzleCompletion()
    {
        if (correctTiles == totalTiles)
        {
            // 퍼즐 완성
            OnPuzzleCompleted?.Invoke();
            Debug.Log("퍼즐완성");
            puzzleObj.SolvePuzzle();
            OnDestroy();
        }
    }
    //치트키
    private void cheatkey()
    {
        puzzleObj.SolvePuzzle();
        OnDestroy();
    }

    private void OnDestroy()
    {
        Destroy(boardgen.gameObject);
    }
    //타일 전체 완성 체크용
    public void OnTilePlaced(TileMovement tileMovement)
    {
        if (tileMovement != null && tileMovement.IsCorrectPosition())
        {
            correctTiles++;// 올바른 위치에 있으면 타일 수 증가
            tileMovement.DisableTileCollider();
            CheckPuzzleCompletion();
        }
    }

    public void SetTotalTiles(int total)
    {
        totalTiles = total;
    }

    private void Update()
    {
        if (Input.GetKeyDown("j"))
        {
            Debug.Log("얍삽한놈");
            cheatkey();
        }
    }
}
