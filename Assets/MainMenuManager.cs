using GameSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject mainMenu;
    private InputHandler inputHandler;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //GameScene Logic
            if (SceneManager.GetActiveScene().name != "MainMenu" && !optionsMenu.activeSelf)
            {
                if (inputHandler == null)
                {
                    inputHandler = GameObject.Find("Player").GetComponent<InputHandler>();
                }
                mainMenu.SetActive(!mainMenu.activeSelf);
                if (mainMenu.activeSelf)
                {
                    Time.timeScale = 0;
                    inputHandler.gamePaused = true;
                }
                else
                {
                    Time.timeScale = 1;
                    inputHandler.gamePaused = false;
                }
            }
            else if (optionsMenu.activeSelf)
            {
                optionsMenu.SetActive(false);
            }
        }
    }
    public void ButtonNewGame()
    {
        SceneManager.LoadScene(1);
        if(inputHandler != null)
            inputHandler.gamePaused = false;
        mainMenu.SetActive(false);
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
