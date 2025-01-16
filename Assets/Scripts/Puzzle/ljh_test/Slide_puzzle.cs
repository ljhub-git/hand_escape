using System;
using UnityEngine;

public class Slide_puzzle : MonoBehaviour
{
    public Slide_img img_Prefab;  
    public Slide_img[,] boxes = new Slide_img[4, 4]; 
    public Sprite[] sprites; 

    private Vector2Int dragStartPos;

    private GameObject puzzleParent;

    public PuzzleObject puzzleObj;

    private bool isMoving = false;

    [SerializeField]
    private float puzzlePotion_x = 0f;
    [SerializeField]
    private float puzzlePotion_y = 1f;
    [SerializeField]
    private float puzzlePotion_z = 2.5f;
    [SerializeField]
    private float puzzleRotate_x = 0f;
    [SerializeField]
    private float puzzleRotate_y = 0f;
    [SerializeField]
    private float puzzleRotate_z = 0f;

    private void Awake()
    {
        puzzleObj = GetComponent<PuzzleObject>();
    }

    private void Start()
    {
        Init();
        Shuffle();
    }

    private void Init()
    {
        // �� ������Ʈ �����Ͽ� �θ�� ����
        if (puzzleParent == null)
        {
            puzzleParent = new GameObject("PuzzleParent");  // "PuzzleParent"��� �̸��� �� ������Ʈ ����
            puzzleParent.transform.position = new Vector3(puzzlePotion_x, puzzlePotion_y, puzzlePotion_z);
            puzzleParent.transform.localRotation = Quaternion.Euler(puzzleRotate_x, puzzleRotate_y, puzzleRotate_z);
        }

        int n = 0;
        for (int y = 3; y >= 0; y--)
            for (int x = 0; x < 4; x++)
            {
                Slide_img box = Instantiate(img_Prefab, new Vector2(x, y), Quaternion.identity);
                box.Init(x, y, n + 1, sprites[n], HandleClick, puzzleParent);

                float vr_scale = box.vr_scale;
                box.transform.localScale = new Vector3(vr_scale, vr_scale, vr_scale);
                // ���� ������ puzzleParent ������Ʈ�� �ڽ����� ����
                //box.transform.SetParent(puzzleParent.transform);
                // ���� ȸ���� �ڽ� �ʱ�ȭ
                box.transform.localRotation = Quaternion.identity;

                boxes[x, y] = box;
                n++;
            }
    }

    public void SetMove(bool isMoving)
    {
        this.isMoving = !isMoving;
    }

    public bool GetMove()
    {
        return isMoving;
    }

    private void Shuffle()
    {
        int emptyX = -1, emptyY = -1;

        // ��ĭ ��ġ ã��
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

        int shuffleSteps = 7; // ���� �ݺ� Ƚ��
        for (int i = 0; i < shuffleSteps; i++)
        {
            Vector2 move = getValidMove(emptyX, emptyY);

            // ��ĭ�� ��ȯ
            int newX = emptyX + (int)move.x;
            int newY = emptyY + (int)move.y;

            Swap(emptyX, emptyY, (int)move.x, (int)move.y);

            // ��ĭ ��ġ ����
            emptyX = newX;
            emptyY = newY;
        }
    }

    void Swap(int x, int y, int dx, int dy)
    {
        if (dx == 0 && dy == 0)
            return;  // ��ȯ�� �ʿ� ����

        var from = boxes[x, y];
        var target = boxes[x + dx, y + dy];

        boxes[x, y] = target;
        boxes[x + dx, y + dy] = from;

        from.UpdatePos(x + dx, y + dy, true);
        target.UpdatePos(x, y, true);
    }

    private void HandleClick(int clickedX, int clickedY)
    {
        
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

        CheckIfSolved();// ����Ȯ��
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
            currentTile.UpdatePos(x, y); // �ִϸ��̼� ���� �̵�
        }

        // ��ĭ �̵�

        boxes[x, clickedY] = emptyTile;
        emptyTile.UpdatePos(x, clickedY);

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

    //������ �������� Ȯ��
    void CheckIfSolved()
    {
        int count = 0;
        bool isSolved = true;

        for (int y = 3; y >= 0; y--)
        {
            for (int x = 0; x < 4; x++)
            {
                // �� ���� ������ (x, y) ��ġ�� �ùٸ��� �ִ��� Ȯ��
                int correctIndex = (3 - y) * 4 + x + 1; // �ùٸ� index ���
                if (boxes[x, y].index != correctIndex)
                {
                    isSolved = false;
                    break; // ������ Ʋ�� ��� �� �ߴ�
                }
                else
                {
                    count++;
                }
            }
            if (!isSolved) break;
            if(count == 16)
            {
                Debug.Log("�����Դϴ�");
                puzzleObj.SolvePuzzle();
                OnDestroy();
            }
        }
    }

    private void cheatkey()
    {
        puzzleObj.SolvePuzzle();
        OnDestroy();
    }


    private void OnDestroy()
    {
        Destroy(puzzleParent);
    }

    private void Update()
    {
        if (Input.GetKeyDown("l"))
        {
            Debug.Log("����ѳ�");
            cheatkey();
        }
            
    }

}
