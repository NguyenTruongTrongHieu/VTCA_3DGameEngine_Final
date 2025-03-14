using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SaveLoadInfo 
{
    //game states
    public string state;

    //item
    public List<int> itemsID = new List<int>();

    //Health
    public int currentHealth;

    //position
    public float[] playerPosition = new float [3];

    //quest
    public int hostageRescued;
    public int enemyKilled;
    public int hostageKilled;

    public SaveLoadInfo()
    { 
        
    }

    public SaveLoadInfo(string state, List<int> items, int currentHealth, float[] playerPosition, int hostageRescued, int enemyKilled, int hostageKilled)
    {
        this.state = state;
        this.itemsID = items;
        this.currentHealth = currentHealth;
        this.playerPosition = playerPosition;
        this.hostageRescued = hostageRescued;
        this.enemyKilled = enemyKilled;
        this.hostageKilled = hostageKilled;
    }
}
