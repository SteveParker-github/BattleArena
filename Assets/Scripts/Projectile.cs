using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform enemyTransform;
    private float damage = 0;
    private string target;
    private GameManager gameManager;

    // Update is called once per frame
    void Update()
    {
        if (gameManager.IsGamePaused) return;

        if (enemyTransform == null) Destroy(gameObject);

        Vector3 targetPostion = Vector3.MoveTowards(transform.position, enemyTransform.position, 5.0f * Time.deltaTime);
        transform.position = targetPostion;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != target) return;

        if (target == "Enemy")
        {
            other.GetComponent<EnemyController>().TakeDamage(damage);
        }

        if (target == "Player")
        {
            other.GetComponent<PlayerController>().TakeDamage(damage);
        }

        Destroy(this.gameObject);
    }

    public void Setup(Transform enemyTransform, float damage, string target, GameManager gameManager)
    {
        this.enemyTransform = enemyTransform;
        this.damage = damage;
        this.target = target;
        this.gameManager = gameManager;
    }
}
