using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    static public bool restartGame;

    [SerializeField] private TMP_Text scoreBlueText;
    [SerializeField] private TMP_Text scoreRedText;
    [SerializeField] private HealthController playerBlue;
    [SerializeField] private HealthController playerRed;
    [SerializeField] private GameObject infoBar;
    [SerializeField] private GameObject blueWin;
    [SerializeField] private GameObject redWin;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject pauseBar;
    [SerializeField] private TMP_Text scoreBlueInfoBarText;
    [SerializeField] private TMP_Text scoreRedInfoBarText;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private float restartDelay;

    [SerializeField] private int maxScore;

    [SerializeField] private IntValue scorePlayerBlue;
    [SerializeField] private IntValue scorePlayerRed;

    bool gameover;

    Scene scene;

    float curretTime;


    private void Start()
    {
        scene = SceneManager.GetActiveScene();
    }

    private void Update()
    {
        scoreBlueText.text = System.Convert.ToString(scorePlayerBlue.value);
        scoreRedText.text = System.Convert.ToString(scorePlayerRed.value);

        WinLoseCheckAndScoreUpdate();
        if (restartGame & !gameover)
        {
            curretTime += Time.deltaTime;
            if (curretTime >= restartDelay)
            {
                RestartGame();
                restartGame = false;
                curretTime = 0;
            }
        }
    }

    void WinLoseCheckAndScoreUpdate()
    {
        if (playerBlue.destroyName == "Head_0")
        {
            scorePlayerRed.value++;
            restartGame = true;
            OpenCloseButtons(false);
            playerBlue.destroyName = null;
        }
        if (playerRed.destroyName == "Head_1")
        {
            scorePlayerBlue.value++;
            restartGame = true;
            OpenCloseButtons(false);
            playerRed.destroyName = null;
        }
        if (scorePlayerBlue.value >= maxScore)
        {
            gameover = true;
            scoreBlueInfoBarText.text = System.Convert.ToString(scorePlayerBlue.value);
            scoreRedInfoBarText.text = System.Convert.ToString(scorePlayerRed.value);
            infoBar.SetActive(true);
            blueWin.SetActive(true);
        }
        if (scorePlayerRed.value >= maxScore)
        {
            gameover = true;
            scoreBlueInfoBarText.text = System.Convert.ToString(scorePlayerBlue.value);
            scoreRedInfoBarText.text = System.Convert.ToString(scorePlayerRed.value);
            infoBar.SetActive(true);
            redWin.SetActive(true);
        }
    }

    void OpenCloseButtons(bool active)
    {
        foreach (GameObject item in buttons)
        {
            item.SetActive(active);
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(scene.buildIndex);
        /*
        if (shells != null)
        {
            foreach (GameObject item in shells)
            {
                Destroy(item);
            }
        }
        Respawn();
        playerBlue.health = 100;
        playerRed.health = 100;
        playerBlue.index = 0;
        playerRed.index = 0;
        playerBlue.destroy = false;
        playerRed.destroy = false;
        OpenCloseButtons(true);
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            players[i].transform.rotation = Quaternion.identity;
            players[i].transform.position = spawnPoints[i].position;
        }
        */
    }


    public void Pause()
    {
        pauseBar.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        pauseBar.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1;
    }

    public void RestartScene()
    {
        Time.timeScale = 1;
        restartGame = false;
        SceneManager.LoadScene(scene.buildIndex);
    }

    public void ExitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

}
