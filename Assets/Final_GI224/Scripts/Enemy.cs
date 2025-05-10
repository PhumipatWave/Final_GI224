using UnityEngine;

public class Enemy : Character
{
    [SerializeField]private int pointsGain;

    public int hp;
    public int damages;
    public int velocity;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        Health = hp;
        Damage = damages;
        Speed = velocity;
        AttackCooldown = 3f;
    }

    private void Update()
    {
        Move();
    }

    public override void Death()
    {
        GameManager.GetInstance().ReturnPrefab(gameObject);
        GameManager.GetInstance().AddScores(pointsGain);
    }

    public override void Move()
    {
        transform.Translate(Vector3.forward*Time.deltaTime*Speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Bullet bullet;

        if (collision.gameObject.GetComponent<Bullet>())
        {
            bullet = collision.gameObject.GetComponent<Bullet>();
            TakeDamaged(bullet.damages);

            GameManager.GetInstance().ReturnBullet(collision.gameObject);
        }
    }
}
