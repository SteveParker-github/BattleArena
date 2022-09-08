using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : Stats
{
    [SerializeField] private int baseExperience = 10;
    [SerializeField] private int incrementExperience = 1;
    [SerializeField] private int baseStrength = 4;
    [SerializeField] private int incrementStrength = 1;
    [SerializeField] private int baseIntelligence = 8;
    [SerializeField] private int incrementIntelligence = 3;
    [SerializeField] private int baseEndurance = 3;
    [SerializeField] private int incrementEndurance = 2;

    private int experiencePoints;
    public int ExperiencePoints { get => experiencePoints; }

    public void Setup(int playerLevel)
    {
        level = playerLevel + Random.Range(0, 4);

        strength = baseStrength + level * incrementStrength;
        intelligence = baseIntelligence + level + incrementIntelligence;
        endurance = baseEndurance + level * incrementEndurance;

        maxHealth = baseHealth + (level + endurance) * incrementHealth;
        health = maxHealth;

        experiencePoints = baseExperience + incrementExperience * level;
    }
}
