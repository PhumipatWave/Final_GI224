using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    [SerializeField]
    class SaveData
    {
        public int CurrentLevel;
        public int CurrentWave;
        public int CurrentEnemySpawned;
        public int Player1_Health, Player2_Health;
        public int Player1_Fired, Player2_Fired;
    }

    private void GameResult()
    {
        
    }
}
