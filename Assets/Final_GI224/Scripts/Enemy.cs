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
        transform.Translate(Vector3.forward * Time.deltaTime * Speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (collision.gameObject.CompareTag("Player1"))
        {
            UiManager.GetInstance().UpdatePleyerHp(0,player.Health);
        }
        else if (collision.gameObject.CompareTag("Player2"))
        {
            UiManager.GetInstance().UpdatePleyerHp(1,player.Health);
        }
    }
}
