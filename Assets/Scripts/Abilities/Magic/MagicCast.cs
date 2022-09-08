using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCast : Ability
{
    private float baseDamage;
    private float incrementDamage;
    private float damage;
    protected float cooldownTime;
    protected bool hasSentMagic;
    private float lowerBoundTime;
    private float UpperBoundTime;
    public MagicCast(
        AbilityManager ctx, 
        string abilityName, 
        float staminaCost, 
        float baseDamage, 
        float incrementDamage, 
        string animationName, 
        float cooldownTime, 
        bool canStop,
        float lowerBoundTime,
        float UpperBoundTime)
    : base(ctx, abilityName, staminaCost, animationName, canStop)
    {
        this.baseDamage = baseDamage;
        this.incrementDamage = incrementDamage;
        this.cooldownTime = cooldownTime;
        this.lowerBoundTime = lowerBoundTime;
        this.UpperBoundTime = UpperBoundTime;

        damage = baseDamage + ctx.Stats.Intelligence * incrementDamage;
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
        hasSentMagic = false;
        ctx.Weapon.SetActive(false);
    }

    protected override void ContinueAbility()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer < 0)
        {
            isActive = false;
            ctx.Weapon.SetActive(true);
            return;
        }

        if (hasSentMagic)
        {
            return;
        }

        if (cooldownTimer > lowerBoundTime && cooldownTimer < UpperBoundTime)
        {
            SendMagic();
        }
    }

    private void SendMagic()
    {
        hasSentMagic = true;

        GameObject projectile = GameObject.Instantiate(Resources.Load<GameObject>(abilityName + "Projectile"), ctx.HandTransform.position, ctx.HandTransform.rotation);
        projectile.GetComponent<Projectile>().Setup(ctx.EnemyTransform, damage, ctx.Target, ctx.GameManager);
    }
}
