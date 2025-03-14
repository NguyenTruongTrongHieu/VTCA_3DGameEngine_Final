using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ItemInfoSaveLoad
{
    public int itemID;
    public int quantity;

    public ItemInfoSaveLoad(int itemID, int quantity)
    {
        this.itemID = itemID;
        this.quantity = quantity;
    }
}

public class SaveLoadInfo 
{
    //game states
    public string state;

    //item
    public List<ItemInfoSaveLoad> items = new List<ItemInfoSaveLoad>();

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

    public SaveLoadInfo(string state, List<ItemInfoSaveLoad> items, int currentHealth, float[] playerPosition, int hostageRescued, int enemyKilled, int hostageKilled)
    {
        this.state = state;
        this.items = items;
        this.currentHealth = currentHealth;
        this.playerPosition = playerPosition;
        this.hostageRescued = hostageRescued;
        this.enemyKilled = enemyKilled;
        this.hostageKilled = hostageKilled;
    }
}
