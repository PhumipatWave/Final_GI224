using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damages;
    private int Speed = 20;

    private void Update()
    {
        if (transform.position.z >= 10)
        {
            GameManager.GetInstance().ReturnBullet(gameObject);
        }

        Move();
    }

    public void Move()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * Speed);
    }
}
