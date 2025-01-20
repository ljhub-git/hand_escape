using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Tile
{
    public enum Direction
    {
        UP, DOWN, LEFT, RIGHT,
    }
    public enum PosNegType
    {
        POS,//볼록
        NEG,//오목
        NONE,//평평
    }

    //여백 기본값 20
    public static int padding = 20;

    //퍼즐 기본 크기 100
    public static int tileSize = 100;

    //크기 조절용 변수
    public float tile_Scale = 1f;

    // 방향과 곡선타입을 저장하는 라인랜더러
    private Dictionary<(Direction, PosNegType), LineRenderer> mLineRenderers
      = new Dictionary<(Direction, PosNegType), LineRenderer>();

    //베지어 곡선의 포인트 리스트 저장, 곡선 활용 템플릿
    public static List<Vector2> BezCurve =
      BezierCurve.PointList2(TemplateBezierCurve.templateControlPoints, 0.001f);

    //원본 텍스쳐
    private Texture2D mOriginalTexture;
    
    //퍼즐 조각 모양 저장
    public Texture2D finalCut { get; private set; }

    public static readonly Color TransparentColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);

    //방향과 곡선을 저장하는 배열
    private PosNegType[] mCurveTypes = new PosNegType[4]
    {
    PosNegType.NONE,
    PosNegType.NONE,
    PosNegType.NONE,
    PosNegType.NONE,
    };

    //Flood Fill 알고리즘 탐색 배열
    private bool[,] mVisited;

    //Flood Fill 알고리즘의 스택
    private Stack<Vector2Int> mStack = new Stack<Vector2Int>();

    public int xIndex = 0;
    public int yIndex = 0;

    public float GetTileScale()
    {
        return tile_Scale;
    }
    public void SetTileScale(float puzzleScale)
    {
        tile_Scale = puzzleScale;
    }

    // For tiles sorting.
    public static TilesSorting tilesSorting = new TilesSorting();
    public void SetCurveType(Direction dir, PosNegType type) //곡선 유형 설정
    {
        mCurveTypes[(int)dir] = type;
    }

    public PosNegType GetCurveType(Direction dir) // 곡선 유형 반환
    {
        return mCurveTypes[(int)dir];
    }

    public Tile(Texture2D texture) // 원본 텍스쳐를 받아와서 여백 및 크기기준으로 초기화
    {
        mOriginalTexture = texture;
        //int padding = mOffset.x;
        int tileSizeWithPadding = 2 * padding + tileSize;

        finalCut = new Texture2D(tileSizeWithPadding, tileSizeWithPadding, TextureFormat.ARGB32, false);

        for (int i = 0; i < tileSizeWithPadding; ++i)
        {
            for (int j = 0; j < tileSizeWithPadding; ++j)
            {
                finalCut.SetPixel(i, j, TransparentColor);
            }
        }
    }

    public void Apply()
    {
        FloodFillInit();
        FloodFill();
        finalCut.Apply();
    }

    void FloodFillInit() //Flood Fill 초기화
    {
        //int padding = mOffset.x;
        int tileSizeWithPadding = 2 * padding + tileSize;

        mVisited = new bool[tileSizeWithPadding, tileSizeWithPadding]; //좌표저장
        for (int i = 0; i < tileSizeWithPadding; ++i)
        {
            for (int j = 0; j < tileSizeWithPadding; ++j)
            {
                mVisited[i, j] = false;
            }
        }

        List<Vector2> pts = new List<Vector2>();
        for (int i = 0; i < mCurveTypes.Length; ++i)
        {
            pts.AddRange(CreateCurve((Direction)i, mCurveTypes[i]));
        }

        // Now we should have a closed curve.
        for (int i = 0; i < pts.Count; ++i)
        {
            mVisited[(int)pts[i].x, (int)pts[i].y] = true;
        }
        // start from the center.
        Vector2Int start = new Vector2Int(tileSizeWithPadding / 2, tileSizeWithPadding / 2);

        mVisited[start.x, start.y] = true;
        mStack.Push(start);
    }

    void Fill(int x, int y) // 좌표를 받아와서 finalCut 텍스처에 색상을 채워넣음
    {
        Color c = mOriginalTexture.GetPixel(x + xIndex * tileSize, y + yIndex * tileSize);
        c.a = 1.0f;
        finalCut.SetPixel(x, y, c);
    }

    void FloodFill() //Flood Fill 알고리즘 실행, 상하좌우순 탐색, 스택에 좌표 추가
    {
        //int padding = mOffset.x;
        int width_height = padding * 2 + tileSize;

        while (mStack.Count > 0)
        {
            Vector2Int v = mStack.Pop();

            int xx = v.x;
            int yy = v.y;

            Fill(v.x, v.y);

            // Check right.
            int x = xx + 1;
            int y = yy;

            if (x < width_height)
            {
                Color c = finalCut.GetPixel(x, y);
                if (!mVisited[x, y])
                {
                    mVisited[x, y] = true;
                    mStack.Push(new Vector2Int(x, y));
                }
            }

            // check left.
            x = xx - 1;
            y = yy;
            if (x > 0)
            {
                Color c = finalCut.GetPixel(x, y);
                if (!mVisited[x, y])
                {
                    mVisited[x, y] = true;
                    mStack.Push(new Vector2Int(x, y));
                }
            }

            // Check up.
            x = xx;
            y = yy + 1;

            if (y < width_height)
            {
                Color c = finalCut.GetPixel(x, y);
                if (!mVisited[x, y])
                {
                    mVisited[x, y] = true;
                    mStack.Push(new Vector2Int(x, y));
                }
            }

            // Check down.
            x = xx;
            y = yy - 1;

            if (y >= 0)
            {
                Color c = finalCut.GetPixel(x, y);
                if (!mVisited[x, y])
                {
                    mVisited[x, y] = true;
                    mStack.Push(new Vector2Int(x, y));
                }
            }
        }
    }

    public static LineRenderer CreateLineRenderer(UnityEngine.Color color, float lineWidth = 1.0f)
    {
        GameObject obj = new GameObject();
        LineRenderer lr = obj.AddComponent<LineRenderer>();

        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        return lr;
    }

    public static void TranslatePoints(List<Vector2> iList, Vector2 offset) // 곡선 포인트 이동
    {
        for (int i = 0; i < iList.Count; i++)
        {
            iList[i] += offset;
        }
    }

    public static void InvertY(List<Vector2> iList) // 포인트 반전
    {
        for (int i = 0; i < iList.Count; i++)
        {
            iList[i] = new Vector2(iList[i].x, -iList[i].y);
        }
    }

    public static void SwapXY(List<Vector2> iList) // 포인트 교체
    {
        for (int i = 0; i < iList.Count; ++i)
        {
            iList[i] = new Vector2(iList[i].y, iList[i].x);
        }
    }

    public List<Vector2> CreateCurve(Direction dir, PosNegType type) // 방향과 곡선유형을 받아와서 포인트 생성, NONE때는 직선
    {
        int padding_x = padding;// mOffset.x;
        int padding_y = padding;// mOffset.y;
        int sw = tileSize;
        int sh = tileSize;

        List<Vector2> pts = new List<Vector2>(BezCurve);
        switch (dir)
        {
            case Direction.UP:
                if (type == PosNegType.POS)
                {
                    TranslatePoints(pts, new Vector2(padding_x, padding_y + sh));
                }
                else if (type == PosNegType.NEG)
                {
                    InvertY(pts);
                    TranslatePoints(pts, new Vector2(padding_x, padding_y + sh));
                }
                else
                {
                    pts.Clear();
                    for (int i = 0; i < 100; ++i)
                    {
                        pts.Add(new Vector2(i + padding_x, padding_y + sh));
                    }
                }
                break;
            case Direction.RIGHT:
                if (type == PosNegType.POS)
                {
                    SwapXY(pts);
                    TranslatePoints(pts, new Vector2(padding_x + sw, padding_y));
                }
                else if (type == PosNegType.NEG)
                {
                    InvertY(pts);
                    SwapXY(pts);
                    TranslatePoints(pts, new Vector2(padding_x + sw, padding_y));
                }
                else
                {
                    pts.Clear();
                    for (int i = 0; i < 100; ++i)
                    {
                        pts.Add(new Vector2(padding_x + sw, i + padding_y));
                    }
                }
                break;
            case Direction.DOWN:
                if (type == PosNegType.POS)
                {
                    InvertY(pts);
                    TranslatePoints(pts, new Vector2(padding_x, padding_y));
                }
                else if (type == PosNegType.NEG)
                {
                    TranslatePoints(pts, new Vector2(padding_x, padding_y));
                }
                else
                {
                    pts.Clear();
                    for (int i = 0; i < 100; ++i)
                    {
                        pts.Add(new Vector2(i + padding_x, padding_y));
                    }
                }
                break;
            case Direction.LEFT:
                if (type == PosNegType.POS)
                {
                    InvertY(pts);
                    SwapXY(pts);
                    TranslatePoints(pts, new Vector2(padding_x, padding_y));
                }
                else if (type == PosNegType.NEG)
                {
                    SwapXY(pts);
                    TranslatePoints(pts, new Vector2(padding_x, padding_y));
                }
                else
                {
                    pts.Clear();
                    for (int i = 0; i < 100; ++i)
                    {
                        pts.Add(new Vector2(padding_x, i + padding_y));
                    }
                }
                break;
        }
        return pts;
    }

    public void DrawCurve(Direction dir, PosNegType type, Color color) // 곡선 포인트 설정, 라인 랜더러 추가
    {
        if (!mLineRenderers.ContainsKey((dir, type)))
        {
            mLineRenderers.Add((dir, type), CreateLineRenderer(color));
        }

        LineRenderer lr = mLineRenderers[(dir, type)];
        lr.gameObject.SetActive(true);
        lr.startColor = color;
        lr.endColor = color;
        lr.gameObject.name = "LineRenderer_" + dir.ToString() + "_" + type.ToString();
        List<Vector2> pts = CreateCurve(dir, type);

        lr.positionCount = pts.Count;
        for (int i = 0; i < pts.Count; ++i)
        {
            lr.SetPosition(i, pts[i]);
        }
    }

    public void HideAllCurves() // 곡선 숨김
    {
        foreach (var item in mLineRenderers)
        {
            item.Value.gameObject.SetActive(false);
        }
    }

    public void DestroyAllCurves() // 곡선 제거
    {
        foreach (var item in mLineRenderers)
        {
            GameObject.Destroy(item.Value.gameObject);
        }

        mLineRenderers.Clear();
    }

}