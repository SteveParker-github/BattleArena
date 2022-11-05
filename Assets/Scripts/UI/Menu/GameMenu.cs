using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private string[] enemyTypes;
    private Button quickGameButton;
    private Button tournamentButton;
    private Button exitMenuButton;
    private Button exitGameButton;
    private PlayerInfo playerInfo;
    // private Button button;

    void Awake()
    {
        EventSystem.current.SetSelectedGameObject(null);

        Transform buttons = transform.GetChild(0).GetChild(1);
        quickGameButton = buttons.GetChild(0).GetComponent<Button>();
        tournamentButton = buttons.GetChild(1).GetComponent<Button>();
        exitMenuButton = buttons.GetChild(2).GetComponent<Button>();
        exitGameButton = buttons.GetChild(3).GetComponent<Button>();

        playerInfo = GameObject.FindObjectOfType<PlayerInfo>();

        quickGameButton.GetComponent<Button>().onClick.AddListener(QuickGameOnClick);
        tournamentButton.GetComponent<Button>().onClick.AddListener(TournamentOnClick);
        exitMenuButton.GetComponent<Button>().onClick.AddListener(ExitMenuOnClick);
        exitGameButton.GetComponent<Button>().onClick.AddListener(ExitGameOnClick);
    }

    private void QuickGameOnClick()
    {
        GameObject spawnEnemies = Instantiate(Resources.Load<GameObject>("SpawnEnemies/SpawnEnemies"));
        spawnEnemies.GetComponent<SpawnEnemies>().RandomEnemies(playerInfo.Level, enemyTypes[Random.Range(0, enemyTypes.Length)]);
        playerInfo.GameMode = "Quick Game";
        SceneManager.LoadScene("BattleScene");
    }

    private void TournamentOnClick()
    {
        string path = Application.persistentDataPath + "/SaveGame/";
        string filePath = path + "/" + playerInfo.PlayerName + "/Tournament.json";

        if (!File.Exists(filePath))
        {
            NewTournament();
        }

        SceneManager.LoadScene("TournamentScene");
    }

    private void NewTournament()
    {
        string[] round1 = new string[16];
        round1[0] = playerInfo.PlayerName;

        FightersTeam[] fightersTeams = new FightersTeam[16];

        FightersTeam fightersTeam = new FightersTeam();
        fightersTeam.fightersName = playerInfo.PlayerName;
        fightersTeams[0] = fightersTeam;

        for (int i = 1; i < fightersTeams.Length; i++)
        {
            FightersTeam newFightersTeam = new FightersTeam();
            newFightersTeam.fightersName = "Fighter " + i;
            fightersTeams[i] = newFightersTeam;
            round1[i] = newFightersTeam.fightersName;
            Fighter[] fighters = new Fighter[Random.Range(1, 4)];
            string enemyType = enemyTypes[Random.Range(0, enemyTypes.Length)];
            int minLevel = playerInfo.Level;
            int maxLevel = Mathf.CeilToInt(minLevel + (9 / fighters.Length));

            for (int j = 0; j < fighters.Length; j++)
            {
                Fighter fighter = new Fighter();
                fighter.enemyType = enemyType;
                fighter.level = Random.Range(minLevel, maxLevel);
                fighters[j] = fighter;
            }

            fightersTeams[i].fighters = fighters;
        }

        Round[] round = new Round[] {new Round(), new Round(), new Round(), new Round()};

        round[0].round = round1;
        round[1].round = new string[8];
        round[2].round = new string[4];
        round[3].round = new string[4];

        Prize[] prizes = new Prize[] { new Prize(), new Prize(), new Prize(), new Prize()};

        for (int i = 0; i < prizes.Length; i++)
        {
            int basePrize = (int)(80 / Mathf.Pow(2, i));
            prizes[i].prizeMoney = basePrize * playerInfo.Level;
            prizes[i].prizeEXP = basePrize * 2 * playerInfo.Level;
        }

        TournamentFile tournamentFile = new TournamentFile();
        tournamentFile.roundNumber = 1;
        tournamentFile.fightersTeams = fightersTeams;
        tournamentFile.rounds = round;
        tournamentFile.winners = new string[4];
        tournamentFile.prizes = prizes;       

        string path = Application.persistentDataPath + "/SaveGame/";
        string saveFolder = path + "/" + playerInfo.PlayerName;

        string json = JsonUtility.ToJson(tournamentFile);

        if (!Directory.Exists(saveFolder))
        {
            Directory.CreateDirectory(saveFolder);
        }

        StreamWriter writer = new StreamWriter(saveFolder + "/Tournament.json");
        writer.AutoFlush = true;
        writer.Write(json);
        writer.Close();
    }

    private void ExitMenuOnClick()
    {
        playerInfo.SaveGame();
        Destroy(playerInfo.gameObject);
        SceneManager.LoadScene("MenuScene");
    }

    private void ExitGameOnClick()
    {
        playerInfo.SaveGame();
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
