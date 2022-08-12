using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenu;
    public void ButtonNewGame()
    {
        SceneManager.LoadScene(1);
    }
    public void ButtonOptions()
    {
        optionsMenu.SetActive(true);
    }
    public void ButtonExit()
    {
        Application.Quit();
    }
}
