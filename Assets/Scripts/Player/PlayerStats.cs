using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Stats
{
    private int currentEXP;
    private PlayerInfo playerInfo;
    public void Setup()
    {
        playerInfo = GameObject.FindObjectOfType<PlayerInfo>();
        level = playerInfo.Level;
        strength = playerInfo.Strength;
        intelligence = playerInfo.Intelligence;
        endurance = playerInfo.Endurance;
        currentEXP = playerInfo.CurrentEXP;

        maxHealth = baseHealth + (level + endurance) * incrementHealth;
        health = maxHealth;

        Debug.Log(playerInfo.PlayerName);
    }
}
