using InfimaGames.LowPolyShooterPack;
using Newtonsoft.Json;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Networking.PlayerConnection;
using UnityEditor.Playables;
using UnityEngine;

public class SaveLoadSystem : MonoBehaviour
{
    public static SaveLoadSystem saveLoadInstance;

    public SaveLoadInfo saveLoadInfo = new SaveLoadInfo();

    public bool isLoadGame = false;//if player want to continue previous save (not over)

    private void Awake()
    {
        if (saveLoadInstance == null)
        {
            saveLoadInstance = this;
            DontDestroyOnLoad(gameObject);

            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (saveLoadInfo != null)
        {
            Debug.Log(saveLoadInfo.state.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveData(string gameStates)
    {
        //Find Player
        var player = GameObject.FindGameObjectWithTag("Player");

        //Create info from player
        float[] playerPosition = {player.transform.position.x, player.transform.position.y, player.transform.position.z };
        List<ItemInfoSaveLoad> items = new List<ItemInfoSaveLoad>();
        foreach (var item in player.GetComponent<PlayerItem>().items) 
        {
            ItemInfoSaveLoad itemSave = new ItemInfoSaveLoad(item.item.id, item.quantity);
            items.Add(itemSave);
        }
        var currentHealth = player.GetComponent<PlayerStats>().currentHealth;
        var hostageRescued = player.GetComponent<PlayerQuest>().hostageRescued;
        var enemyKilled = player.GetComponent<PlayerQuest>().enemyKilled;
        var hostageKilled = player.GetComponent<PlayerQuest>().hostageKilled;
        List<WeaponAmmo> weapons = new List<WeaponAmmo>();
        for (int i = 0; i < player.transform.GetChild(0).GetChild(0).childCount; i++)
        {
            WeaponAmmo weaponAmmo = new WeaponAmmo();
            weaponAmmo.nameWeapon = player.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Weapon>().name;
            weaponAmmo.currentAmmo = player.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Weapon>().currentAmmo;
            weaponAmmo.ammoClip = player.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Weapon>().ammoClip;
            weapons.Add(weaponAmmo);
        }

        //Create new save load info
        saveLoadInfo = new SaveLoadInfo(gameStates, items, currentHealth, playerPosition, hostageRescued, enemyKilled, hostageKilled, weapons);

        //Convert save load info to json
        string saveLoadString = JsonConvert.SerializeObject(saveLoadInfo);

        //Save to playerprefs
        PlayerPrefs.SetString("PlayerInfo", saveLoadString);
        PlayerPrefs.Save();
        Debug.Log($" {saveLoadInfo.playerPosition[0]} , {saveLoadInfo.playerPosition[1]} , {saveLoadInfo.playerPosition[2]}");
    }

    public void LoadData()
    {
        //Load from playerprefs
        var saveLoadString = PlayerPrefs.GetString("PlayerInfo");

        //Check if not have any key
        if (saveLoadString == null)
        {
            return;
        }

        //Convert json to info
        saveLoadInfo = JsonConvert.DeserializeObject<SaveLoadInfo>(saveLoadString);
    }

    public void LoadInfoToPlayer()
    {
        //Find Player
        var player = GameObject.FindGameObjectWithTag("Player");

        //Get info to player
        player.transform.position = new Vector3(saveLoadInfo.playerPosition[0], saveLoadInfo.playerPosition[1], saveLoadInfo.playerPosition[2]);

        foreach (var itemLoad in saveLoadInfo.items)
        {
            var item = ItemsInGame.itemsInstance.items.Find(x => x.id == itemLoad.itemID);
            if (item != null)
            {
                ItemInventory itemInventory = new ItemInventory{ item = item, quantity = itemLoad.quantity };
                player.GetComponent<PlayerItem>().items.Add(itemInventory);
            }
        }
        player.GetComponent<PlayerStats>().currentHealth = saveLoadInfo.currentHealth;
        player.GetComponent<PlayerQuest>().hostageRescued = saveLoadInfo.hostageRescued;
        player.GetComponent<PlayerQuest>().enemyKilled = saveLoadInfo.enemyKilled;
        player.GetComponent<PlayerQuest>().hostageKilled = saveLoadInfo.hostageKilled;
        for (int i = 0; i < saveLoadInfo.weapons.Count; i++)
        {
            WeaponAmmo weaponAmmo = saveLoadInfo.weapons[i];
            player.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Weapon>().currentAmmo = weaponAmmo.currentAmmo ;
            player.transform.GetChild(0).GetChild(0).GetChild(i).GetComponent<Weapon>().ammoClip = weaponAmmo.ammoClip;
        }
    }
}
