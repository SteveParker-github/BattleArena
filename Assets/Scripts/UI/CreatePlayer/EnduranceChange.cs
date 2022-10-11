using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnduranceChange : MonoBehaviour
{
    private Button decreaseButton;
    private Button increaseButton;
    private TextMeshProUGUI statText;
    private TextMeshProUGUI statPointText;
    private TextMeshProUGUI maxHealthText;
    private PlayerInfo playerInfo;

    // Start is called before the first frame update
    void Start()
    {
        decreaseButton = transform.GetChild(0).GetComponent<Button>();
        increaseButton = transform.GetChild(1).GetComponent<Button>();
        statText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        statPointText = transform.parent.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>();
        maxHealthText = transform.parent.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        playerInfo = GameObject.FindObjectOfType<PlayerInfo>();

        decreaseButton.onClick.AddListener(DecreaseStat);
        increaseButton.onClick.AddListener(IncreaseStat);
    }

    // Update is called once per frame
    void Update()
    {
        int stat = int.Parse(statText.text);
        decreaseButton.interactable = stat > 3;
        increaseButton.interactable = int.Parse(statPointText.text) > 0 && stat < 10;
    }

    private void DecreaseStat()
    {
        int stat = int.Parse(statText.text) - 1;
        statText.text = (stat).ToString();

        int statPoints = int.Parse(statPointText.text);
        statPointText.text = (statPoints + 1).ToString();

        UpdateStat(stat);
    }

    private void IncreaseStat()
    {
        int stat = int.Parse(statText.text) + 1;
        statText.text = (stat).ToString();

        int statPoints = int.Parse(statPointText.text);
        statPointText.text = (statPoints - 1).ToString();

        UpdateStat(stat);
    }

    private void UpdateStat(int newStat)
    {
        int maxHealth = playerInfo.UpdateEndurance(newStat);
        maxHealthText.text = maxHealth.ToString();
    }
}
