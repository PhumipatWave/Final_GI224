using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [Header("Screen")]
    [SerializeField] private TMP_Text displayWinLose;
    [SerializeField] private GameObject panelSettings;
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameObject optionScreen;

    [Header("Button")]
    [SerializeField] private Button start;
    [SerializeField] private Button returnButton;
    [SerializeField] private Button exit;
    [SerializeField] private Button[] difficultChooseButton;

    [Header("GamePlayScreen")]
    [SerializeField] private TMP_Text[] displayHpPlayers;
    [SerializeField] private TMP_Text displayPoints;
    [SerializeField] private TMP_Text countDown;

    private void Start()
    {
        StarterScene();
    }

    //Active on Starter Screen
    public void StarterScene()
    {
        startScreen.SetActive(true);
        endScreen.SetActive(false);
        optionScreen.SetActive(false);
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

    }

    public void RestartGame()
    {

    }

    //Return starter page
    public void ReturnStarter()
    {
        StarterScene();
    }

    void SetEndScreen(bool win)
    {
        endScreen.SetActive(true);
        startScreen.SetActive(false);
        optionScreen.SetActive(true);

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

    public void PlayPress()
    {
        displayPoints.gameObject.SetActive(true);
        countDown.gameObject.SetActive(true);

        GameManager.GetInstance().scores = 0;
    }
}
