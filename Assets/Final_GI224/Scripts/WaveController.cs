using UnityEngine;

public class WaveController : MonoBehaviour
{
    public Transform[] spawnPoints;
    public Wave currentWave;
    public Wave[] waves;

    private int currentWaves;
    private float waveEndTime = 0f;

    private int enemiesSpawned = 0;
    private float nextSpawnTime = 0f;

    private float startNextWave;

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
        waveEndTime = Time.time + waves[currentWaves].WaveInterval;
    }

    void Update()
    {
        if (currentWaves >= waves.Length)
            return;

        if (Time.time >= waveEndTime && IsComplete())
        {
            currentWaves++;

            if (currentWaves >= waves.Length)
            {
                UiManager.GetInstance().SetEndScreen(true);
            }
            else
            {
                if (startNextWave >= 5f)
                {
                    StartWave(waves[currentWaves]);
                    waveEndTime += Time.time + waves[currentWaves].WaveInterval;
                    startNextWave = 0;
                    UiManager.GetInstance().UpdateTimeBeforeNextWave(startNextWave, true);
                }
                else 
                {
                    startNextWave += Time.deltaTime;
                    UiManager.GetInstance().UpdateTimeBeforeNextWave(startNextWave,false);
                }
            }
        }

        if (currentWave == null) return;

        if (enemiesSpawned < currentWave.TotalEnemy && Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            enemiesSpawned++;
            nextSpawnTime = Time.time + currentWave.SpawnInterval;
        }
    }

    void SpawnEnemy()
    {
        var p = GameManager.GetInstance().SpawnPrefab();

        int random = Random.Range(0, spawnPoints.Length);

        p.transform.SetPositionAndRotation(spawnPoints[random].transform.position, p.transform.rotation);
    }
}
