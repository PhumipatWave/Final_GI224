using UnityEngine;

public class Enemy : Character
{
    [SerializeField]private int pointsGain;

    public int hp;
    public int damages;
    float playSound;


    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        Health = hp;
        Damage = damages;
        Speed = 8f;
        AttackCooldown = 3f;

        walkSound.volume = PlayerPrefs.GetFloat("SFXVolume", 0.05f);
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
        transform.Translate(Vector3.forward * Time.deltaTime * Speed);
        rb.linearVelocity = Vector3.forward * Speed * Time.deltaTime;

        playSound += 0.5f;
        playSound = PlayWalkSound(playSound);

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
