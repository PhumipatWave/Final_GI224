using UnityEngine;
using System.Collections.Generic;


public class Fence : MonoBehaviour
{
    private Player[] player = new Player[2];

    //Get Player component
    private void Start()
    {
        var p1 = GameObject.Find("Player1");
        var p2 = GameObject.Find("Player2");

        player[0] = p1.GetComponent<Player>();
        player[1] = p2.GetComponent<Player>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            if (collision.gameObject.CompareTag("Enemy"))
            {
                // Both Player take half damage
                player[0].TakeDamaged(enemy.damages / 2);
                player[1].TakeDamaged(enemy.damages / 2);

                UiManager.GetInstance().UpdatePleyerHp(0, player[0].Health);
                UiManager.GetInstance().UpdatePleyerHp(1, player[1].Health);

                GameManager.GetInstance().ReturnPrefab(collision.gameObject);
            }
        }
    }
}
