using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpEndurance : MonoBehaviour
{
    private Button decreaseButton;
    private Button increaseButton;
    private TextMeshProUGUI statText;
    private TextMeshProUGUI statPointText;
    private TextMeshProUGUI maxHealthText;
    private PlayerInfo playerInfo;
    private int defaultStat;

    // Start is called before the first frame update
    void Start()
    {
        decreaseButton = transform.GetChild(0).GetComponent<Button>();
        increaseButton = transform.GetChild(1).GetComponent<Button>();
        statText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        statPointText = transform.parent.GetChild(7).GetChild(1).GetComponent<TextMeshProUGUI>();
        maxHealthText = transform.parent.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>();
        playerInfo = GameObject.FindObjectOfType<PlayerInfo>();

        statText.text = playerInfo.Endurance.ToString();
        defaultStat = playerInfo.Endurance;
        maxHealthText.text = playerInfo.MaxHealth.ToString();

        decreaseButton.onClick.AddListener(DecreaseStat);
        increaseButton.onClick.AddListener(IncreaseStat);
    }

    // Update is called once per frame
    void Update()
    {
        bool leveledUp = playerInfo.LeveledUp;
        decreaseButton.gameObject.SetActive(leveledUp);
        increaseButton.gameObject.SetActive(leveledUp);

        int stat = int.Parse(statText.text);
        decreaseButton.interactable = stat > defaultStat;
        increaseButton.interactable = int.Parse(statPointText.text) > 0;
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

    public void Respec()
    {
        defaultStat = 3;
        statText.text = "3";
        maxHealthText.text = playerInfo.MaxHealth.ToString();
    }

    public void Reset()
    {
        int difference = playerInfo.Endurance - defaultStat;
        statText.text = defaultStat.ToString();
        statPointText.text = (int.Parse(statPointText.text) + difference).ToString();
        UpdateStat(defaultStat);
    }
}
