using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] public enum MenuStates { main, game }
    [SerializeField] public MenuStates CurrentState;

    private GameObject StartMenuParent;
    private GameObject SettingsMenuParent;

    private Animator mainMenuAnimator;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        CurrentState = MenuStates.main;
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (mainMenuAnimator == null) { mainMenuAnimator = this.GetComponent<Animator>(); }
        if (StartMenuParent == null) { StartMenuParent = GameObject.FindGameObjectWithTag("StartSubMenu"); }
        if (SettingsMenuParent == null) { SettingsMenuParent = GameObject.FindGameObjectWithTag("SettingsSubMenu"); }
    }

    void Update()
    {
        switch (CurrentState)
        {
            case MenuStates.main:
                StartMenuParent.SetActive(true);
                break;

            case MenuStates.game:
                StartMenuParent.SetActive(false);
                SettingsMenu(2);
                break;
        }
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        { 
            mainMenuAnimator.Play("FadeIn");
            CurrentState = MenuStates.game;
            
        }
    }

    public void StartGame()
    {
        mainMenuAnimator.Play("FadeOut");
    }

    public void SettingsMenu(int state) //STATES: 0 = main menu, 1 = in game 2 = closed
    {
        switch (state)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;

        }
    }

    public void GameOver()
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
