using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    private static UiManager instance;
    private int difficulty;

    public bool starterSceneActive;

    [Header("Screen")]
    [SerializeField] private TMP_Text displayWinLose;
    [SerializeField] private GameObject panelSettings;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameObject optionScreen;

    [Header("Button")]
    [SerializeField] private Button start;
    [SerializeField] private Button returnButton;
    [SerializeField] private Button[] difficultChooseButton;

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
            Destroy(instance);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {

        if (SceneManager.GetActiveScene().name == "StartScene")
        {
            StarterScene();
        }

        if (start != null)
        {
            start.onClick.AddListener(PlayPress);
        }
        if (returnButton != null)
        {
            returnButton.onClick.AddListener(ReturnStarter);
        }
    }

    //Active on Starter Screen
    public void StarterScene()
    {
        startScreen.SetActive(true);
        endScreen.SetActive(false);
        optionScreen.SetActive(false);
        gamePlayScreen.SetActive(false);

        starterSceneActive = true;
    }

    //Active on Option Screen
    public void OptionScene()
    {
        Time.timeScale = 0;

        startScreen.SetActive(false);
        endScreen.SetActive(false);
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
    }

    //Return starter page
    public void ReturnStarter()
    {
        SceneManager.LoadScene("StartScene");
    }

    //Win or Lose Screen
    public void SetEndScreen(bool win)
    {
        endScreen.SetActive(true);
        startScreen.SetActive(false);
        optionScreen.SetActive(false);

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
        }
    }

    //When press Start button
    public void PlayPress()
    {
        SceneManager.LoadScene($"Difficult{difficulty}");

        gamePlayScreen.SetActive(true);

        startScreen.SetActive(false);
        starterSceneActive = false;

        /*
        displayPoints.gameObject.SetActive(true);
        countDown.gameObject.SetActive(true);


        foreach (var textHp in displayHpPlayers)
        {
            textHp.gameObject.SetActive(true);
        }
        */

        GameManager.GetInstance().scores = 0;
    }

    //Update time before Spawn
    public void UpdateTimeBeforeNextWave(float secounds,bool startWave)
    {
        countDown.gameObject.SetActive(true);

        if (startWave)
        {
            countDown.gameObject.SetActive(false);
        }
        else 
        {
            countDown.text = secounds.ToString();

        }
    }

    //Update Player Health
    public void UpdatePleyerHp(int player,float health)
    {
        displayHpPlayers[player].text = health.ToString();
    }

    //Update Score
    public void UpdatePoints(int point)
    {
        displayPoints.text = $"Points = {point}";
    }
}
