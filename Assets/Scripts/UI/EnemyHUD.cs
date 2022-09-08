using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHUD : MonoBehaviour
{
    private EnemyStats enemyStats;
    private Image healthImage;
    private Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {  
        enemyStats = transform.parent.GetComponent<EnemyStats>();
        healthImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        cameraTransform = GameObject.Find("Player Camera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        (float, float) health = enemyStats.GetHealth();
        healthImage.fillAmount = health.Item1 / health.Item2;
        transform.LookAt(cameraTransform);
    }
}
