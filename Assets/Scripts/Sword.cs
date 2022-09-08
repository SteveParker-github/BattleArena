using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private bool canHit;
    private float damage;
    private List<string> enemiesHit;
    public bool CanHit { get => canHit; set => canHit = value; }
    public List<string> EnemiesHit { get => enemiesHit; }

    // Start is called before the first frame update
    void Start()
    {
        damage = 30.0f;
        enemiesHit = new List<string>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canHit) return;

        if (other.tag != "Enemy") return;

        if (enemiesHit.Contains(other.name)) return;

        other.GetComponent<EnemyController>().TakeDamage(damage);
        enemiesHit.Add(other.name);
    }
}
