using System.IO;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    // Sigleton pattern
    private static GameManager instance;
    private int[] poolSizePrefabs = {10, 15};

    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private GameObject bulletPrefab;

    private List<GameObject> bulletsPools = new List<GameObject>();
    private List<GameObject> prefabsPools = new List<GameObject>();

    public int scores = 0;
    public int[] levelScore = {0, 0, 0};

    public static GameManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnEnterScene;
    }

    // Use function to set up on enter scene
    private void OnEnterScene(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "StartMenu")
        {
            Debug.Log("Start Game");
            GameStartSetUp();
        }
        else
        {
            GameManager.GetInstance().GameLoad();
        }
    }

    public void GameStartSetUp()
    {
        bulletsPools.Clear();
        prefabsPools.Clear();

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

    // Game data variable
    [System.Serializable]
    public class GameData
    {
        public bool IsLevelUnlock;
        public bool IsLevelComplete;
        public int Score;
    }

    // Save data list
    [System.Serializable]
    public class SaveData
    {
        public List<GameData> GameData = new List<GameData>();
    }

    // Initialize save data
    public SaveData InitializeData()
    {
        SaveData dataT = new SaveData();

        for (int i = 0; i < 3; i++)
        {
            dataT.GameData.Add(new GameData
            {
                IsLevelUnlock = i == 0,
                IsLevelComplete = false,
                Score = 0
            });
        }

        if (dataT.GameData == null)
        {
            Debug.Log("Null Value");
        }

        return dataT;
    }

    // Save game
    public void GameSave(int currentLevel, bool isComplete)
    {
        string fileName = "game-save-data.txt";
        string filePath = Application.persistentDataPath + "/" + fileName;

        SaveData data;

        if (File.Exists(filePath))
        {
            string content = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<SaveData>(content);
        }
        else
        {
            data = InitializeData();
        }

        if (currentLevel < data.GameData.Count)
        {
            if (isComplete)
            {
                data.GameData[currentLevel].IsLevelComplete = true;
                if (currentLevel + 1 < data.GameData.Count)
                    data.GameData[currentLevel + 1].IsLevelUnlock = true;
            }

            data.GameData[currentLevel].Score = scores;
        }

        File.WriteAllText(filePath, JsonUtility.ToJson(data));
    }

    // Check save data on load game
    public void GameLoad()
    {
        string filePath = Application.persistentDataPath + "/game-save-data.txt";
        // If don't have save file create new file
        if (!File.Exists(filePath))
        {
            SaveData newData = InitializeData();
            File.WriteAllText(filePath, JsonUtility.ToJson(newData));
        }

        LoadGameData(filePath);
        Debug.Log(filePath);
    }

    // Load game
    public void LoadGameData(string filePath)
    {
        string content = File.ReadAllText(filePath);
        SaveData data = JsonUtility.FromJson<SaveData>(content);

        for (int i = 0; i < data.GameData.Count; i++)
        {
            UiManager.GetInstance().LevelBlock[i].color = data.GameData[i].IsLevelUnlock ? Color.yellow : Color.red;

            UiManager.GetInstance().LevelInfo[i].text =
                $"{(data.GameData[i].IsLevelUnlock ? "Unlock" : "Lock")}\n" +
                $"{(data.GameData[i].IsLevelComplete ? "Complete" : "Not Complete")}\n" +
                $"Score : {data.GameData[i].Score}";

            levelScore[i] = data.GameData[i].Score;
        }
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
            for (int i = 0; i < 2; i++)
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