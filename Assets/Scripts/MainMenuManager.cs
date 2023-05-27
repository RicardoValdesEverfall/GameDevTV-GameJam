using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager InstanceOfMM { get { return _instance; } }
    private static MainMenuManager _instance;

    [SerializeField] public enum MenuStates { main, game }
    [SerializeField] public MenuStates CurrentState;

    [SerializeField] private GameObject StartMenuParent;
    [SerializeField] private GameObject SettingsMenuParent;

    private Animator mainMenuAnimator;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else { _instance = this; }
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        CurrentState = MenuStates.main;
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (mainMenuAnimator == null) { mainMenuAnimator = this.GetComponent<Animator>(); }
        if (StartMenuParent == null) { StartMenuParent = GameObject.FindGameObjectWithTag("StartSubMenu"); }
        if (SettingsMenuParent == null) { SettingsMenuParent = GameObject.FindGameObjectWithTag("SettingsSubMenu"); }
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case MenuStates.main:
                StartMenuParent.SetActive(true);
                break;

            case MenuStates.game:
                StartMenuParent.SetActive(false);
               // SettingsMenu(2);
                break;
        }
    }

    public void LoadGameScene(int index)
    {
        SceneManager.LoadScene(index);
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
        mainMenuAnimator.Play("GameOver");
        
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
