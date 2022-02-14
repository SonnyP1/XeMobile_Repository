using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public static class SaveGameManager
{
    static string SaveDir = Application.persistentDataPath + "/xeNorwerk.json";
    static SaveGameData currentLoadedData;
    static public void SaveGame()
    {
        
        Player player = GameObject.FindObjectOfType<Player>();
        if(player == null)
        {
            return;
        }
 
        SaveGameData data = new SaveGameData();
        data.LevelIndex = SceneManager.GetActiveScene().buildIndex;
        data.SaveTime = System.DateTime.Now.ToString();
        data.PlayerData = player.GenerateSaveData();

        string playerData = JsonUtility.ToJson(data,true);
        File.WriteAllText(SaveDir,playerData);
    }

    static public void LoadGame()
    {
        string savedDataString = File.ReadAllText(SaveDir);

        currentLoadedData = JsonUtility.FromJson<SaveGameData>(savedDataString);
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(currentLoadedData.LevelIndex);
    }

    private static void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Player player = GameObject.FindObjectOfType<Player>();
        if (player == null)
        {
            return;
        }
        player.UpdateFromSavedData(currentLoadedData.PlayerData);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    [Serializable]
    public struct SaveGameData
    {
        public SaveGameData(int levelIndex,PlayerSavedData playerData,float savedPlayerCredits,float savedPlayerHealth,float savedPlayerStamina,string savedTime)
        {
            LevelIndex = levelIndex;
            PlayerData = playerData;
            SaveTime = savedTime;
        }

        public int LevelIndex;
        public PlayerSavedData PlayerData;
        public string SaveTime;
    }
}
