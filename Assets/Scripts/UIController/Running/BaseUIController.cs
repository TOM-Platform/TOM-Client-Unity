using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TextContentUnit;
using static RunningTypePositonMapping;
using UnityEngine.UI;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine.Events;

public abstract class BaseUIController : MonoBehaviour
{
    private const string whiteColor = "#FFFFFF";
    private const string defaultBackpanelColor = "#00000014";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUIVisible(bool isVisible)
    {
        GameObject mainUI = getRootObject();
        if (mainUI == null)
        {
            Debug.LogError("Unknown main UI");
            return;
        }

        mainUI.SetActive(isVisible);        
    }

    public Boolean getIsVisible()
    {
        GameObject mainUI = getRootObject();
        if (mainUI == null)
        {
            Debug.LogError("Unknown main UI");
            return false;
        }

        return mainUI.activeSelf;
    }

    public abstract void ResetUI();
    protected abstract GameObject getRootObject();
    protected abstract GameObject getPanel(UIPosition position);

    public virtual void UpdateText(UIPosition position, string value, TextContentUnitType contentUnitType)
    {
        GameObject panel = getPanel(position);
        if (panel == null)
        {
            Debug.LogError("[UpdateText] Unknown panel position: " + position);
            return;
        }

        if (contentUnitType != TextContentUnitType.Unit)
        {
            if (string.IsNullOrEmpty(value))
            {
                panel.SetActive(false);
            }
            else
            {
                if (!(position == getRunningTypePosition(UIDataType.RunningTarget) && contentUnitType == TextContentUnitType.Footer))
                {
                    panel.SetActive(true);
                }
                panel.GetComponent<TextContentUnit>().UpdateText(value, contentUnitType);
            }
        }
    }

    public virtual void UpdateButtonTextAndIcon(UIPosition position, string text, Sprite icon, UnityAction function)
    {
        GameObject button = getPanel(position);
        if (button == null)
        {
            Debug.LogError("[UpdateButtonTextAndIcon] Unknown panel position: " + position);
            return;
        }

        try
        {
            Image iconComponent = button.GetComponentInChildren<Image>();
            iconComponent.sprite = icon;
        }
        catch (Exception e)
        {
            Debug.LogError("[UpdateButtonTextAndIcon] Error setting icon: " + e.Message);
        }

        try
        {
            TextMeshPro buttonText = button.GetComponentInChildren<TextMeshPro>();
            buttonText.text = text;
        }
        catch (Exception e)
        {
            Debug.LogError("[UpdateButtonTextAndIcon] Error setting text: " + e.Message);
        }
        
        try
        {
            Interactable interactable = button.GetComponent<Interactable>();
            interactable.OnClick.RemoveAllListeners();
            interactable.OnClick.AddListener(function);
        }
        catch (Exception e)
        {
            Debug.LogError("[UpdateButtonTextAndIcon] Error setting onClick function: " + e.Message);
        }
    }

    public void UpdateBackgroundColor(UIPosition position, Boolean alert, string color)
    {
        GameObject panel = getPanel(position);
        if (panel == null)
        {
            Debug.LogError("[UpdateBackgroundColor] Unknown panel position: " + position);
            return;
        }
        string updatedColor;
        if (alert)
        {
            updatedColor = color;
        }
        else
        {
            updatedColor = defaultBackpanelColor;
        }

        panel.GetComponent<TextContentUnit>().UpdateBackpanelColor(updatedColor);
    }

    public void UpdateTextColor(UIPosition position, Boolean alert, string color)
    {
        GameObject panel = getPanel(position);
        if (panel == null)
        {
            Debug.LogError("[UpdateTextColor] Unknown panel position: " + position);
            return;
        }

        string updatedColor;
        if (alert)
        {
            updatedColor = color;
        }
        else
        {
            updatedColor = whiteColor;
        }

        panel.GetComponent<TextContentUnit>().UpdateTextColor(updatedColor);
    }
   
}


public enum UIPosition
{
    Top = 0,
    TopLeft = 1,
    TopCenter = 2,
    TopRight = 3,
    BottomLeftTop = 4,
    BottomLeftBottom = 5,
    BottomRight = 6,
    SelectionLeft = 7,
    SelectionRight = 8,
    Unsupported = 9
}
