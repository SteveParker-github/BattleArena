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
    private bool isActionActive;
    private string[] actions;
    private Dictionary<string, float> animationTime;

    public override void EnterState()
    {
        enemyTransform = GameObject.Find("Enemy").transform;
        isActionActive = false;
        actions = new string[] { "Fireball", "Lightning", "Attack", "Block" };
        animationTime = new Dictionary<string, float>();
        AnimationClip[] clips = ctx.Animator.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in clips)
        {
            animationTime.Add(clip.name, clip.length);
        }
    }
    public override void UpdateState()
    {
        if (enemyTransform == null)
        {
            GameObject enemy = GameObject.Find("Enemy");

            if (enemy == null)
            {
                Debug.Log("All enemies are dead");
                return;
            }

            enemyTransform = enemy.transform;
        }

        ActionButtons();

        if (!isActionActive)
        {
            MovePlayer();
            RotatePlayer();
        }

        CheckSwitchState();
    }
    public override void ExitState()
    {
    }
    public override void CheckSwitchState()
    {
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
        Vector3 target = enemyTransform.position;
        target.y = ctx.transform.position.y;

        ctx.transform.LookAt(target);
    }

    private void ActionButtons()
    {
        int actionOption = -1;

        // Find the first button that has been pressed
        for (int i = 0; i < ctx.IsActionInputs.Length; i++)
        {
            if (ctx.IsActionInputs[i])
            {
                actionOption = i;
                break;
            }
        }

        List<int> isActives = new List<int>();

        // Find Which abilities are currently active.
        // Stop any abilities that are not currently being pressed.
        // NOTE: only abilities that need a constant press can be stopped. i.e. block

        for (int i = 0; i < ctx.IsActionInputs.Length; i++)
        {
            if (ctx.AbilityManager.Abilities[actions[i]].IsActive)
            {
                isActives.Add(i);
            }

            if (i == actionOption) continue;

            ctx.AbilityManager.Abilities[actions[i]].StopAbility(ctx.Animator);
        }

        isActionActive = isActives.Count > 0 ? true : false; //Toggle the isActionActive based on how many items are in the list.

        // If no button was pressed skip the rest of the method.
        if (actionOption == -1 && !isActionActive)
        {
            return;
        }

        // If the item in the isActives is not the same as the action button, return.
        foreach (int item in isActives)
        {
            if (item != actionOption && actionOption != -1) return;
        }

        ctx.Animator.SetFloat(ctx.MovementX, 0);
        ctx.Animator.SetFloat(ctx.MovementY, 0);

        int actionSelect = actionOption == -1 ? isActives[0] : actionOption;

        ctx.AbilityManager.Abilities[actions[actionSelect]].UseAbility(ctx.Animator, enemyTransform, ctx.HandPosition.transform);
    }
}
