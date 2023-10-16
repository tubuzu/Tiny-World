// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitValueSlider : Slider
{
    public int curUnit = 0;
    public float valuePerUnit = 0f;

    public delegate void OnValueChangedHandler();
    public event OnValueChangedHandler OnValueChangedAction;

    protected override void Awake()
    {
        base.Awake();
        onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float newValue)
    {
        if (valuePerUnit != 0)
            curUnit = Mathf.FloorToInt(newValue / valuePerUnit);
        else curUnit = 0;
        value = curUnit * valuePerUnit;

        OnValueChangedAction?.Invoke();
    }

    public void UpdateValue(int newUnit)
    {
        this.curUnit = newUnit;
        value = curUnit * valuePerUnit;
    }
}
