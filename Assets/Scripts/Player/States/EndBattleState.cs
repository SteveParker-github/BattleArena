using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class EndBattleState : PlayerBaseState
{
    public EndBattleState(PlayerController playerController, PlayerStateFactory playerStateFactory)
    : base(playerController, playerStateFactory)
    { }
    private string gameMode;
    private TournamentFile tournamentFile;
    private PlayerInfo playerInfo;
    public override void EnterState()
    {
        playerInfo = GameObject.FindObjectOfType<PlayerInfo>();
        gameMode = playerInfo.GameMode;
        ctx.Controls.Combat.Disable();
        ctx.Controls.Menu.Enable();
        ctx.IsAcceptInput = false;
    }
    public override void UpdateState()
    {
        CheckSwitchState();
    }
    public override void ExitState()
    {
        ctx.Controls.Combat.Enable();
        ctx.Controls.Menu.Disable();
    }
    public override void CheckSwitchState()
    {
        if (ctx.IsAcceptInput)
        {
            switch (gameMode)
            {
                case "Quick Game":
                    SceneManager.LoadScene("GameMenuScene");
                    break;
                case "Tournament":
                    UpdateTournament();
                    SceneManager.LoadScene("TournamentScene");
                    break;
                default:
                    break;
            }
            return;
        }
    }

    private void UpdateTournament()
    {
        GetTournamentFile();
        int roundNumber = tournamentFile.roundNumber - 1;
        string[] currentRound = tournamentFile.rounds[roundNumber].round;
        List<string> winners = new List<string>();
        List<string> losers = new List<string>();

        for (int i = 0; i < currentRound.Length; i += 2)
        {
            if (currentRound[i] == playerInfo.PlayerName || currentRound[i + 1] == playerInfo.PlayerName)
            {
                int winner = i;
                int loser = i + 1;

                bool isPlayerSecondAndWon = currentRound[i + 1] == playerInfo.PlayerName && ctx.GameManager.IsPlayerWinner;
                bool isPlayerFirstAndLost = currentRound[i] == playerInfo.PlayerName && !ctx.GameManager.IsPlayerWinner;

                if (isPlayerSecondAndWon || isPlayerFirstAndLost)
                {
                    winner++;
                    loser--;
                }

                winners.Add(currentRound[winner]);
                losers.Add(currentRound[loser]);

            }
            else
            {
                int winner = i;
                int loser = i + 1;

                if (Random.Range(0, 2) == 0)
                {
                    winner++;
                    loser--;
                }

                winners.Add(currentRound[winner]);
                losers.Add(currentRound[loser]);
            }
        }

        tournamentFile.roundNumber++;
        roundNumber++;

        switch (roundNumber)
        {
            case 3:
                tournamentFile.rounds[roundNumber].round[0] = winners[0];
                tournamentFile.rounds[roundNumber].round[1] = winners[1];
                tournamentFile.rounds[roundNumber].round[2] = losers[0];
                tournamentFile.rounds[roundNumber].round[3] = losers[1];
                break;

            case 4:
                string[] results = new string[] {
                    winners[0],
                    losers[0],
                    winners[1],
                    losers[1]};
                tournamentFile.winners = results;
                break;

            default:
                tournamentFile.rounds[roundNumber].round = winners.ToArray();
                break;
        }

        SaveTournamentFile();
    }

    private void GetTournamentFile()
    {
        string path = Application.persistentDataPath + "/SaveGame/";
        string filePath = path + "/" + playerInfo.PlayerName + "/Tournament.json";

        if (!File.Exists(filePath))
        {
            return;
        }

        string fileText = File.ReadAllText(filePath);
        tournamentFile = JsonUtility.FromJson<TournamentFile>(fileText);
    }

    private void SaveTournamentFile()
    {
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
}
