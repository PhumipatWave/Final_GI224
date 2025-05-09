using UnityEngine;

public class Enemy : Character
{
    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        Health = 50;
        Damage = 10;
        Speed = 100f;
        AttackCooldown = 3f;
    }

    public override void Death()
    {
        GameManager.GetInstance().ReturnPrefab(gameObject);
    }

    public override void Move()
    {
        transform.Translate(Vector3.left * Time.deltaTime * Speed);
        anim.SetFloat("moveSpeed", 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
