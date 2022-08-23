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
    private bool isAction1Input;
    private bool isAction2Input;
    private bool isAction3Input;
    private bool isAction4Input;

    public Vector2 MovePlayerInput { get => movePlayerInput; }
    public bool[] IsActionInputs { get => isActionInputs; }
    public bool IsAction1Input { get => isAction1Input; set => isAction1Input = value; }
    public bool IsAction2Input { get => isAction2Input; set => isAction2Input = value; }
    public bool IsAction3Input { get => isAction3Input; set => isAction3Input = value; }
    public bool IsAction4Input { get => isAction4Input; set => isAction4Input = value; }
    #endregion


    public PlayerBaseState CurrentState { get => currentState; set => currentState = value; }
    public CharacterController Controller { get => controller; }
    public AbilityManager AbilityManager { get => abilityManager; }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        abilityManager = GameObject.Find("AbilityManager").GetComponent<AbilityManager>();
        SetupControls();

        states = new PlayerStateFactory(this);
        currentState = states.CombatState();
        currentState.EnterState();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState();
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
