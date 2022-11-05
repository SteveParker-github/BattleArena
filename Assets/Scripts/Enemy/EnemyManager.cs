using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyPrefabs;
    private int[] xLocations = new int[] { 0, -5, 5 };
    private PlayerController player;
    private Transform playerTransform;
    private List<GameObject> enemies;
    private GameManager gameManager;
    private int totalEXP;
    private SpawnEnemies spawnEnemies;
    private bool gameEnded;
    public GameManager GameManager { get => gameManager; }

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        GameObject playerObject = GameObject.Find("Player");
        player = playerObject.GetComponent<PlayerController>();
        playerTransform = playerObject.transform.Find("Target");
        spawnEnemies = GameObject.FindObjectOfType<SpawnEnemies>();
        enemies = new List<GameObject>();

        CreateEnemies();

        totalEXP = 0;
        gameEnded = false;
    }

    private void CreateEnemies()
    {
        for (int i = 0; i < spawnEnemies.Fighters.Length; i++)
        {
            enemies.Add(Instantiate(Resources.Load<GameObject>("Enemies/" + spawnEnemies.Fighters[i].enemyType), transform));
            enemies[i].GetComponent<EnemyStats>().Setup(spawnEnemies.Fighters[i].level);
            Vector3 position = enemies[i].transform.position;
            position.x += xLocations[i];
            enemies[i].transform.position = position;
        }
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

        if (!gameEnded)
        {
            gameManager.EndBattle(true, totalEXP);
            gameEnded = true;
        }

        return false;
    }

    public void RemoveChild(int index)
    {
        totalEXP += enemies[index].GetComponent<EnemyStats>().ExperiencePoints;
        enemies.RemoveAt(index);
    }
}
