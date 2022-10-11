using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndBattleState : PlayerBaseState
{
    public EndBattleState(PlayerController playerController, PlayerStateFactory playerStateFactory)
    : base(playerController, playerStateFactory)
    { }

    public override void EnterState()
    {
        ctx.Controls.Combat.Disable();
        ctx.Controls.Menu.Enable();
        ctx.IsAcceptInput = false;
    }
    public override void UpdateState()
    {
        if (ctx.IsAcceptInput)
        {
            SceneManager.LoadScene("GameMenuScene");
            return;
        }

        CheckSwitchState();
    }
    public override void ExitState()
    {
        ctx.Controls.Combat.Enable();
        ctx.Controls.Menu.Disable();
    }
    public override void CheckSwitchState()
    {
    }
}
