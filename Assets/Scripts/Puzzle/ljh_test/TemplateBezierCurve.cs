using System.Collections.Generic;
using UnityEngine;

public class TemplateBezierCurve : MonoBehaviour //직선과 곡선을 그림
{
    public static readonly List<Vector2> templateControlPoints = new List<Vector2>() //베지어 곡선 제어점 좌표
    {
        new Vector2(0, 0),
        new Vector2(35, 15),
        new Vector2(47, 13),
        new Vector2(45, 5),
        new Vector2(48, 0),
        new Vector2(25, -5),
        new Vector2(15, -18),
        new Vector2(36, -20),
        new Vector2(64, -20),
        new Vector2(85, -18),
        new Vector2(75, -5),
        new Vector2(52, 0),
        new Vector2(55, 5),
        new Vector2(53, 13),
        new Vector2(65, 15),
        new Vector2(100, 0)
    };

    public GameObject PointPrefab;

    LineRenderer[] mLineRenderers = null; // 직선 및 곡선

    List<GameObject> mPointGameObjects = new List<GameObject>(); 

    public float LineWidth;
    public float LineWidthBezier;
    public Color LineColor = new Color(0.5f, 0.5f, 0.5f, 0.8f);
    public Color BezierCurveColor = new Color(0.5f, 0.6f, 0.8f, 0.8f);

    private LineRenderer CreateLine()
    {
        GameObject obj = new GameObject();
        LineRenderer lr = obj.AddComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = LineColor;
        lr.endColor = LineColor;
        lr.startWidth = LineWidth;
        lr.endWidth = LineWidth;
        return lr;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mLineRenderers = new LineRenderer[2];
        mLineRenderers[0] = CreateLine();
        mLineRenderers[1] = CreateLine();

        mLineRenderers[0].gameObject.name = "LineRenderer_obj_0";
        mLineRenderers[1].gameObject.name = "LineRenderer_obj_1";

        for (int i = 0; i < templateControlPoints.Count; i++)
        {
            GameObject obj = Instantiate(PointPrefab, templateControlPoints[i], Quaternion.identity);
            obj.name = "ControlPoint_" + i.ToString();
            mPointGameObjects.Add(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        LineRenderer lineRenderer = mLineRenderers[0]; //점 연결 직선
        LineRenderer curveRenderer = mLineRenderers[1]; // 곡선

        List<Vector2> pts = new List<Vector2>();
        for (int i = 0; i < mPointGameObjects.Count; i++)
        {
            pts.Add(mPointGameObjects[i].transform.position);
        }

        lineRenderer.positionCount = pts.Count; 
        for (int i = 0; i < pts.Count; i++)
        {
            lineRenderer.SetPosition(i, pts[i]); // 제어점 갯수 만큼의 직선
        }

        List<Vector2> curve = BezierCurve.PointList2(pts, 0.01f); //베지어 곡선 계산 및 그리기
        curveRenderer.startColor = BezierCurveColor;
        curveRenderer.endColor = BezierCurveColor;
        curveRenderer.positionCount = curve.Count;
        curveRenderer.startWidth = LineWidthBezier;
        curveRenderer.endWidth = LineWidthBezier;

        for (int i = 0; i < curve.Count; i++)
        {
            curveRenderer.SetPosition(i, curve[i]);
        }
    }
}
