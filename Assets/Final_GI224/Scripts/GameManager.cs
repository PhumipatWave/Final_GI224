using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // Sigleton pattern
    private static GameManager instance;
    private int[] poolSizePrefabs = {10, 15};
    public int scores = 0;

    [SerializeField] private GameObject[] prefabs;
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
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InstiantiatePlayer();
        UiManager.GetInstance().UpdatePoints(scores);

        int currentPre = 0;
        for (int i = 0; i < poolSizePrefabs[0]; i++)
        {
            CreateBullet();
        }

        for (int i = 0; i < poolSizePrefabs[1]; i++)
        {
            if (i > poolSizePrefabs[1] / prefabs.Length && i < poolSizePrefabs[1] * 2 / prefabs.Length)
            {
                currentPre = 1;
            }
            else if (i > poolSizePrefabs[1] * 2 / prefabs.Length)
            {
                currentPre = 2;
            }

            CreateNewPrefab(currentPre);
        }
    }

    public GameObject PlayerPrefab;
    public int playerCount;

    public void InstiantiatePlayer()
    {
        playerCount = 2;

        for (int i = 0; i < playerCount; i++)
        {
            SpawnPlayer(i);
        }
    }

    public void SpawnPlayer(int p)
    {
        GameObject player = Instantiate(PlayerPrefab);
        player.gameObject.name = ($"Player{p + 1}");
    }

    [SerializeField]
    class SaveData
    {
        public int CurrentLevel;
        public int CurrentWave;
        public int CurrentEnemySpawned;
        public int[] Player_Health = { 0, 0 };
        public int[] Player_FireCount = { 0, 0 };
        public int[] Player_Kill = { 0, 0 };
    }

    private void GameResult()
    {

    }

    public void GameSave()
    {
        string fileName = "game-save-data.txt";
        string filePath = Application.persistentDataPath + "/" + fileName;

        SaveData gameData = new SaveData();

        Player p1 = GameObject.Find("Player1").GetComponent<Player>();
        Player p2 = GameObject.Find("Player2").GetComponent<Player>();

        gameData.CurrentLevel = 0;
        gameData.CurrentWave = 0;
        gameData.CurrentEnemySpawned = 0;
        gameData.Player_Health[0] = p1.Health;
        gameData.Player_Health[1] = p2.Health;
        gameData.Player_FireCount[0] = p1.FireCount;
        gameData.Player_FireCount[1] = p2.FireCount;
        gameData.Player_Kill[0] = p1.EnemyKill;
        gameData.Player_Kill[1] = p2.EnemyKill;

        string content = JsonUtility.ToJson(gameData);
        File.WriteAllText(filePath, content);
    }

    public void GameLoad()
    {
        string fileName = "game-save-data.txt";
        string filePath = Application.persistentDataPath + "/" + fileName;
        string content = File.ReadAllText(filePath);
        Debug.Log(content);

        SaveData gameData = JsonUtility.FromJson<SaveData>(content);

        Player p1 = GameObject.Find("Player1").GetComponent<Player>();
        Player p2 = GameObject.Find("Player2").GetComponent<Player>();

        p1.Health = gameData.Player_Health[0];
        p2.Health = gameData.Player_Health[1];
        p1.FireCount = gameData.Player_FireCount[0];
        p2.FireCount = gameData.Player_FireCount[1];
        p1.EnemyKill = gameData.Player_Kill[0];
        p2.EnemyKill = gameData.Player_Kill[1];
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
        if (prefabsPools.Count == 0)
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