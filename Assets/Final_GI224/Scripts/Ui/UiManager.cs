using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    private static UiManager instance;

    public bool starterSceneActive;

    [Header("SelectLevel")]
    public TMP_Text[] LevelInfo;
    public Image[] LevelBlock;

    [Header("Screen")]
    [SerializeField] private TMP_Text displayWinLose;
    [SerializeField] private GameObject panelSettings;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject selectLevelScreen;
    [SerializeField] private GameObject endGameScreen;
    [SerializeField] private GameObject optionScreen;

    [Header("Button")]
    [SerializeField] private Button start;
    [SerializeField] private Button returnButton;
    [SerializeField] private Button[] levelChooseButton;
    [SerializeField] private Button backButton;

    [Header("GamePlayScreen")]
    [SerializeField] private TMP_Text[] displayHpPlayers;
    [SerializeField] private TMP_Text displayPoints;
    [SerializeField] private TMP_Text countDown;
    [SerializeField] private GameObject gamePlayScreen;

    public static UiManager GetInstance()
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
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "StartMenu")
        {
            StarterScene();

            if (levelChooseButton != null)
            {
                levelChooseButton[0].onClick.AddListener(() => OnSelectLevel(1));
                levelChooseButton[1].onClick.AddListener(() => OnSelectLevel(2));
                levelChooseButton[2].onClick.AddListener(() => OnSelectLevel(3));
            }
        }

        if (start != null)
        {
            start.onClick.AddListener(OnStart);
        }

        if (returnButton != null)
        {
            returnButton.onClick.AddListener(ReturnStarter);
        }

        if (backButton != null)
        {
            backButton.onClick.AddListener(BackToStarter);
        }
    }

    //Active on Starter Screen
    public void StarterScene()
    {
        startScreen.SetActive(true);
        selectLevelScreen.SetActive(false);
        endGameScreen.SetActive(false);
        optionScreen.SetActive(false);
        gamePlayScreen.SetActive(false);

        starterSceneActive = true;
    }

    //Active on Option Screen
    public void OptionScene()
    {
        Time.timeScale = 0;

        startScreen.SetActive(false);
        endGameScreen.SetActive(false);
        optionScreen.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        var activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.name);

        Time.timeScale = 1;
    }

    //Return starter page
    public void ReturnStarter()
    {
        Time.timeScale = 1;

        starterSceneActive = true;
        startScreen.SetActive(true);
        endGameScreen.SetActive(false);
        SceneManager.LoadScene("StartMenu");
    }

    public void BackToStarter()
    {
        starterSceneActive = true;
        selectLevelScreen.SetActive(false);
        startScreen.SetActive(true);
    }

    //Win or Lose Screen
    public void SetEndScreen(bool win)
    {
        endGameScreen.SetActive(true);
        startScreen.SetActive(false);
        optionScreen.SetActive(false);
        gamePlayScreen.SetActive(false);

        Time.timeScale = 0;

        if (panelSettings != null)
        {
            Image color = panelSettings.GetComponent<Image>();

            if (win)
            {
                color.color = Color.green;
                displayWinLose.text = "congratulations you complete this level!";
            }
            else
            {
                color.color = Color.red;
                displayWinLose.text = "Try again next time!";
            }

            if (SceneManager.GetActiveScene().name == "Level-01")
            {
                GameManager.GetInstance().GameSave(0, win);
                Debug.Log("Game win level 1 save");
            }
            else if (SceneManager.GetActiveScene().name == "Level-02")
            {
                GameManager.GetInstance().GameSave(1, win);
                Debug.Log("Game win level 2 save");
            }
            else if (SceneManager.GetActiveScene().name == "Level-03")
            {
                GameManager.GetInstance().GameSave(2, win);
                Debug.Log("Game win level 3 save");
            }
        }
    }

    //When press Start button
    public void OnStart()
    {
        startScreen.SetActive(false);
        selectLevelScreen.SetActive(true);
    }

    public void OnSelectLevel(int level)
    {
        if (LevelBlock[level - 1].color == Color.yellow)
        {
            SceneManager.LoadScene($"Level-0{level}");

            gamePlayScreen.SetActive(true);

            selectLevelScreen.SetActive(false);
            starterSceneActive = false;

            GameManager.GetInstance().scores = 0;
        }
        else
        {
            Debug.Log("This level is lock!");
        }
    }

    //Update time before Spawn
    public void UpdateTimeBeforeNextWave(int currentWave, float secounds,bool startWave)
    {
        countDown.gameObject.SetActive(true);

        if (startWave)
        {
            countDown.gameObject.SetActive(false);
        }
        else 
        {
            countDown.text = $"Wave {currentWave + 1} : Start On {Mathf.CeilToInt(secounds).ToString()}";
        }
    }

    //Update Player Health
    public void UpdatePleyerHp(int player,float health)
    {
        displayHpPlayers[player].text = $"Player{player + 1} \n Health : {health.ToString()}";
    }

    //Update Score
    public void UpdatePoints(int point)
    {
        displayPoints.text = $"Points = {point}";
    }
}