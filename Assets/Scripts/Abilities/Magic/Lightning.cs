using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : Ability
{
    private float damage;
    protected float cooldownTime;
    public Lightning(string abilityName, float staminaCost, float damage, string animationName, float cooldownTime, bool canStop)
    : base(abilityName, staminaCost, animationName, canStop)
    {
        this.damage = damage;
        this.cooldownTime = cooldownTime;
    }

    public override void UseAbility(Animator animator, Transform enemyTransform, Transform handTransform)
    {
        if (isActive) return;
        isActive = true;
        animator.Play(animationName);
        cooldownTimer = cooldownTime;
    }

    protected override void ContinueAbility(Transform enemyTransform, Transform handTransform)
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer < 0) 
        {
            isActive = false;
            return;
        }

    }
}
