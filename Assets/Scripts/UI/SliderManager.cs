using System;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class SliderManager : MonoBehaviour
{
    public Action<float> OnValueChanged;

    [SerializeField]
    private Slider slider = null;
    [SerializeField]
    private TextMeshProUGUI text_Value = null;

    public void Init(float _value)
    {
        if (slider != null)
        {
            slider.onValueChanged.AddListener(ValueChanged);
            slider.value = _value;
            if (text_Value != null)
            {
                text_Value.text = _value.ToString();
            }
        }
    }

    public void ValueChanged(float _value)
    {
        if (text_Value != null)
        {
            text_Value.text = _value.ToString("0.00");
        }

        OnValueChanged?.Invoke(_value);
    }
}
