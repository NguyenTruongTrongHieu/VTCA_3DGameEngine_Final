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
        }
        else
        {
            Destroy(gameObject);
        }

        ZPlayerPrefs.Initialize("ConCac", "concac");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadData();
        if (saveLoadInfo != null)
        {
            Debug.Log(saveLoadInfo.ToString());
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
        List<int> itemsId = new List<int>();
        foreach (var item in player.GetComponent<PlayerItem>().items) 
        {
            itemsId.Add(item.item.id);
        }
        var currentHealth = player.GetComponent<PlayerStats>().currentHealth;
        var hostageRescued = player.GetComponent<PlayerQuest>().hostageRescued;
        var enemyKilled = player.GetComponent<PlayerQuest>().enemyKilled;
        var hostageKilled = player.GetComponent<PlayerQuest>().hostageKilled;

        //Create new save load info
        saveLoadInfo = new SaveLoadInfo(gameStates, itemsId, currentHealth, playerPosition, hostageRescued, enemyKilled, hostageKilled);

        //Convert save load info to json
        string saveLoadString = JsonConvert.SerializeObject(saveLoadInfo);

        //Save to playerprefs
        PlayerPrefs.SetString("PlayerInfo", saveLoadString);
        PlayerPrefs.Save();

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

        foreach (var itemID in saveLoadInfo.itemsID)
        {
            var item = ItemsInGame.itemsInstance.items.Find(x => x.id == itemID);
            if (item != null)
            {
                player.GetComponent<PlayerItem>().PickingUpItem(item);
            }
        }
        player.GetComponent<PlayerStats>().currentHealth = saveLoadInfo.currentHealth;
        player.GetComponent<PlayerQuest>().hostageRescued = saveLoadInfo.hostageRescued;
        player.GetComponent<PlayerQuest>().enemyKilled = saveLoadInfo.enemyKilled;
        player.GetComponent<PlayerQuest>().hostageKilled = saveLoadInfo.hostageKilled;
    }
}
