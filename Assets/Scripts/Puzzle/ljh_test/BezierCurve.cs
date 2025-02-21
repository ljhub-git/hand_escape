using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve //베지어 곡선
{
    private static float[] Factorial = new float[] //0부터 18까지의 팩토리얼 값을 미리 계산
    {
        1.0f,
        1.0f,
        2.0f,
        6.0f,
        24.0f,
        120.0f,
        720.0f,
        5040.0f,
        40320.0f,
        362880.0f,
        3628800.0f,
        39916800.0f,
        479001600.0f,
        6227020800.0f,
        87178291200.0f,
        1307674368000.0f,
        20922789888000.0f,
        355687428096000.0f,
        6402373705728000.0f,
    };
    private static float Binomial(int n, int i) //이항계수
    {
        float ni;
        float a1 = Factorial[n];
        float a2 = Factorial[i];
        float a3 = Factorial[n-i];
        ni = a1 / (a2 * a3);
        return ni;
    }

    private static float Bernstein(int n, int i, float t) //베른슈타인 다항식
    {
        float t_i = Mathf.Pow(t, i);
        float t_n_minus_i = Mathf.Pow((1 - t), (n - i));

        float basis = Binomial(n, i) * t_i * t_n_minus_i;
        return basis;
    }

    public static Vector3 Point2(float t, List<Vector2> controlPoints) //베지어 곡선 계산
    {
        int N = controlPoints.Count - 1;

        if(N>18)
        {
            Debug.Log("you have 18 control points");
            controlPoints.RemoveRange(18, controlPoints.Count - 18);
        }

        if (t <= 0) return controlPoints[0];
        if (t >= 1) return controlPoints[controlPoints.Count - 1];

        Vector2 p = new Vector2();

        for(int i = 0; i <controlPoints.Count; ++i)
        {
            Vector2 bn = Bernstein(N, i, t) * controlPoints[i];
            p += bn;
        }

        return p;
    }

    public static List<Vector2> PointList2(List<Vector2> controlPoints, float interval = 0.01f) //곡선의 모든 점 계산 및 반환
    {
        int N = controlPoints.Count - 1;
        if (N > 18)
        {
            Debug.Log("you have 18 control points");
            controlPoints.RemoveRange(18, controlPoints.Count - 18);
        }

        List<Vector2> points = new List<Vector2>();
        for(float t = 0.0f; t<= 1.0f + interval - 0.0001f; t += interval)
        {
            Vector2 p = new Vector2();
            for (int i = 0; i < controlPoints.Count; ++i)
            {
                Vector2 bn = Bernstein(N, i, t) * controlPoints[i];
                p += bn;
            }
            points.Add(p); 
        }
        return points;
    }
}
