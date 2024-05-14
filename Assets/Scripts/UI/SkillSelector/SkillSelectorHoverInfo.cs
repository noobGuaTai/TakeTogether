using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSelectorHoverInfo : MonoBehaviour
{
    private string info;
    private Text descriptionText;

    public void SetInfo(string info, Text descriptionText)
    {
        this.info = info;
        this.descriptionText = descriptionText;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionText.text = info;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionText.text = "";
    }
}
