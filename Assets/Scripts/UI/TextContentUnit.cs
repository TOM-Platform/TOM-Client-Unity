using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Rendering;
using System;
using UnityEngine.Rendering;

namespace TOM.Common.UI
{

    public class TextContentUnit : MonoBehaviour
    {
        public TextMesh TxtHeader;
        public TextMesh TxtContent;
        public TextMesh TxtUnit;
        public TextMesh TxtFooter;
        public GameObject Backpanel;

        public enum TextContentUnitType
        {
            Header,
            Content,
            Unit,
            Footer
        }

        private string Header { get; set; }
        private string Content { get; set; }
        private string Unit { get; set; }
        private string Footer { get; set; }
        private string BackpanelColor { get; set; }
        private string TextColor { get; set; }

        private string HeaderNullMessage = "[UpdateHeader] Header TextMesh is null.";
        private string ContentNullMessage = "[UpdateContent] Content TextMesh is null.";
        private string UnitNullMessage = "[UpdateUnit] Unit TextMesh is null.";
        private string FooterNullMessage = "[UpdateFooter] Footer TextMesh is null.";


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void UpdateTextMesh(TextMesh textMesh, string text, string errorMessage, ref string property)
        {
            if (textMesh == null)
            {
                Debug.LogError(errorMessage);
                return;
            }

            property = text;
            textMesh.text = property;
        }

        public void UpdateText(string text, TextContentUnitType contentUnitType)
        {
            switch (contentUnitType)
            {
                case TextContentUnitType.Header:
                    string updatedHeader = text;
                    UpdateTextMesh(TxtHeader, text, HeaderNullMessage, ref updatedHeader);
                    Header = updatedHeader;
                    break;
                case TextContentUnitType.Content:
                    string updatedContent = text;
                    UpdateTextMesh(TxtContent, text, ContentNullMessage, ref updatedContent);
                    Content = updatedContent;
                    break;
                case TextContentUnitType.Unit:
                    string updatedUnit = text;
                    UpdateTextMesh(TxtUnit, text, UnitNullMessage, ref updatedUnit);
                    Unit = updatedUnit;
                    break;
                case TextContentUnitType.Footer:
                    string updatedFooter = text;
                    UpdateTextMesh(TxtFooter, text, FooterNullMessage, ref updatedFooter);
                    Footer = updatedFooter;
                    break;
                default:
                    Debug.LogError("Unknown TextContentUnitType: " + contentUnitType);
                    break;
            }
        }

        public void UpdateBackpanelColor(string rgbaHexColor)
        {
            try
            {
                Color newCol;
                if (ColorUtility.TryParseHtmlString(rgbaHexColor, out newCol))
                {
                    MeshRenderer meshRenderer = Backpanel.GetComponent<MeshRenderer>();
                    if (meshRenderer != null)
                    {
                        meshRenderer.material.SetColor("_Color", newCol);
                        BackpanelColor = rgbaHexColor;
                    }
                    else
                    {
                        Debug.LogError("MeshRenderer component not found on Backpanel object.");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception occurred while updating backpanel color: " + e.Message);
            }
        }

        // rgbaHexColor is in hex format such as #RGBA, #RGB
        public void UpdateTextColor(string rgbaHexColor)
        {
            Color newCol;
            if (ColorUtility.TryParseHtmlString(rgbaHexColor, out newCol))
            {
                TxtContent.color = newCol;
                TxtUnit.color = newCol;
                TextColor = rgbaHexColor;
            }
        }
    }

}
