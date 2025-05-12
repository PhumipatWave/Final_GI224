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
        public bool[] isLevelUnlock = new bool[3];
        public bool[] isComplete = new bool[3];
        public int[] Score = new int[3];
    }

    public void GameSave(int currentLevel, bool isComplete)
    {
        string fileName = "game-save-data.txt";
        string filePath = Application.persistentDataPath + "/" + fileName;

        SaveData gameData = new SaveData();

        // On start game first time
        if (currentLevel == 0)
        {
            gameData.isLevelUnlock = new bool[] { true, false, false };
            gameData.isComplete = new bool[] { false, false, false };
            gameData.Score = new int[] { 0, 0, 0 };
        }
        // On start level 1
        else if (currentLevel == 1)
        {
            if (isComplete)
            {
                gameData.isLevelUnlock = new bool[] { true, true, false };
                gameData.isComplete = new bool[] { true, false, false };
                gameData.Score = new int[] { scores, 0, 0 };
            }
            else
            {
                gameData.isLevelUnlock = new bool[] { true, false, false };
                gameData.isComplete = new bool[] { false, false, false };
                gameData.Score = new int[] { scores, 0, 0 };
            }
        }
        // On start level 2
        else if (currentLevel == 2)
        {
            if (isComplete)
            {
                gameData.isLevelUnlock = new bool[] { true, true, true };
                gameData.isComplete = new bool[] { true, true, false };
                gameData.Score = new int[] { levelScore[0], scores, 0 };
            }
            else
            {
                gameData.isLevelUnlock = new bool[] { true, true, false };
                gameData.isComplete = new bool[] { true, false, false };
                gameData.Score = new int[] { levelScore[0], scores, 0 };
            }
        }
        // On start level 3
        else if (currentLevel == 3)
        {
            if (isComplete)
            {
                gameData.isLevelUnlock = new bool[] { true, true, true };
                gameData.isComplete = new bool[] { true, true, true };
                gameData.Score = new int[] { levelScore[0], levelScore[1], scores };
            }
            else
            {
                gameData.isLevelUnlock = new bool[] { true, true, true };
                gameData.isComplete = new bool[] { true, true, false };
                gameData.Score = new int[] { gameData.Score[0], levelScore[1], scores };
            }
        }

        string content = JsonUtility.ToJson(gameData);
        File.WriteAllText(filePath, content);
    }

    public void GameLoad()
    {
        string fileName = "game-save-data.txt";
        string filePath = Application.persistentDataPath + "/" + fileName;

        // Check is it have save file
        if (File.Exists(filePath))
        {
            LoadGameData(filePath);
        }
        else
        {
            GameSave(0, false);
            LoadGameData(filePath);
        }
    }

    public void LoadGameData(string filePath)
    {
        string content = File.ReadAllText(filePath);
        Debug.Log(filePath);
        Debug.Log(content);

        SaveData gameData = JsonUtility.FromJson<SaveData>(content);

        for (int i = 0; i <= 2; i++)
        {
            UiManager.GetInstance().LevelBlock[i].color =
                gameData.isLevelUnlock[i] ? Color.yellow : Color.red;

            UiManager.GetInstance().LevelInfo[i].text =
                $"{(gameData.isLevelUnlock[i] ? "Unlock" : "Lock")}\n" +
                $"{(gameData.isComplete[i] ? "Complete" : "Not Complete")}\n" +
                $"Score : {gameData.Score[i]}";

            levelScore[i] = gameData.Score[i];

            Debug.Log(UiManager.GetInstance().LevelInfo[i].text);
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