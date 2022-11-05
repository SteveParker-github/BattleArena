using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class HUD : MonoBehaviour
{
    private Transform panel;
    private List<ActionButton> actionButtons;
    private Transform buttonUI;
    private Image healthImage;
    private TextMeshProUGUI healthText;
    private Transform gameEndPanel;
    private Image gameEndImage;
    private TextMeshProUGUI gameEndText;
    private TextMeshProUGUI continueMessageText;
    private bool gameOver;
    private float lAlphaLevel = 0.1f;
    private float uAlphaLevel = 1.0f;
    private float amplitude;
    private float offSet;
    private bool usingKeyboard;
    private bool imageSwapped;

    void Awake()
    {
        buttonUI = transform.GetChild(1);
        actionButtons = new List<ActionButton>();

        for (int i = 0; i < buttonUI.childCount; i++)
        {
            actionButtons.Add(buttonUI.GetChild(i).GetComponent<ActionButton>());
        }

        panel = transform.GetChild(0);
        healthImage = panel.GetChild(0).GetComponent<Image>();
        healthText = panel.GetChild(1).GetComponent<TextMeshProUGUI>();
        gameEndPanel = transform.GetChild(2);
        gameEndImage = gameEndPanel.GetComponent<Image>();
        gameEndText = gameEndPanel.GetChild(0).GetComponent<TextMeshProUGUI>();
        continueMessageText = gameEndPanel.GetChild(1).GetComponent<TextMeshProUGUI>();
        gameOver = false;
        amplitude = 2 / (uAlphaLevel - lAlphaLevel);
        offSet = (uAlphaLevel + lAlphaLevel) / 2;

        usingKeyboard = true;
        imageSwapped = false;
    }

    private void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame && !usingKeyboard)
        {
            usingKeyboard = true;
            imageSwapped = true;
        }

        if (Gamepad.current.wasUpdatedThisFrame && usingKeyboard)
        {
            usingKeyboard = false;
            imageSwapped = true;
        }

        if (imageSwapped)
        {
            for (int i = 0; i < actionButtons.Count; i++)
            {
                actionButtons[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(usingKeyboard);
                actionButtons[i].transform.GetChild(0).GetChild(1).gameObject.SetActive(!usingKeyboard);
            }

            imageSwapped = false;
        }



        if (!gameOver) return;

        Vector4 tempColour = continueMessageText.color;
        float alpha = Mathf.Sin(Time.time * 2f) / amplitude + offSet;
        tempColour.w = alpha;
        continueMessageText.color = tempColour;
        
        tempColour = gameEndImage.color;

        if (tempColour.w >= 1) return;
        tempColour.w += Time.deltaTime / 5;
        gameEndImage.color = tempColour;

    }

    public void UpdateHealth((float, float) health)
    {
        healthImage.fillAmount = health.Item1 / health.Item2;
        healthText.text = health.Item1 + "/" + health.Item2;
    }

    public void UpdateNames(string[] abilityNames)
    {
        for (int i = 0; i < abilityNames.Length; i++)
        {
            actionButtons[i].UpdateText(abilityNames[i]);
        }
    }

    public void StartHighlight(int index)
    {
        actionButtons[index].StartHighlight();
    }

    public void StopHighlight(int index)
    {
        actionButtons[index].StopHighlight();
    }

    public void EndGame(bool hasPlayerWon)
    {
        buttonUI.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
        gameEndPanel.gameObject.SetActive(true);
        gameOver = true;

        if (hasPlayerWon)
        {
            PlayerWon();
            return;
        }

        PlayerLost();
    }

    private void PlayerWon()
    {
        gameEndText.text = "Victory!";
        gameEndText.color = new Vector4(0.8f, 0.7f, 0, 1);
        gameEndImage.color = new Vector4(1, 1, 1, 0);
    }

    private void PlayerLost()
    {
        gameEndText.text = "You're Dead!";
        gameEndText.color = new Vector4(0.6f, 0.01f, 0, 1);
    }
}
