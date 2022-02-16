using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseBtnManger : MonoBehaviour
{
    public void RestartBtn()
    {
        GameplayStatics.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenuBtn()
    {
        GameplayStatics.LoadScene("StartMenu");
    }
    
    public void QuitBtn()
    {
        GameplayStatics.CloseApplication();
    }
}
