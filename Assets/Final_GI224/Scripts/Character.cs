using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public Rigidbody rb;
    public Animation anim;

    private int health;
    private int damages;
    private float speed;
    float attackCooldown;

    public void Initalize()
    {

    }

    public void TakesDamages(int damage)
    {
        health -= damage;

        Debug.Log($"{health}");

        if (health < 0) 
        {
            Death();
        }
    }

    public abstract void Death();
    
    public abstract void Move();

    public abstract void Attack();
}
