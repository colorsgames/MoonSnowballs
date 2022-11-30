using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainButtons, mpButtons;

    public void TwoPlayers()
    {
        SceneManager.LoadScene(1);
    }

    public void Multiplayer()
    {
        //SceneManager.LoadScene(2);
    }

    public void OnePlayers()
    {
        SceneManager.LoadScene(3);
    }

    public void SetMpButton(bool value)
    {
        mainButtons.SetActive(!value);
        mpButtons.SetActive(value);
    }
}
