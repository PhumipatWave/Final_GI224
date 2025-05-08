using System.IO;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : MonoBehaviour
{
    // Sigleton pattern
    private static GameManager instance;
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

    private void Start()
    {
        InstiantiatePlayer();
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
        public int[] Player_Health = {0, 0};
        public int[] Player_FireCount = { 0, 0 };
        public int[] Player_Kill = { 0, 0 };
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

    public void GameResult()
    {
        
    }
}
