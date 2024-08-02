using UnityEngine;
using System;
using UnityEngine.Rendering;
using TMPro;
using UnityEngine.UI;

public class ResizableTextContent : MonoBehaviour
{
    public TMP_Text textComponent;
    public Transform backgroundTransform;
    public Renderer backgroundRenderer;
    public float padding = 0.01f;

    private float backgroundToTextRatio = 0.01f;

    void Start()
    {
        AdjustBackgroundSize();
    }

    void Update()
    {
        // AdjustBackgroundSize();
    }

    void AdjustBackgroundSize()
    {
        if (textComponent != null && backgroundTransform != null)
        {
            // Get the preferred size of the text
            Vector2 textSize = new Vector2(textComponent.preferredWidth, textComponent.preferredHeight);
            Debug.Log(textSize.x + ";" + textSize.y);

            float _padding = padding;
            if (textSize.x == 0 || textSize.y == 0)
            {
                _padding = 0;
            }

            // Adjust the background size with padding
            backgroundTransform.localScale = new Vector3(textSize.x * backgroundToTextRatio + _padding, textSize.y * backgroundToTextRatio + _padding, 1);

            // Center the text relative to the background
            // textComponent.transform.localPosition = new Vector3(-textSize.x / 2, textSize.y / 2, -0.01f); // Slightly offset in Z to avoid clipping
        }
    }

    public void SetText(string newText)
    {
        if (textComponent != null)
        {
            textComponent.text = newText;
            AdjustBackgroundSize();
        }
    }

    public void SetTextColor(Color newColor)
    {
        if (textComponent != null)
        {
            textComponent.color = newColor;
        }
    }

    public void SetBackgroundColor(Color newColor)
    {
        if (backgroundRenderer != null)
        {
            backgroundRenderer.material.color = newColor;
        }
    }
}

