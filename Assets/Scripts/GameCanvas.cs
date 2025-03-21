using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    private void Awake()
    {
        //Load data if player choose continue previous save
        if (SaveLoadSystem.saveLoadInstance.isLoadGame)
        {
            SaveLoadSystem.saveLoadInstance.LoadInfoToPlayer();
            Debug.Log($" {SaveLoadSystem.saveLoadInstance.saveLoadInfo.playerPosition[0]} , " +
                $"{SaveLoadSystem.saveLoadInstance.saveLoadInfo.playerPosition[1]} , " +
                $"{SaveLoadSystem.saveLoadInstance.saveLoadInfo.playerPosition[2]}");
        }
        else
        {
            SaveLoadSystem.saveLoadInstance.saveLoadInfo.indexEnemies = new List<int>();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(GameOverManager.overInstance.isOver)
        {
            gameObject.SetActive(false);
        }
    }
}
