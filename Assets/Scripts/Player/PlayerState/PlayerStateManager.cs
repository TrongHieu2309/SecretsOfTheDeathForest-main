using System.Collections;
using Unity.Android.Gradle;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateManager : Entity
{
    public static PlayerStateManager instance { get; private set; }

    [Header("JUMP DETAILS")]
    public float jumpForce = 15;
    public float wallJumpForce;
    public float wallJumpHorizontalForce;
    [HideInInspector] public float wallSlideSpeed = 0.3f;
    [HideInInspector] public float wallJumpDuration = 0.3f;
    [HideInInspector] public bool isWallJumping;
    /* [HideInInspector] */ public bool onWall;
    [HideInInspector] public bool isWallSliding;

    [Header("BLOCK")]
    [SerializeField] private Image ImgBlockedCoolDown;
    public Animator animShieldCooldown;
    public float blockCoolDown = 5f;
    [HideInInspector] public float blockTimer;
    [HideInInspector] public bool isBlocking;
    [HideInInspector] public bool blocked;

    [Header("AUDIO")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip swordSound;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip shieldSound;
    [SerializeField] private AudioClip gameOverSound;

    [Header("CHECK DETAILS")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Vector2 groundCheckSize;
    [SerializeField] private Vector2 wallCheckSize;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask skullLayer;
    [SerializeField] private LayerMask wallLayer;

    /*-----POISONING-----*/
    [Header("POISON")]
    [SerializeField] private GameObject poison;
    [SerializeField] private Image poisonImage;
    [SerializeField] private Image skillImage;
    [HideInInspector] public float maxSkill;
    [HideInInspector] public float currentSkill;
    [HideInInspector] private float damageSkill = 1f;
    [HideInInspector] private float damagePoison = 5f;
    [HideInInspector] private float timerDamagePoison = 2f;
    [HideInInspector] private float coolDownPoison = 10f;
    [HideInInspector] public float timerPoison;
    [HideInInspector] private bool isPoisoning;

    [HideInInspector] public bool isGrounded;
    [HideInInspector] public int currentCombo = 0;
    [HideInInspector] public int maxCombo = 3;
    [HideInInspector] public float comboTimer = 0;
    [HideInInspector] public float comboCooldown = 0.7f;

    PlayerBaseState currentState;
    public PlayerIdleState IdleState = new PlayerIdleState();
    public PlayerMoveState MoveState = new PlayerMoveState();
    public PlayerBaseState JumpState = new PlayerJumpState();
    public PlayerBaseState FallState = new PlayerFallState();
    public PlayerBaseState AttackState = new PlayerAttackState();
    public PlayerBlockState BlockState = new PlayerBlockState();
    public PlayerWallJumpState WallJumpState = new PlayerWallJumpState();
    public PlayerHurtAndDeadState HurtAndDeadState = new PlayerHurtAndDeadState();

    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        poison.SetActive(false);
        currentSkill = maxSkill;
        blockTimer = blockCoolDown;
    }

    void Start()
    {
        currentState = IdleState;
        currentState.EnterState(this);
    }

    protected override void Update()
    {
        base.Update();
        if (blockTimer < blockCoolDown) blockTimer += Time.deltaTime;

        if (blockTimer < blockCoolDown) {animShieldCooldown.SetBool("shield", true);}
        else {animShieldCooldown.SetBool("shield", false);}

        isGrounded = IsGrounded();
        onWall = IsWall();

        Poisoning();
        UpdatePoisonUI();
        UpdateBlockedCoolDown();

        float velY = Mathf.Sign(rb.linearVelocity.y);
        anim.SetFloat("yVelocity", velY);
        anim.SetBool("isGrounded", isGrounded);

        currentState.UpdateState(this);
    }

    public void SwitchState(PlayerBaseState newState)
    {
        currentState.ExitState(this);
        currentState = newState;
        newState.EnterState(this);

    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer | skullLayer) != null;
    }

    private bool IsWall()
    {
        return Physics2D.OverlapBox(wallCheck.position, wallCheckSize, 0, wallLayer) != null;
    }

    public void JumpSound()
    {
        SoundManager.instance.PlaySound(jumpSound);
    }

    public void HurtSound()
    {
        SoundManager.instance.PlaySound(hurtSound);
    }

    public void BlockedSound()
    {
        SoundManager.instance.PlaySound(shieldSound);
    }

    private void StopWallJump()
    {
        isWallJumping = false;
    }

    public IEnumerator WallJumpCooldown()
    {
        yield return new WaitForSeconds(wallJumpDuration);
        StopWallJump();
    }

    public void UpdateSkillBar()
    {
        if (skillImage != null)
        {
            skillImage.fillAmount = currentSkill / maxSkill;
        }
    }

    private void UpdateBlockedCoolDown()
    {
        if (ImgBlockedCoolDown != null)
            ImgBlockedCoolDown.fillAmount = blockTimer / blockCoolDown;
    }

    public void Poison()
    {
        isPoisoning = true;
        poison.SetActive(true);
        timerPoison = coolDownPoison;
    }

    private void Poisoning()
    {
        if (isPoisoning)
        {
            timerDamagePoison -= Time.deltaTime;
            if (timerDamagePoison <= 0f)
            {
                if (currentSkill > 0f)
                {
                    currentSkill = Mathf.Max(currentSkill - damageSkill, 0f);
                    UpdateSkillBar();
                    timerDamagePoison = 2f;
                }
                else
                {
                    Entity entity = this.GetComponent<Entity>();
                    if (entity != null)
                        entity.TakeDamage(damagePoison);
                    timerDamagePoison = 2f;
                    SwitchState(HurtAndDeadState);
                }
            }
            timerPoison -= Time.deltaTime;
        }

        if (timerPoison <= 0f)
        {
            isPoisoning = false;
            poison.SetActive(false);
        }
    }

    private void UpdatePoisonUI()
    {
        if (poisonImage != null)
        {
            poisonImage.fillAmount = timerPoison / coolDownPoison;
        }
    }

    /*-----ANIAMTION EVENT-----*/
    public void SwordSound()
    {
        SoundManager.instance.PlaySound(swordSound);
    }

    public void GameOverSound()
    {
        SoundManager.instance.PlaySound(gameOverSound);
    }

    public void GameOver()
    {
        GameManager.instance.GameOverManagement();
    }

    public void DamageEnemy()
    {
        Collider2D[] enemyCollider = Physics2D.OverlapCircleAll(attackPoint.position, attackDistance, targetLayer);
        
        foreach (Collider2D enemies in enemyCollider)
        {
            if (enemies.CompareTag("Enemy"))
            {
                Entity entity = enemies.GetComponent<Entity>();
                if (entity != null)
                    entity.TakeDamage(5);
            }
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
        Gizmos.DrawWireSphere(attackPoint.position, attackDistance);
    }
}
