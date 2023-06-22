using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GlobalDataHandler : MonoBehaviour {
    public static GlobalDataHandler Instance {
        get;
        set;
    }

    private void Awake() {
        Debug.Log("Script : GlobalDataHandler");

        DontDestroyOnLoad(transform.gameObject);
        Instance = this;
    }

    private void Start() {
        loadData();
        Debug.Log("Loading menu now ...");
        //SceneManager.LoadScene(1);
    }

    // Add Data here
    public int gameMode = 0;
    public int difficulty = 1; // Use this in local scene in cpu mode to adjust difficulty;
    public string playerName = "DefaultName";

    //TODO: Need designer to give the UserTile assests
    //      Details : tiles_resolution - 9:16 (as most mobile divices are)
    //                256-1080 pixel range.
    //                ...
    public Sprite[] tiles; // get the tiles from assets.

    // use this int to change the tile color in all the scenes
    // alternatively one could add some purchasing of tiles thing and unlock them.
    // although then we need to setup up players login and data in server.
    public int userTile = 0; // set 0th sprite to blue (default).
    public bool sound = false;

    // save data offline
    public void saveData() {
        try {
            string playerData = JsonUtility.ToJson(this.convertThis());
            Debug.Log(Application.persistentDataPath);
            System.IO.File.WriteAllText(Application.persistentDataPath + "/playerData.json", playerData);
        }
        catch (ArgumentException e) {
            Debug.Log(e);
        }
    }

    // load data from file;
    public void loadData() {
        //GlobalDataHandler temp = JsonUtility.FromJson<GlobalDataHandler>(path);
        //this.difficulty = temp.difficulty;
        //this.playerName = temp.playerName;
        //this.gameMode = temp.gameMode;
        //this.userTile = temp.userTile;
        //Destroy(temp);
        try {
            string playerData = System.IO.File.ReadAllText(Application.persistentDataPath + "/playerData.json");
            PlayerDataClass pdc = JsonUtility.FromJson<PlayerDataClass>(playerData);
            this.playerName = pdc.PlayerName;
            this.userTile = pdc.UserTile;
            this.gameMode = pdc.GameMode;
            this.difficulty = pdc.Difficulty;
            this.sound = pdc.Sound;
        }
        catch (ArgumentException e) {
            Debug.Log(e);
        }
    }

    private void OnApplicationQuit() {
        saveData();
    }

    private PlayerDataClass convertThis() {
        return new PlayerDataClass(this.gameMode, this.difficulty, this.userTile, this.playerName, this.sound);
    }
}
