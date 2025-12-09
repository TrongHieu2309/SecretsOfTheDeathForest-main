using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.comboTimer = 0f;
        player.currentCombo = 0;
        player.IsAttacking = true;
        PerformNextAttack(player);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.comboTimer += Time.deltaTime;
        player.Rb.linearVelocity = Vector2.zero;

        if (player.comboTimer >= player.comboCooldown)
        {
            player.currentCombo = 0;
            player.SwitchState(player.IdleState);
        }

        if (Input.GetMouseButtonDown(0) && player.comboTimer < player.comboCooldown)
        {
            player.currentCombo++;
            if (player.currentCombo > player.maxCombo)
                player.currentCombo = 1;

            player.comboTimer = 0f;
            PerformNextAttack(player);
        }
    }

    private void PerformNextAttack(PlayerStateManager player)
    {
        int attackIndex = player.currentCombo == 0 ? 1 : player.currentCombo;
        player.Anim.SetTrigger("attack" + attackIndex);
        Debug.Log("Attack " + attackIndex);
    }

    public override void ExitState(PlayerStateManager player)
    {
        Debug.Log("Exit Attack State");
        player.IsAttacking = false;
    }
}