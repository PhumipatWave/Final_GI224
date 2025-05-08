using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public Transform[] spawnPoint;
    public int spawnCounts;
    List<Transform> usedSpawn = new List<Transform>();

    public GameObject[] obstaclePrefab;
    private Player playerController;

    private void Awake()
    {
        var player = GameObject.Find("Player");
        playerController = player.GetComponent<Player>();
    }

    void Spawn() //Spawn object at the location
    {
        int randomPoint = Random.Range(0, spawnPoint.Length);//สุ่มตำแหน่งเกิด

        int randomObject = Random.Range(0, obstaclePrefab.Length);//สุ่มมอนที่จะเกิด


        if (!usedSpawn.Contains(transform))
        {
            Instantiate
            (
                obstaclePrefab[randomObject], spawnPoint[randomPoint].position, obstaclePrefab[randomObject].transform.rotation
            );

            usedSpawn.Add(spawnPoint[randomPoint]);

            spawnCounts++;
        }

    }

    IEnumerator SpawnCorontine()//Spawn many object at the same time
    {
        yield return new WaitForSeconds(2f);

        while (!playerController.isGameOver && !playerController.isGameWin && !playerController.isGamePause)
        {
            do
            {
                Spawn();
            }
            while (spawnCounts < 3);

            usedSpawn.Clear();

            spawnCounts = 0;

            yield return new WaitForSeconds(2f);
        }

    }
}
