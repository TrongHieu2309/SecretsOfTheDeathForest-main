using UnityEngine;

public class Mushroom : EnemyController
{
    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        damageValue = 25f;
        timerRest = 0.7f;
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
            }
            if (PlayerStateManager.instance.isBlocking)
            {
                PlayerStateManager.instance.blocked = true;
                anim.SetTrigger("stun");
                Debug.Log("Blocked by Player");
            }
        }
    }
}
