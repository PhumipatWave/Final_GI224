using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public Transform[] spawnPoints;
    public Wave currentWave;
    public Wave[] waves;
    List<Transform> usedSpawn = new List<Transform>();

    private int currentWaves;
    private float waveEndTime = 0f;

    private int enemiesSpawned = 0;
    private float nextSpawnTime = 2f;

    private float startNextWave;
    private bool startWave = false;

    public bool IsComplete()
    {
        return enemiesSpawned >= currentWave?.TotalEnemy;
    }

    public void StartWave(Wave wave)
    {
        currentWave = wave;
        enemiesSpawned = 0;
        nextSpawnTime = Time.time;
    }

    private void Start()
    {
        StartWave(waves[currentWaves]);
        waveEndTime = Time.time + waves[currentWaves].WaveInterval + startNextWave;
    }

    void Update()
    {
        if (currentWaves >= waves.Length)
            return;

        if (Time.time >= waveEndTime + startNextWave && IsComplete())
        {
            Debug.Log("End wave");
            currentWaves++;
            startWave = false;

            if (currentWaves >= waves.Length)
            {
                UiManager.GetInstance().SetEndScreen(true);
            }
            else
            {
                StartWave(waves[currentWaves]);
                waveEndTime += Time.time + waves[currentWaves].WaveInterval;
            }
        }

        if (startWave)
        {
            if (currentWave == null) return;

            if (enemiesSpawned < currentWave.TotalEnemy && Time.time >= nextSpawnTime)
            {
                StartCoroutine(SpawnCorontine());
                nextSpawnTime = Time.time + currentWave.SpawnInterval;
            }
        }
        
    }

    private void FixedUpdate()
    {
        if (startNextWave <= 0)
        {
            Debug.Log("Start");
            startWave = true;
            startNextWave = 5;

            UiManager.GetInstance().UpdateTimeBeforeNextWave(startNextWave, true);
        }

        if (!startWave)
        {
            startNextWave -= Time.deltaTime;
            UiManager.GetInstance().UpdateTimeBeforeNextWave(startNextWave, false);
        }
    }

    void SpawnEnemy()
    {
        var p = GameManager.GetInstance().SpawnPrefab();

        int random = Random.Range(0, spawnPoints.Length);

        if (!usedSpawn.Contains(transform))
        {
            p.transform.SetPositionAndRotation(spawnPoints[random].transform.position, p.transform.rotation);

            usedSpawn.Add(spawnPoints[random]);
        }
    }

    IEnumerator SpawnCorontine()
    {
        int totalPer = 0;

        while (totalPer < currentWave.EnemySpawnPerWave && enemiesSpawned < currentWave.TotalEnemy)
        {
            SpawnEnemy();
            totalPer++;
            enemiesSpawned ++;
        }

        usedSpawn.Clear();

        yield return null;
    }
}
