using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private PlayerController player;
    private Transform playerTransform;
    private List<GameObject> enemies;
    private GameManager gameManager;
    private int totalEXP;
    public GameManager GameManager { get => gameManager; }
    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        GameObject playerObject = GameObject.Find("Player");
        player = playerObject.GetComponent<PlayerController>();
        playerTransform = playerObject.transform.Find("Target");

        enemies = new List<GameObject>();

        for (int i = 0; i < transform.childCount; i++)
        {
            enemies.Add(transform.GetChild(i).gameObject);
            enemies[i].GetComponent<EnemyStats>().Setup(player.PlayerStats.Level);
        }

        totalEXP = 0;
    }

    public Transform GetPlayerTransform()
    {
        return playerTransform;
    }

    public Transform GetEnemyTransform(int index)
    {
        return enemies[index].transform.Find("Target");
    }

    public int GetNewEnemyIndex(int currentIndex, int indexShift)
    {
        return (currentIndex + indexShift + enemies.Count) % enemies.Count;

    }

    public bool IsEnemiesAlive()
    {
        if (enemies.Count > 0) return true;

        gameManager.EndBattle(true, totalEXP);
        return false;
    }

    public void RemoveChild(int index)
    {
        totalEXP += enemies[index].GetComponent<EnemyStats>().ExperiencePoints;
        enemies.RemoveAt(index);
    }
}
