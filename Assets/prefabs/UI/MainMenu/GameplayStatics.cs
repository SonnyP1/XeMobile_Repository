using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameplayStatics
{
    public static void LoadScene(string levelToLoad)
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public static void CloseApplication()
    {
        Application.Quit();
    }

    public static void PauseGame()
    {
        Time.timeScale = 0;
    }
    public static void UnPauseGame()
    {
        Time.timeScale = 1;
    }
}
