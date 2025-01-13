using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Jigsaw_PuzzleManager : MonoBehaviour
{
    private HashSet<TileMovement> placedTiles; // �̹� ��Ȯ�� ��ġ�� ���� Ÿ���� ����

    private int totalTiles; // ��ü Ÿ�� ��
    private int correctTiles; // ��Ȯ�� ��ġ�� ���� Ÿ�� ��

    // ������ �ϼ��Ǿ��� �� ȣ��Ǵ� �̺�Ʈ
    public delegate void PuzzleCompleted();
    public event PuzzleCompleted OnPuzzleCompleted;

    public AudioClip succesSound; // ���� ����
    private AudioSource succesSource;

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
            correctTiles++;// �ùٸ� ��ġ�� ������ Ÿ�� �� ����
            tileMovement.DisableTileCollider();
            CheckPuzzleCompletion();
        }
    }

    public void SetTotalTiles(int total)
    {
        totalTiles = total;
    }
}
