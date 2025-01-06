using System;
using UnityEngine;

public class Slide_puzzle : MonoBehaviour
{
    public Slide_img img_Prefab;  // 퍼즐 조각 프리팹
    public Slide_img[,] boxes = new Slide_img[4, 4]; // 4x4 그리드
    public Sprite[] sprites;  // 15개의 퍼즐 조각에 들어갈 스프라이트들

    private Vector2Int dragStartPos;

    private void Start()
    {
        Init();
        Shuffle();
    }

    private void Init()
    {
        int n = 0;
        for (int y = 3; y >= 0; y--)
            for (int x = 0; x < 4; x++)
            {
                Slide_img box = Instantiate(img_Prefab, new Vector2(x, y), Quaternion.identity);
                box.Init(x, y, n + 1, sprites[n], HandleClick);
                boxes[x, y] = box;
                n++;
            }
    }

    private void Shuffle()
    {
        int emptyX = -1, emptyY = -1;

        // 빈칸 위치 찾기
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                if (boxes[x, y].IsEmpty())
                {
                    emptyX = x;
                    emptyY = y;
                    break;
                }
            }
        }

        int shuffleSteps = 100; // 섞기 반복 횟수
        for (int i = 0; i < shuffleSteps; i++)
        {
            Vector2 move = getValidMove(emptyX, emptyY);

            // 빈칸과 교환
            int newX = emptyX + (int)move.x;
            int newY = emptyY + (int)move.y;

            Swap(emptyX, emptyY, (int)move.x, (int)move.y);

            // 빈칸 위치 갱신
            emptyX = newX;
            emptyY = newY;
        }

        Debug.Log("Shuffle completed!");

        // 섞은 후 모든 퍼즐 조각 위치 출력
        LogPuzzlePositions();
    }



    private void LogPuzzlePositions()
    {
        Debug.Log("섞인 후 퍼즐 위치:");
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                var tile = boxes[x, y];
                Debug.Log($"Index: {tile.index}, Array Pos: ({x}, {y}), LocalPos: {tile.transform.localPosition}");
            }
        }
    }



    void Swap(int x, int y, int dx, int dy)
    {
        if (dx == 0 && dy == 0)
            return;  // 교환할 필요 없음

        var from = boxes[x, y];
        var target = boxes[x + dx, y + dy];

        boxes[x, y] = target;
        boxes[x + dx, y + dy] = from;

        from.UpdatePos(x + dx, y + dy, true);
        target.UpdatePos(x, y, true);
    }

    private void HandleClick(int clickedX, int clickedY)
    {
        Debug.Log($"HandleClick called: Array({clickedX}, {clickedY}), Index: {boxes[clickedX, clickedY].index}");

        int emptyX = -1, emptyY = -1;
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                if (boxes[x, y].IsEmpty())
                {
                    emptyX = x;
                    emptyY = y;
                    break;
                }
            }
        }


        int rowDirection = clickedY - emptyY;
        int columnDirection = clickedX - emptyX;


        if (rowDirection != 0 && columnDirection != 0)
        {
            Debug.Log("Invalid move: not in the same row or column.");
            return;
        }

        if (rowDirection == 0)
        {
            MoveTilesHorizontally(clickedX, emptyX, clickedY);
        }
        else if (columnDirection == 0)
        {
            MoveTilesVertically(clickedY, emptyY, clickedX);
        }

        CheckIfSolved();// 정답확인
    }
    private void MoveTilesHorizontally(int clickedX, int emptyX, int y)
    {
        int direction = (clickedX > emptyX) ? 1 : -1;

        Slide_img emptyTile = boxes[emptyX, y];

        for (int x = emptyX; x != clickedX; x += direction)
        {
            Slide_img currentTile = boxes[x + direction, y];
            boxes[x, y] = currentTile;
            currentTile.UpdatePos(x, y);
        }

        boxes[clickedX, y] = emptyTile;
        emptyTile.UpdatePos(clickedX, y);
    }

    private void MoveTilesVertically(int clickedY, int emptyY, int x)
    {
        int direction = (clickedY > emptyY) ? 1 : -1;
        Slide_img emptyTile = boxes[x, emptyY];
        for (int y = emptyY; y != clickedY; y += direction)
        {
            Slide_img currentTile = boxes[x, y + direction];
            boxes[x, y] = currentTile;
            currentTile.UpdatePos(x, y); // 애니메이션 포함 이동
        }

        // 빈칸 이동

        boxes[x, clickedY] = emptyTile;
        emptyTile.UpdatePos(x, clickedY);

    }



    //int getDx(int x, int y)
    //{

    //    if (x < 3 && boxes[x + 1, y].IsEmpty())
    //    {
    //        return 1;
    //    }
    //    if (x > 0 && boxes[x - 1, y].IsEmpty())
    //    {
    //        return -1;
    //    }
    //    return 0;
    //}

    //int getDy(int x, int y)
    //{
    //    if (y < 3 && boxes[x, y + 1].IsEmpty())
    //    {
    //        return 1;
    //    }
    //    if (y > 0 && boxes[x, y - 1].IsEmpty())
    //    {
    //        return -1;
    //    }
    //    return 0;
    //}
    //int getColumnDirection(int x, int y)
    //{
    //    Debug.Log($"Start checking column for ({x}, {y})");
    //    for (int j = 0; j < 4; j++)
    //    {
    //        Debug.Log($"Checking column: Array({x}, {j}), Index: {boxes[x, j].index}, IsEmpty: {boxes[x, j].IsEmpty()}");
    //        if (boxes[x, j].IsEmpty())
    //        {
    //            Debug.Log($"Empty tile found in column: Array({x}, {j})");
    //            return j > y ? 1 : -1; // 위쪽(+1) 또는 아래쪽(-1)
    //        }
    //    }
    //    return 0;
    //}


    //int getRowDirection(int x, int y)
    //{
    //    Debug.Log($"Start checking row for ({x}, {y})");
    //    for (int i = 0; i < 4; i++)
    //    {
    //        Debug.Log($"Checking row: Array({i}, {y}), Index: {boxes[i, y].index}, IsEmpty: {boxes[i, y].IsEmpty()}");
    //        if (boxes[i, y].IsEmpty())
    //        {
    //            Debug.Log($"Empty tile found in row: Array({i}, {y})");
    //            return i > x ? 1 : -1; // 오른쪽(+1) 또는 왼쪽(-1)
    //        }
    //    }
    //    return 0;
    //}




    //void Shuffle()
    //{
    //    for (int i = 0; i < 4; ++i)
    //    {
    //        for (int j = 0; j < 4; ++j)
    //        {
    //            if (boxes[i, j].IsEmpty())
    //            {
    //                Vector2 pos = getValidMove(i, j);
    //                Swap(i, j, (int)pos.x, (int)pos.y);
    //            }
    //        }
    //    }
    //}

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
        int count = 0;
        bool isSolved = true;
        Debug.Log("클릭시작" + isSolved);

        for (int y = 3; y >= 0; y--)
        {
            for (int x = 0; x < 4; x++)
            {
                // 각 퍼즐 조각이 (x, y) 위치에 올바르게 있는지 확인
                int correctIndex = (3 - y) * 4 + x + 1; // 올바른 index 계산
                if (boxes[x, y].index != correctIndex)
                {
                    isSolved = false;
                    break; // 퍼즐이 틀린 경우 비교 중단
                }
                else
                {
                    count++;
                }
            }
            if (!isSolved) break;
            if(count == 16)
            {
                Debug.Log("정답입니다");
            }
            else
            {
                Debug.Log("count: " + count);
            }
        }
        
    }

}
