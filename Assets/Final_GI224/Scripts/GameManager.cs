using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public int scores = 0;

    [SerializeField]private int initialPoolSize = 30;
    private static GameManager instance;
    [SerializeField]private GameObject[] prefabs;
    private int[] poolSizePrefabs;
    private List<GameObject> prefabsPools = new List<GameObject>();

    public static GameManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField]
    class SaveData
    {
        public int CurrentLevel;
        public int CurrentWave;
        public int CurrentEnemySpawned;
        public int Player1_Health, Player2_Health;
        public int Player1_Fired, Player2_Fired;
    }

    private void Start()
    {
        UiManager.GetInstance().UpdatePoints(scores);

        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewPrefab();
        }
    }

    private void GameResult()
    {
        
    }

    public void CreateNewPrefab()
    {
        int prefabNum = Random.Range(0, prefabs.Length);

        GameObject c = Instantiate(prefabs[prefabNum]);

        c.SetActive(false);

        prefabsPools.Add(c);
    }

    public GameObject SpawnPrefab()
    { 
        if(prefabsPools.Count == 0) 
        {
            int r = Random.Range(0, prefabs.Length);

            CreateNewPrefab();
        }

        int random = Random.Range(0, prefabsPools.Count);

        GameObject p = prefabsPools[random];

        prefabsPools.RemoveAt(random);

        p.SetActive(true);
        return p;
    }

    public void ReturnPrefab(GameObject en)
    {
        prefabsPools.Add(en);
        en.SetActive(false);
    }

    public void AddScores(int scoreGain)
    {
        scores += scoreGain;
        UiManager.GetInstance().UpdatePoints(scores);
    }
}
