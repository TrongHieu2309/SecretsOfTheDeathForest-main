using UnityEngine;

public class Boss : EnemyController 
{
    private int attackNumber;
    private SpriteRenderer sprite;
    private bool upgraded = false;
    public float timerStun;

    [SerializeField] private Vector2 attack3Size;
    [SerializeField] private Transform attackPoint3;

    protected override void Awake()
    {
        base.Awake();
        sprite = GetComponent<SpriteRenderer>();
    }

    protected override void Start()
    {
        attackNumber = 2;
        coolDownAttack = 4f;
        timer = coolDownAttack;
    }

    protected override void Update()
    {
        base.Update();

        if (DetectPlayer() && this.currentHp > (this.maxHp / 2))
        {
            attackNumber = 2;
            damageValue = 20f;
        }

        if (DetectPlayer() && this.currentHp <= (this.maxHp / 2))
        {
            attackNumber = 1;
            damageValue = 30f;
        }

        StunBoss();
        UpgradeBoss();
    }

    protected override void Movement()
    {
        if (Vector2.Distance(PlayerStateManager.instance.transform.position, transform.position) > 2.5f)
        {
            Vector2 dir = (PlayerStateManager.instance.transform.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(dir.x * moveSpeed, 0);
            Flip();
        }
    }

    private void Flip()
    {
        transform.localScale = new Vector3(PlayerStateManager.instance.transform.position.x < transform.position.x ? -1 : 1, 1, 1);
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
            }
        }
        if (DetectNearByPlayer())
        {
            if (PlayerStateManager.instance != null && !PlayerStateManager.instance.isBlocking)
            {
                PlayerStateManager.instance.TakeDamage(damageValue);
                PlayerStateManager.instance.SwitchState(PlayerStateManager.instance.HurtAndDeadState);
            }
            if (PlayerStateManager.instance.isBlocking)
            {
                PlayerStateManager.instance.blocked = true;
            }
        }
    }

    private Collider2D DetectNearByPlayer()
    {
        if (attackPoint3 == null || targetLayer.value == 0) return null;

        var detectPlayer = Physics2D.OverlapBoxAll(attackPoint3.position, attack3Size, 0, targetLayer);
        foreach (var detect in detectPlayer)
        {
            if (detect != null && detect.CompareTag("Player"))
            {
                return detect;
            }
        }
        return null;
    }

    protected override void HandleAttack()
    {
        timer += Time.deltaTime;

        if (DetectPlayer() && timer >= coolDownAttack)
        {
            anim.SetTrigger("attack" + attackNumber);
            timer = 0;
        }

        if (DetectNearByPlayer())
        {
            this.attackDistanceEnemy = new Vector2(0, 0);
            anim.ResetTrigger("hurt");
            anim.SetBool("attack3", true);
            damageValue = 35f;
        }
        else
        {
            this.attackDistanceEnemy = new Vector2(2.4f, 3.5f);
            anim.SetBool("attack3", false);
        }
    }

    private void UpgradeBoss()
    {
        if (!upgraded && this.currentHp <= (this.maxHp / 2))
        {
            upgraded = true;
            moveSpeed = 5;
            damageValue = 30f;
            sprite.color = new Color32(255, 180, 180, 255);
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(attackPoint3.position, attack3Size);
    }

    public void WinGame()
    {
        Destroy(gameObject);
        GameManager.instance.WinGame();
    }

    public void StunBoss()
    {
        if (timerStun > 0)
        {
            anim.SetBool("stunned", true);
            canMove = true;
            this.attackDistanceEnemy = new Vector2(0, 0);
            this.attack3Size = new Vector2(0, 0);
            timerStun -= Time.deltaTime;
        }
        else
        {
            anim.SetBool("stunned", false);
            canMove = false;
            this.attackDistanceEnemy = new Vector2(2.4f, 3.5f);
            this.attack3Size = new Vector2(3f, 2f);
        }
    }
}
