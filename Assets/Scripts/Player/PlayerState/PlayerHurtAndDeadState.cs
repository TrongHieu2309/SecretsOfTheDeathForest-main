using UnityEngine;

public class PlayerHurtAndDeadState : PlayerBaseState
{
    private float timer = 0f;
    private float hurtCoolDown = 0.3f;
    public override void EnterState(PlayerStateManager player)
    {
        player.Rb.linearVelocity = Vector2.zero;
        if (player.currentHp > 0) {player.Anim.SetTrigger("hurt");}
        else {player.Anim.SetTrigger("dead");}
    }

    public override void UpdateState(PlayerStateManager player)
    {
        timer += Time.deltaTime;
        if (timer >= hurtCoolDown)
        {
            player.SwitchState(player.IdleState);
        }
    }

    public override void ExitState(PlayerStateManager player)
    {
        Debug.Log("Exit Hurt or Dead State");
    }
}