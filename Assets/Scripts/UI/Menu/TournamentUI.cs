using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TournamentUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> buttons;
    private PrizeUI prizeUI;
    private PlayerInfo playerInfo;
    private TournamentFile tournamentFile;
    private bool isShowingPrizeUI;
    // Start is called before the first frame update
    void Awake()
    {
        prizeUI = transform.GetChild(0).GetChild(3).GetComponent<PrizeUI>();
        playerInfo = GameObject.FindObjectOfType<PlayerInfo>();

        EventSystem.current.SetSelectedGameObject(null);

        buttons[0].GetComponent<Button>().onClick.AddListener(NextFightOnClick);
        buttons[1].GetComponent<Button>().onClick.AddListener(PrizeWinnersOnClick);
        buttons[2].GetComponent<Button>().onClick.AddListener(BackOnClick);

        GetFile();
        UpdateRounds();

        if (!CanFight())
        {
            buttons[0].GetComponent<Button>().interactable = false;
            DeleteFile();
        }

        isShowingPrizeUI = false;
    }

    private void GetFile()
    {
        string path = Application.persistentDataPath + "/SaveGame/";
        string filePath = path + "/" + playerInfo.PlayerName + "/Tournament.json";

        if (!File.Exists(filePath))
        {
            SceneManager.LoadScene("GameMenuScene");
            return;
        }

        string fileText = File.ReadAllText(filePath);
        tournamentFile = JsonUtility.FromJson<TournamentFile>(fileText);
    }

    private void UpdateRounds()
    {
        if (tournamentFile == null) return;

        //update round title
        transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = tournamentFile.roundNumber.ToString();

        //update round fighters
        Transform roundTransform = transform.GetChild(0).GetChild(2);

        //Loop each round
        for (int i = 0; i < 4; i++)
        {
            Transform currentRound = roundTransform.GetChild(i);
            string[] roundName = tournamentFile.rounds[i].round;
            List<TextMeshProUGUI> fighterText = new List<TextMeshProUGUI>();
            Transform firstColumn = currentRound.GetChild(0);
            Transform secondColumn = currentRound.GetChild(1);

            for (int j = 0; j < firstColumn.childCount; j++)
            {
                fighterText.Add(firstColumn.GetChild(j).GetChild(0).GetComponent<TextMeshProUGUI>());
            }

            for (int j = 0; j < secondColumn.childCount; j++)
            {
                fighterText.Add(secondColumn.GetChild(j).GetChild(0).GetComponent<TextMeshProUGUI>());
            }

            for (int j = 0; j < roundName.Length; j++)
            {
                if (i == tournamentFile.roundNumber -1 && roundName[j] == playerInfo.PlayerName)
                {
                    Instantiate(Resources.Load<GameObject>("UI/RoundBorder"), fighterText[j].gameObject.transform);
                }

                fighterText[j].text = roundName[j];
            }
        }

        if (tournamentFile.roundNumber == 5)
        {
            transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            print("Winners are: \n " + tournamentFile.winners[0] + "\n" + tournamentFile.winners[1] + "\n" + tournamentFile.winners[2] + "\n" + tournamentFile.winners[3]);
        }
    }

    private bool CanFight()
    {
        if (tournamentFile.roundNumber == 5)
        {
            GiveOutPrizes();
            return false;
        } 

        string[] currentRound = tournamentFile.rounds[tournamentFile.roundNumber - 1].round;

        for (int i = 0; i < currentRound.Length; i++)
        {
            if (currentRound[i] == playerInfo.PlayerName)
            {
                return true;
            }
        }

        return false;
    }

    private void GiveOutPrizes()
    {
        for (int i = 0; i < tournamentFile.winners.Length; i++)
        {
            if (tournamentFile.winners[i] == playerInfo.PlayerName)
            {
                playerInfo.Money += tournamentFile.prizes[i].prizeMoney;
                playerInfo.UpdateEXP(tournamentFile.prizes[i].prizeEXP);
            }
        }
    }

    private void DeleteFile()
    {
        string path = Application.persistentDataPath + "/SaveGame/";
        string filePath = path + "/" + playerInfo.PlayerName + "/Tournament.json";

        if (!File.Exists(filePath))
        {
            return;
        }

        File.Delete(filePath);
    }

    // Start the enxt fight the player has
    private void NextFightOnClick()
    {
        string[] currentRound = tournamentFile.rounds[tournamentFile.roundNumber - 1].round;

        string opponentName = "";

        for (int i = 0; i < currentRound.Length; i++)
        {
            if (currentRound[i] == playerInfo.PlayerName)
            {
                if (i % 2 == 0)
                {
                    opponentName = currentRound[i + 1];
                }
                else
                {
                    opponentName = currentRound[i - 1];
                }
                break;
            }
        }

        for (int i = 0; i < tournamentFile.fightersTeams.Length; i++)
        {
            if (tournamentFile.fightersTeams[i].fightersName == opponentName)
            {
                GameObject spawnEnemiesObject = Instantiate(Resources.Load<GameObject>("SpawnEnemies/SpawnEnemies"));
                spawnEnemiesObject.GetComponent<SpawnEnemies>().Fighters = tournamentFile.fightersTeams[i].fighters;
                break;
            }
        }

        playerInfo.GameMode = "Tournament";
        SceneManager.LoadScene("BattleScene");
    }

    private void PrizeWinnersOnClick()
    {
        if (isShowingPrizeUI)
        {
            prizeUI.HideUI();
            isShowingPrizeUI = false;
            return;
        }

        isShowingPrizeUI = true;
        prizeUI.ShowUI(tournamentFile.prizes, tournamentFile.winners);
    }

    private void BackOnClick()
    {
        playerInfo.SaveGame();
        SceneManager.LoadScene("GameMenuScene");
    }
}
