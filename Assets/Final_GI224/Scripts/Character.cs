using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public Rigidbody rb;
    public Animator anim;

    private int health;
    public int Health
    {
        get { return health; }
        set { health = Mathf.Clamp(value, 0, 100); }
    }

    private int damage;
    public int Damage
    {
        get { return damage; }
        set { damage = Mathf.Clamp(value, 0, 250); }
    }

    private int fireCount;
    public int FireCount
    {
        get { return fireCount; }
        set { fireCount = Mathf.Clamp(value, 0, 25); }
    }

    private float speed;
    public float Speed
    {
        get { return speed; }
        set { speed = Mathf.Clamp(value, 0f, 300f); }
    }

    private float attackCooldown;
    public float AttackCooldown
    {
        get { return attackCooldown; }
        set { attackCooldown = Mathf.Clamp(value, 0f, 5f); }
    }

    public void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    public void TakeDamaged(int damage) { }
    public abstract void Death();
    public abstract void Move();
    public abstract void Attack();
}
