using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManger : MonoBehaviour
{
    public void StartGame()
    {
        GameplayStatics.LoadScene("Level_One");
    }

    public void QuitGame()
    {
        GameplayStatics.CloseApplication();
    }

    public void LoadGame()
    {
        SaveGameManager.LoadGame();
    }
}
