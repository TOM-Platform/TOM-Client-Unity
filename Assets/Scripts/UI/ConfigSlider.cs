using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class ConfigSlider : MonoBehaviour
{
    public string header;
    public float min = 0;
    public float max = 1;
    public bool useStepDivisions = true;
    public int numStepDivisions = 10;
    public string unit;
    public TextContentUnit textContentUnit;
    public PinchSlider pinchSlider;

    // Start is called before the first frame update
    void Start()
    {
        RunInitSequence(InitSliderSettings, InitText);
    }

    // Update is called once per frame
    void Update()
    {
        textContentUnit.UpdateText(
            GetCurrentValue().ToString(),
            TextContentUnit.TextContentUnitType.Content
        );
    }

    public void SetCurrentValue(float newValue)
    {
        float convertedValue = (newValue - min) / (max - min);
        pinchSlider.SliderValue = convertedValue;
    }

    public float GetCurrentValue()
    {
        float currentValue = pinchSlider.SliderValue * (max - min) + min;
        float step = (max - min) / numStepDivisions;
        float steppedValue = Mathf.Round(currentValue / step) * step;

        return steppedValue;
    }

    // Runs init functions sequentially, take in variable number of arguments.
    private void RunInitSequence(params Action[] initFuncs)
    {
        foreach (var initFunc in initFuncs)
        {
            initFunc?.Invoke();
        }
    }

    private void InitSliderSettings()
    {
        if (pinchSlider == null)
        {
            return;
        }

        pinchSlider.UseSliderStepDivisions = useStepDivisions;
        pinchSlider.SliderStepDivisions = numStepDivisions;
    }

    private void InitText()
    {
        textContentUnit.UpdateText(header, TextContentUnit.TextContentUnitType.Header);
        textContentUnit.UpdateText(unit, TextContentUnit.TextContentUnitType.Unit);
        textContentUnit.UpdateText(
            GetCurrentValue().ToString(),
            TextContentUnit.TextContentUnitType.Content
        );
    }
}
