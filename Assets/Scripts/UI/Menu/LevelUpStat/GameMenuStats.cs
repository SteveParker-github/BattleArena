using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameMenuStats : MonoBehaviour
{
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI levelText;
    private TextMeshProUGUI expText;
    private TextMeshProUGUI moneyText;
    private GameObject statPointsObject;
    private TextMeshProUGUI statPointsText;
    private GameObject confirmButtonObject;
    private Button confirmButton;
    private GameObject resetButtonObject;
    private Button resetButton;
    private GameObject respecButtonObject;
    private TextMeshProUGUI respecButtonText;
    private Button respecButton;
    private PlayerInfo playerInfo;
    private LevelUpStat strengthLevelUp;
    private LevelUpStat intelligenceLevelUp;
    private LevelUpEndurance enduranceLevelUp;
    // Start is called before the first frame update
    void Start()
    {
        nameText = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        levelText = transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        expText = transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        moneyText = transform.parent.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        statPointsObject = transform.GetChild(7).gameObject;
        statPointsText = statPointsObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        Transform buttons = transform.GetChild(8);
        confirmButtonObject = buttons.GetChild(0).gameObject;
        confirmButton = confirmButtonObject.GetComponent<Button>();
        resetButtonObject = buttons.GetChild(1).gameObject;
        resetButton = resetButtonObject.GetComponent<Button>();
        respecButtonObject = buttons.GetChild(2).gameObject;
        respecButton = respecButtonObject.GetComponent<Button>();
        playerInfo = GameObject.FindObjectOfType<PlayerInfo>();

        strengthLevelUp = transform.GetChild(4).GetComponent<LevelUpStat>();
        intelligenceLevelUp = transform.GetChild(5).GetComponent<LevelUpStat>();
        enduranceLevelUp = transform.GetChild(6).GetComponent<LevelUpEndurance>();

        nameText.text = playerInfo.PlayerName;
        levelText.text = playerInfo.Level.ToString();
        expText.text = playerInfo.GetEXP();
        moneyText.text = playerInfo.Money.ToString();
        statPointsText.text = playerInfo.SkillPoints.ToString();

        confirmButton.onClick.AddListener(ConfirmOnClick);
        resetButton.onClick.AddListener(ResetOnClick);
        respecButton.onClick.AddListener(RespecOnClick);

        respecButtonText = respecButtonObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        
        playerInfo.SaveGame();
    }

    // Update is called once per frame
    void Update()
    {
        bool leveledUp = playerInfo.LeveledUp;

        confirmButtonObject.SetActive(leveledUp);
        resetButtonObject.SetActive(leveledUp);
        respecButtonObject.SetActive(!leveledUp);

        confirmButton.interactable = int.Parse(statPointsText.text) == 0;

        respecButton.interactable = playerInfo.EnoughForRespec(respecButtonText);
    }

    private void ConfirmOnClick()
    {
        playerInfo.SkillPoints = 0;
        playerInfo.LeveledUp = false;
        respecButtonObject.SetActive(true);
        playerInfo.SaveGame();
    }

    private void ResetOnClick()
    {
        strengthLevelUp.Reset();
        intelligenceLevelUp.Reset();
        enduranceLevelUp.Reset();
    }

    private void RespecOnClick()
    {
        moneyText.text = playerInfo.Respec();
        statPointsText.text = playerInfo.SkillPoints.ToString();
        respecButtonObject.SetActive(false);
        strengthLevelUp.Respec();
        intelligenceLevelUp.Respec();
        enduranceLevelUp.Respec();
    }
}
