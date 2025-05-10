using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public int scores = 0;

    private static GameManager instance;
    private int[] poolSizePrefabs = { 10 , 15 };
    private bool changePrefab;

    [SerializeField]private GameObject[] prefabs;
    [SerializeField] private GameObject bulletPrefab;

    private List<GameObject> bulletsPools = new List<GameObject>();
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

        int currentPre = 0;
        for (int i = 0; i < poolSizePrefabs[0]; i++)
        {
            CreateBullet();
        }

        for (int i = 0; i < poolSizePrefabs[1]; i++)
        {
            if (i > poolSizePrefabs[1]/prefabs.Length && i < poolSizePrefabs[1]*2/prefabs.Length)
            {
                currentPre = 1;
            }
            else if ( i > poolSizePrefabs[1]*2 / prefabs.Length)
            {
                currentPre = 2;
            }

            CreateNewPrefab(currentPre);
        }
    }

    private void GameResult()
    {
        
    }

    //Enemy
    public void CreateNewPrefab(int prefabNum)
    {
        GameObject c = Instantiate(prefabs[prefabNum]);

        c.SetActive(false);

        prefabsPools.Add(c);
    }

    public GameObject SpawnPrefab()
    { 
        if(prefabsPools.Count == 0) 
        {
            for (int i = 0; i < 3; i++)
            {
                CreateNewPrefab(i);
            }
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

    //Score
    public void AddScores(int scoreGain)
    {
        scores += scoreGain;
        UiManager.GetInstance().UpdatePoints(scores);
    }

    //Bullet
    public void CreateBullet()
    {
        GameObject b = Instantiate(bulletPrefab);

        b.SetActive(false);

        bulletsPools.Add(b);
    }

    public GameObject SpawnBullet()
    {
        if (bulletsPools.Count == 0)
        {
            CreateBullet();
        }

        GameObject b = bulletsPools[0];

        bulletsPools.RemoveAt(0);
        b.SetActive(true);
        return b;
    }

    public void ReturnBullet(GameObject bull)
    {
        bulletsPools.Add(bull);
        bull.SetActive(false);
    }
}
