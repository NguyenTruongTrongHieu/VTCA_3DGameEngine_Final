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

public class WeaponAmmo
{
    public string nameWeapon;
    public int currentAmmo;
    public int ammoClip;
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

    //ammo
    public List<WeaponAmmo> weapons = new List<WeaponAmmo>();



    public SaveLoadInfo()
    {
        this.state = "None";
        this.items = new List<ItemInfoSaveLoad>();
        this.currentHealth = 100;
        this.playerPosition = new float[3];
        this.hostageRescued = 0;
        this.enemyKilled = 0;
        this.hostageKilled = 0;
        this.weapons = new List<WeaponAmmo>();
    }

    public SaveLoadInfo(string state, List<ItemInfoSaveLoad> items, int currentHealth, float[] playerPosition, int hostageRescued, int enemyKilled, int hostageKilled, List<WeaponAmmo> weapons)
    {
        this.state = state;
        this.items = items;
        this.currentHealth = currentHealth;
        this.playerPosition = playerPosition;
        this.hostageRescued = hostageRescued;
        this.enemyKilled = enemyKilled;
        this.hostageKilled = hostageKilled;
        this.weapons = weapons;
    }
}
