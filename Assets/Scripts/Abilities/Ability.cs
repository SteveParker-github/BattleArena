using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{
    protected string abilityName;
    protected float staminaCost;
    protected string animationName;
    protected bool canStop;
    protected bool isActive;
    protected float cooldownTimer;

    public float StaminaCost { get => staminaCost; }
    public bool IsActive { get => isActive; }

    public Ability(string abilityName, float staminaCost, string animationName, bool canStop)
    {
        this.abilityName = abilityName;
        this.staminaCost = staminaCost;
        this.animationName = animationName;
        this.canStop = canStop;
        isActive = false;
    }

    public abstract void UseAbility(Animator animator, Transform enemyTransform, Transform handTransform);

    public void StopAbility(Animator animator)
    {
        if (isActive)
        {
            if (canStop)
            {
                isActive = false;
                animator.SetTrigger("EscapeAction");
                return;
            }
        }
    }

    protected abstract void ContinueAbility(Transform EnemyTransform, Transform handTransform);
}
