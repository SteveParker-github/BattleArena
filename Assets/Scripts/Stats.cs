using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    protected float baseHealth = 100;
    protected float incrementHealth = 10;
    protected int level;
    protected int strength;
    protected int intelligence;
    protected int endurance;
    protected float maxHealth;
    protected float health;
    public float Health { get => health; set => health = value; }
    public int Level { get => level; }
    public int Strength { get => strength; }
    public int Intelligence { get => intelligence; }
    public int Endurance { get => endurance; }

    public (float, float) GetHealth()
    {
        return (health, maxHealth);
    }
}
