using System.Collections;
using UnityEngine;

public class ChangeTransparency
{
    private Material material;

    public ChangeTransparency(Material material)
    {
        this.material = material;
    }

    public IEnumerator FadeTransparency(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;

        // 투명도로 변화
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            SetMaterialAlpha(alpha);
            yield return null;
        }

        // 정확히 목표 알파 값으로 설정
        SetMaterialAlpha(endAlpha);

        // 복원
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(endAlpha, startAlpha, elapsedTime / duration);
            SetMaterialAlpha(alpha);
            yield return null;
        }

        // 정확히 원래 알파 값으로 설정
        SetMaterialAlpha(startAlpha);
    }

    public void SetMaterialAlpha(float alpha)
    {
        if (material == null) return;

        Color newColor = material.color;
        newColor.a = alpha;
        material.color = newColor;
    }
}
