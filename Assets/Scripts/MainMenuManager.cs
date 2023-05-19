using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void SettingsMenu(bool state)
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
