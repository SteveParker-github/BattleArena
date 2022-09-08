using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : PlayerBaseState
{
    public CombatState(PlayerController playerController, PlayerStateFactory playerStateFactory)
    : base(playerController, playerStateFactory)
    { }

    private const float ROTATIONSPEED = 5.0f;
    private const float WALKSPEED = 2.0f;
    private const float DASHSPEED = 10.0f;

    private Transform enemyTransform;
    private int currentEnemy;
    private bool isActionActive;
    private int actionOption;
    private string[] actions;

    public override void EnterState()
    {
        currentEnemy = 0;
        actions = new string[] { "Fireball", "Lightning", "Attack", "Block" };
        ctx.HUD.UpdateNames(actions);
        ctx.IsMenuInput = false;
    }
    public override void UpdateState()
    {
        CheckSwitchState();
        SwapTarget();

        if (ctx.IsMenuInput)
        {
            ctx.IsPaused = !ctx.IsPaused;
            ctx.GameManager.IsGamePaused = ctx.IsPaused;
            ctx.IsMenuInput = false;
        }

        if (ctx.IsPaused)
        {
            ctx.Animator.StartPlayback();
            return;
        }

        ctx.Animator.StopPlayback();

        if (enemyTransform == null)
        {
            if (!ctx.EnemyManager.IsEnemiesAlive()) return;
            
            currentEnemy = ctx.EnemyManager.GetNewEnemyIndex(currentEnemy, 0);
            enemyTransform = ctx.EnemyManager.GetEnemyTransform(currentEnemy);
            ctx.AbilityManager.NewTarget(enemyTransform);
        }

        ActionButtons();
        RotatePlayer();

        if (!isActionActive)
        {
            MovePlayer();
        }
    }
    public override void ExitState()
    {
    }
    public override void CheckSwitchState()
    {
        if (ctx.GameManager.IsGameOver)
        {
            ctx.Animator.SetFloat(ctx.MovementX, 0);
            ctx.Animator.SetFloat(ctx.MovementY, 0);
            SwitchState(factory.EndBattleState());
        }
    }

    private void MovePlayer()
    {
        ctx.Animator.SetFloat(ctx.MovementX, ctx.MovePlayerInput.x);
        ctx.Animator.SetFloat(ctx.MovementY, ctx.MovePlayerInput.y);

        if (ctx.MovePlayerInput == Vector2.zero) return;

        float moveSpeed = WALKSPEED * ctx.MovePlayerInput.magnitude;

        Vector3 moveDirection = new Vector3(ctx.MovePlayerInput.x, 0.0f, ctx.MovePlayerInput.y).normalized;

        moveDirection = ctx.transform.right * ctx.MovePlayerInput.x + ctx.transform.forward * ctx.MovePlayerInput.y;

        ctx.Controller.Move(moveDirection.normalized * (Time.deltaTime * moveSpeed));
    }

    private void RotatePlayer()
    {
        Vector3 target = (enemyTransform.position - ctx.transform.position).normalized;
        target.y = ctx.transform.position.y;

        Quaternion targetRotation = Quaternion.LookRotation(target);
        ctx.transform.rotation = Quaternion.Lerp(ctx.transform.rotation, targetRotation, Time.deltaTime);
    }

    private void ActionButtons()
    {
        if (isActionActive)
        {
            if (ctx.IsActionInputs[actionOption])
            {
                ctx.AbilityManager.Abilities[actions[actionOption]].UseAbility();
            }
            else
            {
                ctx.AbilityManager.Abilities[actions[actionOption]].StopAbility();
            }

            isActionActive = ctx.AbilityManager.Abilities[actions[actionOption]].IsActive;

            if (!isActionActive) ctx.HUD.StopHighlight(actionOption);

            return;
        }

        actionOption = Array.IndexOf(ctx.IsActionInputs, true);

        // If no button was pressed skip the rest of the method.
        if (actionOption == -1) return;

        ctx.Animator.SetFloat(ctx.MovementX, 0);
        ctx.Animator.SetFloat(ctx.MovementY, 0);

        ctx.AbilityManager.Abilities[actions[actionOption]].UseAbility();
        isActionActive = true;
        ctx.HUD.StartHighlight(actionOption);
    }

    private void SwapTarget()
    {
        if (!ctx.IsSwapPrevTargetInput && !ctx.IsSwapNextTargetInput) return;

        int indexShift = 1;
        ctx.IsSwapNextTargetInput = false;

        if (ctx.IsSwapPrevTargetInput)
        {
            indexShift = -1;
            ctx.IsSwapPrevTargetInput = false;
        }

        currentEnemy = ctx.EnemyManager.GetNewEnemyIndex(currentEnemy, indexShift);
        enemyTransform = ctx.EnemyManager.GetEnemyTransform(currentEnemy);
        ctx.AbilityManager.NewTarget(enemyTransform);
    }
}
