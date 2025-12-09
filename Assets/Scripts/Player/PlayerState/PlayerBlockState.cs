using UnityEngine;

public class PlayerBlockState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.Anim.SetBool("block", true);
        player.Rb.linearVelocity = Vector2.zero;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.Anim.SetBool("blocked", player.blocked);

        if (Input.GetMouseButtonUp(1))
        {
            player.SwitchState(player.IdleState);
        }

        if (player.blocked)
        {
            player.BlockedSound();

            player.blockTimer = 0;
            player.blocked = false;
            player.SwitchState(player.IdleState);
        }
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.isBlocking = false;
        player.Anim.SetBool("block", false);
        Debug.Log("Exit Block State");
    }
}