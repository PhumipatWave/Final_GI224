using UnityEngine;

public abstract class Character : MonoBehaviour
{
    // Declare basic character component
    public Rigidbody rb;
    public Animator anim;

    // Declare encapsulation variable
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

    // Declare method
    // Initialize the component
    public void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    public void TakeDamaged(int damage) 
    {
        health -= damage;

        if (health <= 0)
        {
            Death();
        }
    }

    public abstract void Death();
    public abstract void Move();
}
