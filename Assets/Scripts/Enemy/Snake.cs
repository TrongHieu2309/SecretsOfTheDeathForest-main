using UnityEngine;

public class Snake : EnemyController
{
    protected override void Awake()
    {
        base.Awake();
        damageValue = 10f;
        timerRest = .3f;
        coolDownAttack = 2f;
    }

    protected override void DamagePlayer()
    {
        if (DetectPlayer())
        {
            if (PlayerStateManager.instance != null && !PlayerStateManager.instance.isBlocking)
            {
                PlayerStateManager.instance.TakeDamage(damageValue);
                PlayerStateManager.instance.SwitchState(PlayerStateManager.instance.HurtAndDeadState);
                PlayerStateManager.instance.Poison();
            }
            if (PlayerStateManager.instance.isBlocking)
            {
                PlayerStateManager.instance.blocked = true;
                Debug.Log("Blocked by Player");
            }  
        }
    }
}
