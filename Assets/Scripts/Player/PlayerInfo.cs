using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    private int level;
    private int strength;
    private int intelligence;
    private int endurance;
    private int currentEXP;

    public int Level { get => level; }
    public int Strength { get => strength; }
    public int Intelligence { get => intelligence; }
    public int Endurance { get => endurance; }
    public int CurrentEXP { get => currentEXP; }

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        strength = 8;
        intelligence = 8;
        endurance = 8;
        currentEXP = 0;
        GameObject.DontDestroyOnLoad(this.gameObject);
    }
}
