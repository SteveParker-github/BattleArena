using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform handTransform;
    [SerializeField] private float preferedMinDistance = 5.0f;
    [SerializeField] private float preferedMaxDistance = 10.0f;
    private EnemyStats enemyStats;
    private EnemyManager enemyManager;
    private Transform playerTransform;
    private AbilityManager abilityManager;
    private Animator animator;
    private bool isActionActive;
    private int actionOption;
    private string[] actions;
    private float abilityCooldown;
    private readonly int movementY = Animator.StringToHash("MovementY");
    private readonly int movementX = Animator.StringToHash("MovementX");
    private bool isMoving;
    private float moveCooldown;
    private Vector3 targetLocation;
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        enemyManager = transform.parent.GetComponent<EnemyManager>();
        playerTransform = enemyManager.GetPlayerTransform();
        abilityManager = GetComponent<AbilityManager>();
        animator = GetComponent<Animator>();
        abilityManager.Setup(handTransform, animator, "Player");
        abilityManager.NewTarget(playerTransform);

        isActionActive = false;
        actions = new string[] { "Fireball", "Lightning" };
        isMoving = false;
        agent = GetComponent<NavMeshAgent>();
        abilityCooldown = 3.0f;
        moveCooldown = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyManager.GameManager.IsGameOver)
        {
            animator.SetFloat(movementX, 0);
            animator.SetFloat(movementY, 0);
            return;
        }
        Vector3 velocity = transform.InverseTransformVector(agent.velocity).normalized;
        animator.SetFloat(movementY, velocity.z);
        animator.SetFloat(movementX, velocity.x);

        if (enemyManager.GameManager.IsGamePaused)
        {
            animator.StartPlayback();
            return;
        }

        animator.StopPlayback();

        if (isActionActive && !isMoving)
        {
            abilityManager.Abilities[actions[actionOption]].UseAbility();
            isActionActive = abilityManager.Abilities[actions[actionOption]].IsActive;
            return;
        }

        RotateEnemy();

        MoveEnemy();

        if (isMoving) return;

        if (abilityCooldown > 0.0f)
        {
            abilityCooldown -= Time.deltaTime;
            return;
        }

        actionOption = Random.Range(0, actions.Length);
        abilityManager.Abilities[actions[actionOption]].UseAbility();
        isActionActive = true;
        abilityCooldown = Random.Range(3.0f, 10.0f);
    }

    private void RotateEnemy()
    {
        Vector3 target = playerTransform.position;
        target.y = transform.position.y;

        transform.LookAt(target);
    }

    private void MoveEnemy()
    {
        if (moveCooldown > 0)
        {
            moveCooldown -= Time.deltaTime;
            return;
        }

        if (targetLocation == Vector3.zero)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer > preferedMinDistance && distanceToPlayer < preferedMaxDistance)
            {
                moveCooldown = Random.Range(2.0f, 5.0f);
                return;
            }

            float targetDistance = ((preferedMaxDistance - preferedMinDistance) / 2 + preferedMinDistance) - distanceToPlayer;
            targetLocation = transform.position + transform.forward * -1 * targetDistance;

            agent.SetDestination(targetLocation);
            isMoving = true;
            return;
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            targetLocation = Vector3.zero;
            moveCooldown = Random.Range(2.0f, 5.0f);
            isMoving = false;
            return;
        }
    }

    public void TakeDamage(float damage)
    {

        enemyStats.Health -= damage;
        if (enemyStats.Health <= 0)
        {
            enemyManager.RemoveChild(transform.GetSiblingIndex());
            Destroy(this.gameObject);
        }
    }
}
