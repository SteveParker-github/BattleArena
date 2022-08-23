using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Ability
{
    private float damageReduction;
    private float extraStaminaCost;

    public Block(string abilityName, float staminaCost, float damageReduction, string animationName, float extraStaminaCost, bool canStop)
    : base(abilityName, staminaCost, animationName, canStop)
    {
        this.damageReduction = damageReduction;
        this.extraStaminaCost = extraStaminaCost;

    }

    public override void UseAbility(Animator animator, Transform enemyTransform, Transform handTransform)
    {
        if (isActive) return;
        isActive = true;
        animator.Play(animationName);
    }

    protected override void ContinueAbility(Transform enemyTransform, Transform handTransform)
    { }
}
