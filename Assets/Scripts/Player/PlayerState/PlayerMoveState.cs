using NUnit.Framework.Interfaces;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Enter Move State");
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.Anim.SetFloat("xVelocity", Mathf.Abs(player.HorizontalInput));
        
        if (player.isGrounded)
        {
            player.Rb.linearVelocity = new Vector2(player.HorizontalInput * player.MoveSpeed, 0);
        }
        else
        {
            player.Rb.linearVelocity = new Vector2(player.HorizontalInput * player.MoveSpeed, player.Rb.linearVelocity.y);
        }

        if (player.Rb.linearVelocity.y < 0f) player.SwitchState(player.FallState);

        if (player.HorizontalInput == 0)
        {
            player.SwitchState(player.IdleState);
        }
        
        player.HandleFlip(player.transform);

        if (player.Rb.linearVelocity.y < 0) player.SwitchState(player.FallState);

        if (Input.GetKeyDown(KeyCode.Space) && player.isGrounded)
        {
            player.SwitchState(player.JumpState);
        }

        if (Input.GetMouseButtonDown(1) && (player.blockTimer >= player.blockCoolDown))
        {
            player.SwitchState(player.BlockState);
            player.isBlocking = true;
        }

        if (Input.GetMouseButtonDown(0) && player.isGrounded) player.SwitchState(player.AttackState);
    }

    public override void ExitState(PlayerStateManager player)
    {
        Debug.Log("Exit Move State");
    }
}