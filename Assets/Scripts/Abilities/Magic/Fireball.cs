using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Ability
{
    private float damage;
    protected float cooldownTime;
    private bool isSentFireball;
    private GameObject fireball;
    public Fireball(string abilityName, float staminaCost, float damage, string animationName, float cooldownTime, bool canStop)
    : base(abilityName, staminaCost, animationName, canStop)
    {
        this.damage = damage;
        this.cooldownTime = cooldownTime;
        isSentFireball = false;
    }

    public override void UseAbility(Animator animator, Transform enemyTransform, Transform handTransform)
    {
        if (isActive)
        {
            ContinueAbility(enemyTransform, handTransform);
            return;
        }

        isActive = true;
        animator.Play(animationName);
        cooldownTimer = cooldownTime;
        isSentFireball = false;
    }

    protected override void ContinueAbility(Transform enemyTransform, Transform handTransform)
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer < 0)
        {
            isActive = false;
            return;
        }

        if (isSentFireball)
        {
            return;
        }

        if (cooldownTimer < 1.347f && cooldownTimer > 1.297f)
        {
            SendFireball(enemyTransform, handTransform);
        }
    }

    private void SendFireball(Transform enemyTransform, Transform handTransform)
    {
        isSentFireball = true;

        fireball = GameObject.Instantiate(Resources.Load<GameObject>("Cube"), handTransform.position, handTransform.rotation);
        fireball.GetComponent<FireballProjectile>().Setup(enemyTransform, damage);

    }
}
