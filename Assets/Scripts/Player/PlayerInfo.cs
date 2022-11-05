using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class PlayerInfo : MonoBehaviour
{
    protected int baseHealth = 100;
    protected int incrementHealth = 10;
    private int level;
    private int strength;
    private int intelligence;
    private int endurance;
    private int maxHealth;
    private int currentEXP;
    private string playerName;
    private int skillPoints;
    private bool leveledUp;
    private int money;
    private string gameMode;

    public int Level { get => level; }
    public int Strength { get => strength; set => strength = value; }
    public int Intelligence { get => intelligence; set => intelligence = value; }
    public int Endurance { get => endurance; }
    public int MaxHealth { get => maxHealth; }
    public int CurrentEXP { get => currentEXP; }
    public string PlayerName { get => playerName; set => playerName = value; }
    public int SkillPoints { get => skillPoints; set => skillPoints = value; }
    public bool LeveledUp { get => leveledUp; set => leveledUp = value; }
    public int Money { get => money; set => money = value; }
    public string GameMode { get => gameMode; set => gameMode = value; }

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        strength = 3;
        intelligence = 3;
        endurance = 3;
        maxHealth = baseHealth + (level + endurance) * incrementHealth;
        currentEXP = 0;
        skillPoints = 0;
        money = 100;
        leveledUp = false;
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    public int UpdateEndurance(int newStat)
    {
        endurance = newStat;

        return UpdateMaxHealth();
    }

    public int UpdateMaxHealth()
    {
        maxHealth = baseHealth + (level + endurance) * incrementHealth;
        return maxHealth;
    }

    public void UpdateEXP(int earnEXP)
    {
        currentEXP += earnEXP;
        UpdateLevel();
    }

    private void UpdateLevel()
    {
        bool levelUp;
        do
        {
            int nextLevel = (int)((level * 100) * 1.1f);
            levelUp = currentEXP >= nextLevel;

            if (levelUp)
            {
                level++;
                currentEXP -= nextLevel;
                skillPoints += 3;
                UpdateMaxHealth();
                leveledUp = true;
            }
        } while (levelUp);
    }

    public string GetEXP()
    {
        return currentEXP + " / " + ((level * 100) * 1.1f);
    }

    public bool EnoughForRespec(TextMeshProUGUI respecButtonText)
    {
        int cost = level * 100;
        respecButtonText.text = "Respec for " + cost;
        return money >= cost;
    }

    public string Respec()
    {
        money -= level * 100;
        int totalSkillPoints = strength + intelligence + endurance - 9;
        skillPoints += totalSkillPoints;
        strength = 3;
        intelligence = 3;
        endurance = 3;
        UpdateMaxHealth();
        leveledUp = true;
        return money.ToString();
    }

    public void SaveGame()
    {
        string path = Application.persistentDataPath + "/SaveGame/";
        string saveFolder = path + "/" + playerName;

        SaveFile saveFile = new SaveFile();
        saveFile.playerName = playerName;
        saveFile.money = money;
        saveFile.level = level;
        saveFile.strength = strength;
        saveFile.intelligence = intelligence;
        saveFile.endurance = endurance;
        saveFile.maxHealth = maxHealth;
        saveFile.currentEXP = currentEXP;

        string json = JsonUtility.ToJson(saveFile);

        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
        }

        StreamWriter writer = new StreamWriter(saveFolder + "/Save.json");
        writer.AutoFlush = true;
        writer.Write(json);
        writer.Close();
    }

    public void LoadGame(string folderName)
    {
        string path = Application.persistentDataPath + "/SaveGame/";
        string saveFilePath = path + "/" + folderName + "/Save.json";

        if (!File.Exists(saveFilePath))
        {
            return;
        }

        string saveText = File.ReadAllText(saveFilePath);
        SaveFile saveFile = JsonUtility.FromJson<SaveFile>(saveText);

        playerName = saveFile.playerName;
        money = saveFile.money;
        level = saveFile.level;
        strength = saveFile.strength;
        intelligence = saveFile.intelligence;
        endurance = saveFile.endurance;
        maxHealth = saveFile.maxHealth;
        currentEXP = saveFile.currentEXP;
    }
}
