using System;
using UnityEngine;

public class Slide_puzzle : MonoBehaviour
{
    public Slide_img img_Prefab;  // 퍼즐 조각 프리팹
    public Slide_img[,] boxes = new Slide_img[4, 4]; // 4x4 그리드
    public Sprite[] sprites;  // 15개의 퍼즐 조각에 들어갈 스프라이트들

    private void Start()
    {
        Init();
        for (int i = 0; i < 99; i++)
            Shuffle();
    }

    private void Init()
    {
        int n = 0;
        for(int y = 3; y >= 0; y--)
            for(int x = 0; x< 4; x++)
            {
                Slide_img box = Instantiate(img_Prefab, new Vector2(x,y), Quaternion.identity);
                box.Init(x, y, n + 1, sprites[n], ClickToSwap);
                boxes[x, y] = box;
                n++;
            }
    }

    void ClickToSwap(int x, int y)
    {
        int dx = getDx(x, y);
        int dy = getDy(x, y);
        Swap(x,y,dx,dy);
        CheckIfSolved();// 정답확인
    }

    void Swap(int x, int y, int dx, int dy)
    {
        if (dx == 0 && dy == 0)
            return;  // 교환할 필요 없음

        var from = boxes[x, y];
        var target = boxes[x+dx, y+dy];

        boxes[x, y] = target;
        boxes[x + dx, y + dy] = from;

        from.UpdatePos(x + dx, y + dy);
        target.UpdatePos(x, y);
    }

    int getDx(int x, int y)
    {
        if(x < 3 && boxes[x+1, y].IsEmpty())
        {
            return 1;
        }
        if (x > 0 && boxes[x - 1, y].IsEmpty())
        {
            return -1;
        }
        return 0;
    }

    int getDy(int x, int y)
    {
        if (y < 3 && boxes[x, y+1].IsEmpty())
        {
            return 1;
        }
        if (y > 0 && boxes[x, y-1].IsEmpty())
        {
            return -1;
        }
        return 0;
    }

    void Shuffle()
    {
        for(int i=0; i<4; ++i)
        {
            for (int j = 0; j < 4; ++j)
            {
                if(boxes[i, j].IsEmpty())
                {
                    Vector2 pos = getValidMove(i, j);
                    Swap(i,j, (int)pos.x, (int)pos.y);
                }
            }
        }
    }

    private Vector2 lastMove;

    Vector2 getValidMove(int x, int y)
    {
        Vector2 pos = new Vector2();
        do
        {
            int n = UnityEngine.Random.Range(0, 4);
            if (n == 0)
                pos = Vector2.left;
            else if (n == 1)
                pos = Vector2.right;
            else if (n == 2)
                pos = Vector2.up;
            else
                pos = Vector2.down;


        } while (!(isValidRange(x + (int)pos.x) && isValidRange(y + (int)pos.y)) || isRepeatMove(pos));

        lastMove = pos;
        return pos;
    }
    bool isValidRange(int n)
    {
        return n >= 0 && n <= 3;
    }

    bool isRepeatMove(Vector2 pos)
    {
        return pos * -1 == lastMove;
    }

    //퍼즐이 정답인지 확인
    void CheckIfSolved()
    {
        bool isSolved = true;
        Debug.Log("클릭시작"+isSolved);

        for (int y = 3; y >= 0 ; y--)
        {
            for(int x = 0; x < 4; x++)
            {
                // 각 퍼즐 조각이 (x, y) 위치에 올바르게 있는지 확인
                int correctIndex = (3 - y) * 4 + x + 1; // 올바른 index 계산
                if (boxes[x, y].index != correctIndex)
                {
                    isSolved = false;
                    Debug.Log("비교완료 " + isSolved);
                    Debug.Log("x: " + x + "y:" + y);
                    Debug.Log("현재 index: " + boxes[x, y].index);  // 퍼즐 조각의 실제 index
                    Debug.Log("올바른 index: " + correctIndex);  // 올바른 index 계산값
                    break; // 퍼즐이 틀린 경우 비교 중단
                }
            }
            if (!isSolved) break;
        }
    }
}
