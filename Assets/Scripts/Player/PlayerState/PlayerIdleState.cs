using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.Anim.SetFloat("xVelocity", 0);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.Rb.linearVelocity = Vector2.zero;
        if (player.HorizontalInput != 0)
        {
            player.SwitchState(player.MoveState);
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.isGrounded)
        {
            player.SwitchState(player.JumpState);
        }

        if (Input.GetMouseButtonDown(0) && player.isGrounded)
        {
            player.SwitchState(player.AttackState);
            player.currentCombo = 1;
        }

        if (Input.GetMouseButtonDown(1) && (player.blockTimer >= player.blockCoolDown))
        {
            player.SwitchState(player.BlockState);
            player.isBlocking = true;
        }

        // if (!player.isGrounded && player.isWallSliding)
        // {
        //     player.SwitchState(player.WallJumpState);
        // }
    }

    public override void ExitState(PlayerStateManager player)
    {
        Debug.Log("Exit Idle State");
    }
}