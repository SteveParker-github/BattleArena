using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    private string enemyType;
    private int numberOfEnemies;
    private int levelOffset;
    private Fighter[] fighters;

    public string EnemyType { get => enemyType; }
    public int NumberOfEnemies { get => numberOfEnemies; }
    public int LevelOffset { get => levelOffset; }
    public Fighter[] Fighters { get => fighters; set => fighters = value; }

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void Setup(string enemyType, int numberOfEnemies, int maxLevelCap)
    {
        this.enemyType = enemyType;
        this.numberOfEnemies = numberOfEnemies;
        this.levelOffset = maxLevelCap;
    }

    public void RandomEnemies(int playerLevel, string enemyType)
    {
        int numberOfEnemies = Random.Range(1, 4);
        int levelOffset = Mathf.CeilToInt(9 / numberOfEnemies);
        fighters = new Fighter[numberOfEnemies];

        for (int i = 0; i < fighters.Length; i++)
        {
            Fighter fighter = new Fighter();
            fighters[i] = fighter;
            fighters[i].enemyType = enemyType;
            fighters[i].level = Random.Range((-3 + levelOffset), (1 + levelOffset));
        }
    }
}
