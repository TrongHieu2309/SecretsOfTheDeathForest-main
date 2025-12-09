using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Enter Fall State");
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.Rb.linearVelocity = new Vector2(player.HorizontalInput * player.MoveSpeed, player.Rb.linearVelocity.y);

        player.HandleFlip(player.transform);

        if (player.onWall)
        {
            player.SwitchState(player.WallSlideState);
        }

        if (player.isGrounded)
        {
            player.SwitchState(player.IdleState);
        }
    }

    public override void ExitState(PlayerStateManager player)
    {
        Debug.Log("Exit Fall State");
    }
}