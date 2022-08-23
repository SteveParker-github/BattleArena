using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    private Transform enemyTransform;
    private float damage = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyTransform == null) return;

        Vector3 targetPostion = Vector3.MoveTowards(transform.position, enemyTransform.position, 2.0f * Time.deltaTime);
        transform.position = targetPostion;
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Hit!");
        if (other.tag != "Enemy") return;

        other.GetComponent<EnemyController>().TakeDamage(damage);
        Destroy(this.gameObject);

    }

    public void Setup(Transform enemyTransform, float damage)
    {
        this.enemyTransform = enemyTransform;
        this.damage = damage;
    }
}
