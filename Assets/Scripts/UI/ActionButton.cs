using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionButton : MonoBehaviour
{
    private Vector4 BASECOLOUR = new Vector4(0.45f, 0, 0, 1);
    private Vector4 HIGHLIGHTCOLOUR = new Vector4(0.8f, 0, 0, 1);
    private Image buttonBackground;
    private TextMeshProUGUI abilityText;

    void Awake()
    {
       buttonBackground = GetComponent<Image>();
       abilityText = transform.GetChild(1).GetComponent<TextMeshProUGUI>(); 
    }

    public void UpdateText(string text)
    {
        abilityText.text = text;
    }

    public void StopHighlight()
    {
        buttonBackground.color = BASECOLOUR;
    }

    public void StartHighlight()
    {
        buttonBackground.color = HIGHLIGHTCOLOUR;
    }
}
