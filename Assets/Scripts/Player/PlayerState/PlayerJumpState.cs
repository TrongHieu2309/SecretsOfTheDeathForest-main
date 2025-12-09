using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Enter Jump State");
        player.Rb.AddForce(Vector2.up * player.jumpForce, ForceMode2D.Impulse);
        player.JumpSound();
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.Rb.linearVelocity = new Vector2(player.HorizontalInput * player.MoveSpeed, player.Rb.linearVelocity.y);

        player.HandleFlip(player.transform);

        if (player.Rb.linearVelocity.y < 0)
        {
            player.SwitchState(player.FallState);
        }
    }

    public override void ExitState(PlayerStateManager player)
    {
        Debug.Log("Exit Jump State");
    }
}