using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveButtonScript : MonoBehaviour
{

    public void SaveGame()
    {
        SaveGameManager.SaveGame();
    }
    public void LoadSaveGame()
    {
        SaveGameManager.LoadGame();
    }
}
