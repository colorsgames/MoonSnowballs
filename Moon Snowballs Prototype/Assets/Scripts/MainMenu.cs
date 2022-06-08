using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void TwoPlayers()
    {
        SceneManager.LoadScene(1);
    }

    public void OnePlayers()
    {
        SceneManager.LoadScene(2);
    }
}
