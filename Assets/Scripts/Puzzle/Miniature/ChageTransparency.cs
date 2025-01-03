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

        // ������ ��ȭ
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            SetMaterialAlpha(alpha);
            yield return null;
        }

        // ��Ȯ�� ��ǥ ���� ������ ����
        SetMaterialAlpha(endAlpha);

        // ����
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(endAlpha, startAlpha, elapsedTime / duration);
            SetMaterialAlpha(alpha);
            yield return null;
        }

        // ��Ȯ�� ���� ���� ������ ����
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
