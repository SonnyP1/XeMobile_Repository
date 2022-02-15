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

        Zombie[] zombiesInScene = GameObject.FindObjectsOfType<Zombie>();
        List<ZombieSavedData> zombieSavedDatas = new List<ZombieSavedData>();
        foreach(Zombie zombie in zombiesInScene)
        {
            zombieSavedDatas.Add(zombie.GeneratorSaveData());
        }
 
        SaveGameData data = new SaveGameData();
        data.LevelIndex = SceneManager.GetActiveScene().buildIndex;
        data.SaveTime = System.DateTime.Now.ToString();
        data.PlayerData = player.GenerateSaveData();
        data.ZombiesDatas = zombieSavedDatas.ToArray();

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

        //do enemy stuff
        List<Zombie> zombieList = new List<Zombie>();
        Zombie[] zombiesOnLoad = GameObject.FindObjectsOfType<Zombie>();
        for (int i = 0; i < zombiesOnLoad.Length;i++)
        {
            zombieList.Add(zombiesOnLoad[i]);
        }

        foreach(Zombie zombie in zombiesOnLoad)
        {
            foreach(ZombieSavedData zombieData in currentLoadedData.ZombiesDatas)
            {                
                if(zombie.gameObject.name == zombieData.UniqueID)
                {
                    zombie.UpdateFromSavedData(zombieData);
                    zombieList.Remove(zombie);
                }
            }
        }

        foreach(Zombie zombie in zombieList)
        {
            GameObject.Destroy(zombie.gameObject);
        }


        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    [Serializable]
    public struct SaveGameData
    {
        public SaveGameData(int levelIndex,PlayerSavedData playerData,ZombieSavedData[] zombieSavedDatas,float savedPlayerCredits,float savedPlayerHealth,float savedPlayerStamina,string savedTime)
        {
            LevelIndex = levelIndex;
            PlayerData = playerData;
            SaveTime = savedTime;
            ZombiesDatas = zombieSavedDatas;
        }

        public int LevelIndex;
        public PlayerSavedData PlayerData;
        public ZombieSavedData[] ZombiesDatas;
        public string SaveTime;
    }
}
