using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Ability
{
    private float damage;
    protected float cooldownTime;
    protected Sword sword;
    public Attack(AbilityManager ctx, string abilityName, float staminaCost, float damage, string animationName, float cooldownTime, bool canStop)
    : base(ctx, abilityName, staminaCost, animationName, canStop)
    {
        this.damage = damage;
        this.cooldownTime = cooldownTime;
        sword = GameObject.Find("Sword").GetComponent<Sword>();
    }

    public override void UseAbility()
    {
        if (isActive)
        {
            ContinueAbility();
            return;
        }

        isActive = true;
        ctx.Animator.Play(animationName);
        cooldownTimer = cooldownTime;
        sword.CanHit = true;
    }

    protected override void ContinueAbility()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer < 0)
        {
            isActive = false;
            sword.CanHit = false;
            sword.EnemiesHit.Clear();
            return;
        }
    }
}
