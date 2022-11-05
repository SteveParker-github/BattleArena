using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private PlayerInfo playerInfo;
    private bool isGamePaused;
    private bool isGameOver;
    private HUD hud;
    private bool isPlayerWinner;

    public bool IsGamePaused { get => isGamePaused; set => isGamePaused = value; }
    public bool IsGameOver { get => isGameOver; set => isGameOver = value; }
    public bool IsPlayerWinner { get => isPlayerWinner; }

    // Start is called before the first frame update
    void Start()
    {
        playerInfo = GameObject.FindObjectOfType<PlayerInfo>();
        isGameOver = false;
        hud = GameObject.FindObjectOfType<HUD>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void EndBattle(bool isPlayerWinner, int totalEXP)
    {
        playerInfo.UpdateEXP(totalEXP);
        this.isPlayerWinner = isPlayerWinner;
        
        if (isPlayerWinner)
        {
            playerInfo.Money += 50;
        }

        isGameOver = true;
        hud.EndGame(isPlayerWinner);
    }
}
