using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private GameObject weapon;
    private Dictionary<string, Ability> abilities;
    private Transform enemyTransform;
    private Transform handTransform;
    private Animator animator;
    private string target;
    private GameManager gameManager;
    private Stats stats;

    public Dictionary<string, Ability> Abilities { get => abilities; }
    public Transform EnemyTransform { get => enemyTransform; }
    public Transform HandTransform { get => handTransform; }
    public Animator Animator { get => animator; }
    public string Target { get => target; }
    public GameObject Weapon { get => weapon; }
    public GameManager GameManager { get => gameManager; }
    public Stats Stats { get => stats; }
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        stats = GetComponent<Stats>();
        abilities = new Dictionary<string, Ability>();
        abilities.Add("Attack", new Attack(this, "Attack", 15, 30, "Attack", 2.033f, false));
        abilities.Add("Block", new Block(this, "Block", 10, 30, "Block", 1f, true));
        abilities.Add("Fireball", new MagicCast(this, "Fireball", 25, 20, 2, "Fireball", 3.367f, false, 1.3f, 1.35f));
        abilities.Add("Lightning", new MagicCast(this, "Lightning", 20, 10, 3, "Lightning", 2.667f, false, 1.8f, 1.9f));
    }

    public void Setup(Transform newHandTransform, Animator newAnimator, string newTarget)
    {
        handTransform = newHandTransform;
        animator = newAnimator;
        target = newTarget;
    }

    public void NewTarget(Transform newEnemyTransform)
    {
        enemyTransform = newEnemyTransform;
    }
}
