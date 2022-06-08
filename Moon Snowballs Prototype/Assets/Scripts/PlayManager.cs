using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayManager : MonoBehaviour
{
    static public bool restartGame;

    public TMP_Text scoreBlueText;
    public TMP_Text scoreRedText;
    public HealthController Player_0;
    public HealthController Player_1;
    public GameObject infoBar;
    public GameObject blueWin;
    public GameObject redWin;
    public GameObject pauseButton;
    public GameObject pauseBar;
    public TMP_Text scoreBlueInfoBarText;
    public TMP_Text scoreRedInfoBarText;
    public GameObject[] buttons;
    public Transform[] spawnPoints;
    public GameObject[] players;
    public float restartDelay;

    public int maxScore;

    bool gameover;

    Scene scene;

    float curretTime;
    int scorePlayer_0;
    int scorePlayer_1;

    GameObject[] shells;
    GameObject[] chips;

    Vector2[] oldPositions;
    Quaternion[] oldRotations;

    private void Start()
    {
        scene = SceneManager.GetActiveScene();
        chips = GameObject.FindGameObjectsWithTag("Floor");
        oldPositions = new Vector2[chips.Length];
        oldRotations = new Quaternion[chips.Length];
        for (int i = 0; i < chips.Length; i++)
        {
            if (chips[i].GetComponent<Chip>())
            {
                oldPositions[i] = chips[i].transform.position;
                oldRotations[i] = chips[i].transform.rotation;
            }
        }
    }

    private void Update()
    {
        scoreBlueText.text = System.Convert.ToString(scorePlayer_0);
        scoreRedText.text = System.Convert.ToString(scorePlayer_1);

        WinLoseCheckAndScoreUpdate();

        if (GameObject.FindGameObjectWithTag("Shell"))
        {
            shells = GameObject.FindGameObjectsWithTag("Shell");
        }
        if (restartGame & !gameover)
        {
            RestartGame();
        }
    }

    void WinLoseCheckAndScoreUpdate()
    {
            if (Player_0.destroyName == "Head_0")
            {
                scorePlayer_1++;
                restartGame = true;
                OpenCloseButtons(false);
                Player_0.destroyName = null;
            }
            if (Player_1.destroyName == "Head_1")
            {
                scorePlayer_0++;
                restartGame = true;
                OpenCloseButtons(false);
                Player_1.destroyName = null;
            }
        if (scorePlayer_0 >= maxScore)
        {
            gameover = true;
            scoreBlueInfoBarText.text = System.Convert.ToString(scorePlayer_0);
            scoreRedInfoBarText.text = System.Convert.ToString(scorePlayer_1);
            infoBar.SetActive(true);
            blueWin.SetActive(true);
        }
        if (scorePlayer_1 >= maxScore)
        {
            gameover = true;
            scoreBlueInfoBarText.text = System.Convert.ToString(scorePlayer_0);
            scoreRedInfoBarText.text = System.Convert.ToString(scorePlayer_1);
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
        curretTime += Time.deltaTime;
        if(curretTime >= restartDelay)
        {
            if (shells != null)
            {
                foreach (GameObject item in shells)
                {
                    Destroy(item);
                }
            }
            Respawn();
            Player_0.health = 100;
            Player_1.health = 100;
            Player_0.index = 0;
            Player_1.index = 0;
            Player_0.destroy = false;
            Player_1.destroy = false;
            OpenCloseButtons(true);
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                players[i].transform.rotation = Quaternion.identity;
                players[i].transform.position = spawnPoints[i].position;
            }
            restartGame = false;
            curretTime = 0;
        }
    }

    void Respawn()
    {
        for (int i = 0; i < chips.Length; i++)
        {
            if (chips[i].GetComponent<Chip>())
            {
                chips[i].transform.position = oldPositions[i];
                chips[i].transform.rotation = oldRotations[i];
            }
        }
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
