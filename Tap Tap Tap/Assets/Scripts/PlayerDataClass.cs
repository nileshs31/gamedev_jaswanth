using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataClass{
    public int GameMode = 0;
    public int Difficulty = 1; // Use this in local scene in cpu mode to adjust difficulty;
    public string PlayerName = "DefaultName";
    public int UserTile = 0;
    public PlayerDataClass(int gm,int diff, int tileNum,string name) {
        this.GameMode = gm;
        this.Difficulty = diff;
        this.UserTile = tileNum;
        this.PlayerName = name;
    }
    public PlayerDataClass() { }
}
