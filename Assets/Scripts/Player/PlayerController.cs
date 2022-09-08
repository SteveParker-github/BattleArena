using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject handPosition;
    public GameObject HandPosition { get => handPosition; }
    private PlayerStateFactory states;
    private PlayerBaseState currentState;
    private CharacterController controller;
    private AbilityManager abilityManager;
    private EnemyManager enemyManager;
    private bool isPaused;
    private bool isBlocking;
    private GameManager gameManager;
    private PlayerStats playerStats;
    private HUD hud;
    public bool IsPaused { get => isPaused; set => isPaused = value; }
    public bool IsBlocking { get => isBlocking; set => isBlocking = value; }

    #region Animation
    private Animator animator;
    private readonly int movementX = Animator.StringToHash("MovementX");
    private readonly int movementY = Animator.StringToHash("MovementY");
    public Animator Animator { get => animator; }
    public int MovementX { get => movementX; }
    public int MovementY { get => movementY; }

    #endregion

    #region Controls
    private Controls controls;
    private Vector2 movePlayerInput;
    private bool[] isActionInputs;
    private bool isMenuInput;
    private bool isSwapPrevTargetInput;
    private bool isSwapNextTargetInput;
    private bool isAcceptInput;
    private bool isBackInput;

    public Controls Controls { get => controls; }
    public Vector2 MovePlayerInput { get => movePlayerInput; }
    public bool[] IsActionInputs { get => isActionInputs; }
    public bool IsMenuInput { get => isMenuInput; set => isMenuInput = value; }
    public bool IsSwapPrevTargetInput { get => isSwapPrevTargetInput; set => isSwapPrevTargetInput = value; }
    public bool IsSwapNextTargetInput { get => isSwapNextTargetInput; set => isSwapNextTargetInput = value; }
    public bool IsAcceptInput { get => isAcceptInput; set => isAcceptInput = value; }
    public bool IsBackInput { get => isBackInput; set => isBackInput = value; }
    #endregion


    public PlayerBaseState CurrentState { get => currentState; set => currentState = value; }
    public CharacterController Controller { get => controller; }
    public AbilityManager AbilityManager { get => abilityManager; }
    public EnemyManager EnemyManager { get => enemyManager; }
    public GameManager GameManager { get => gameManager; }
    public PlayerStats PlayerStats { get => playerStats; }
    public HUD HUD { get => hud; }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        abilityManager = GetComponent<AbilityManager>();
        abilityManager.Setup(handPosition.transform, animator, "Enemy");
        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerStats = GetComponent<PlayerStats>();
        hud = GameObject.FindObjectOfType<HUD>();
        playerStats.Setup();
        SetupControls();

        states = new PlayerStateFactory(this);
        currentState = states.CombatState();
        isPaused = false;
        gameManager.IsGamePaused = false;
        isBlocking = false;
    }

    private void Start()
    {
        currentState.EnterState();
        hud.UpdateHealth(playerStats.GetHealth());
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState();
    }

    public void TakeDamage(float damage)
    {
        float newDamage = isBlocking ? damage * 0.2f : damage;

        float healthReduction = Mathf.Min(playerStats.Health, newDamage);

        playerStats.Health -= healthReduction;
        playerStats.Health = Mathf.RoundToInt(playerStats.Health);
        hud.UpdateHealth(playerStats.GetHealth());

        if (playerStats.Health <= 0)
        {
            print("Player is dead!");
            gameManager.EndBattle(false, 0);
            animator.SetFloat(movementX, 0);
            animator.SetFloat(movementY, 0);
        }
    }

    #region setupcontrols
    private void SetupControls()
    {
        controls = new Controls();
        isActionInputs = new bool[4];

        controls.Combat.MovePlayer.started += OnMovePlayerInput;
        controls.Combat.MovePlayer.performed += OnMovePlayerInput;
        controls.Combat.MovePlayer.canceled += OnMovePlayerInput;
        controls.Combat.Action1.started += OnAction1Input;
        controls.Combat.Action1.canceled += OnAction1Input;
        controls.Combat.Action2.started += OnAction2Input;
        controls.Combat.Action2.canceled += OnAction2Input;
        controls.Combat.Action3.started += OnAction3Input;
        controls.Combat.Action3.canceled += OnAction3Input;
        controls.Combat.Action4.started += OnAction4Input;
        controls.Combat.Action4.canceled += OnAction4Input;
        controls.Combat.Menu.started += OnMenuInput;
        controls.Combat.Menu.canceled += OnMenuInput;
        controls.Combat.SwapPrevTarget.started += OnSwapPrevTargetInput;
        controls.Combat.SwapPrevTarget.canceled += OnSwapPrevTargetInput;
        controls.Combat.SwapNextTarget.started += OnSwapNextTargetInput;
        controls.Combat.SwapNextTarget.canceled += OnSwapNextTargetInput;

        controls.Menu.Accept.started += OnAcceptInput;
        controls.Menu.Accept.canceled += OnAcceptInput;
        controls.Menu.Back.started += OnBackInput;
        controls.Menu.Back.canceled += OnBackInput;
    }

    private void OnMovePlayerInput(InputAction.CallbackContext context)
    {
        movePlayerInput = context.ReadValue<Vector2>();
    }

    private void OnAction1Input(InputAction.CallbackContext context)
    {
        isActionInputs[0] = context.ReadValueAsButton();
    }

    private void OnAction2Input(InputAction.CallbackContext context)
    {
        isActionInputs[1] = context.ReadValueAsButton();
    }

    private void OnAction3Input(InputAction.CallbackContext context)
    {
        isActionInputs[2] = context.ReadValueAsButton();
    }

    private void OnAction4Input(InputAction.CallbackContext context)
    {
        isActionInputs[3] = context.ReadValueAsButton();
    }

    private void OnMenuInput(InputAction.CallbackContext context)
    {
        isMenuInput = context.ReadValueAsButton();
    }

    private void OnSwapPrevTargetInput(InputAction.CallbackContext context)
    {
        isSwapPrevTargetInput = context.ReadValueAsButton();
    }

    private void OnSwapNextTargetInput(InputAction.CallbackContext context)
    {
        isSwapNextTargetInput = context.ReadValueAsButton();
    }

    private void OnAcceptInput(InputAction.CallbackContext context)
    {
        isAcceptInput = context.ReadValueAsButton();
    }

    private void OnBackInput(InputAction.CallbackContext context)
    {
        isBackInput = context.ReadValueAsButton();
    }

    private void OnEnable()
    {
        controls.Combat.Enable();
    }

    private void OnDisable()
    {
        controls.Combat.Disable();
    }
    #endregion
}
