using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool isGamePaused;
    private bool isGameOver;
    private HUD hud;

    public bool IsGamePaused { get => isGamePaused; set => isGamePaused = value; }
    public bool IsGameOver { get => isGameOver; set => isGameOver = value; }
    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        hud = GameObject.FindObjectOfType<HUD>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void EndBattle(bool isPlayerWinner, int totalEXP)
    {
        isGameOver = true;
        hud.EndGame(isPlayerWinner);
    }
}
