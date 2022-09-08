using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{
    protected AbilityManager ctx;
    protected string abilityName;
    protected float staminaCost;
    protected string animationName;
    protected bool canStop;
    protected bool isActive;
    protected float cooldownTimer;

    public float StaminaCost { get => staminaCost; }
    public bool IsActive { get => isActive; }

    public Ability(AbilityManager ctx, string abilityName, float staminaCost, string animationName, bool canStop)
    {
        this.ctx = ctx;
        this.abilityName = abilityName;
        this.staminaCost = staminaCost;
        this.animationName = animationName;
        this.canStop = canStop;
        isActive = false;
    }

    public abstract void UseAbility();

    public virtual void StopAbility()
    {
        if (isActive)
        {
            ContinueAbility();
        }
    }

    protected abstract void ContinueAbility();
}
