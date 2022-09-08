using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Ability
{
    private float damageReduction;
    private float extraStaminaCost;
    private PlayerController playerController;

    public Block(AbilityManager ctx, string abilityName, float staminaCost, float damageReduction, string animationName, float extraStaminaCost, bool canStop)
    : base(ctx, abilityName, staminaCost, animationName, canStop)
    {
        this.damageReduction = damageReduction;
        this.extraStaminaCost = extraStaminaCost;
        playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    public override void UseAbility()
    {
        if (isActive) return;
        isActive = true;
        playerController.IsBlocking = true;
        ctx.Animator.Play(animationName);
    }

    public override void StopAbility()
    {
        if (!isActive) return;

        if (canStop)
        {
            playerController.IsBlocking = false;
            isActive = false;
            ctx.Animator.SetTrigger("EscapeAction");
            return;
        }
    }

    protected override void ContinueAbility()
    { }
}
