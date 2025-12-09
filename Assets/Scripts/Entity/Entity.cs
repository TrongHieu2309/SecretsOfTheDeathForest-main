using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    [Header("MOVEMENT DETAILS")]
    [SerializeField] protected float moveSpeed;

    [Header("HEALTH POINT BAR")]
    [SerializeField] protected float maxHp;
    [HideInInspector]public float currentHp;
    [SerializeField] private Image healthPointUI;

    [Header("ATTACK DETAILS")]
    [SerializeField] public float attackDistance;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask targetLayer;

    protected bool isFacingRight = true;
    protected bool isAttacking;

    protected float horizontalInput;
    protected Rigidbody2D rb;
    protected Animator anim;
    protected BoxCollider2D boxCollider2D;
    protected float dir;

    public Animator Anim => anim;
    public Rigidbody2D Rb => rb;
    public float HorizontalInput => horizontalInput;
    public float MoveSpeed => moveSpeed;
    public bool IsFacingRight => isFacingRight;

    public bool IsAttacking
    {
        get => isAttacking;
        set
        {
            if (isAttacking == value) return;
            
            isAttacking = value;
            
            OnAttackStateChanged?.Invoke(isAttacking);
        }
    }

    public event System.Action<bool> OnAttackStateChanged;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        currentHp = maxHp;
    }

    protected virtual void Update()
    {
        if (IsAttacking)
        {
            horizontalInput = 0;
        }
        else
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
        }
    }

    public void HandleFlip(Transform transform)
    {
        if (horizontalInput < 0 && isFacingRight) Flip(transform);
        else if (horizontalInput > 0 && !isFacingRight) Flip(transform);
    }

    public void Flip(Transform transform)
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0, -180, 0);
    }

    public void TakeDamage(float damageValue)
    {
        currentHp -= damageValue;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHpBar();

        if (currentHp <= 0)
        {
            anim.SetTrigger("dead");
            HandleDead();
        }
        else
        {
            anim.SetTrigger("hurt");
        }
    }

    protected virtual void HandleDead()
    {
        rb.gravityScale = 0;
        boxCollider2D.enabled = false;
        rb.linearVelocity = Vector2.zero;
    }

    protected void UpdateHpBar()
    {
        if (healthPointUI != null)
        {
            healthPointUI.fillAmount = currentHp / maxHp;
        }
    }
}