using Mono.Cecil;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class HumanSpawner : MonoBehaviour
{
    [SerializeField] private List<SpawnerHumanScriptable> human = new List<SpawnerHumanScriptable>();
    [SerializeField] private List<GameObject> enemyPositions = new List<GameObject>();
    [SerializeField] private List<GameObject> hostagePositions = new List<GameObject>();

    private void Awake()
    {
        //Find position for enemy and hostage
        //AddHostagePositons();
        //AddEnemyPositions();

        var player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent <PlayerQuest>().totalHostages = hostagePositions.Count;


        if (SaveLoadSystem.saveLoadInstance.saveLoadInfo != null && 
            SaveLoadSystem.saveLoadInstance.isLoadGame)
        {
            //Random and spawn prefab hostage
            SpawnHostages(SaveLoadSystem.saveLoadInstance.saveLoadInfo.hostageRescued + SaveLoadSystem.saveLoadInstance.saveLoadInfo.hostageKilled);
        }
        else
        {
            //Random and spawn prefab hostage
            SpawnHostages(0);
        }

    }

    private void Start()
    {
        SpawnEnemies(0);
    }

    void AddEnemyPositions()
    {
        foreach (var enemyPosition in GameObject.FindGameObjectsWithTag("EnemyPosition"))
        {
            enemyPositions.Add(enemyPosition);
            Destroy(enemyPosition);
        }
    }

    void AddHostagePositons()
    {
        foreach (var hostagePosition in GameObject.FindGameObjectsWithTag("HostagePosition"))
        {
            hostagePositions.Add(hostagePosition);
            Destroy(hostagePosition);
        }
    }

    void SpawnHostages(int beginLoop)
    {
        int numberOfLoop = hostagePositions.Count;
        for (int i = beginLoop; i < numberOfLoop; i++)
        {
            int randomPrefab;
            int randomPositon;
            //Random a hostage prefab
            do
            {
                randomPrefab = Random.Range(0, human.Count);
            }
            while (human[randomPrefab].tag != "Hostage");

            //random position
            randomPositon = Random.Range(0, hostagePositions.Count);
            human[randomPrefab].SpawnEntity(hostagePositions[randomPositon].transform.position);
            hostagePositions.RemoveAt(randomPositon);
        }
    }

    void SpawnEnemies(int beginLoop)
    {
        var playerQuest = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerQuest>();

        int numberOfLoop = enemyPositions.Count;
        for (int i = beginLoop; i < numberOfLoop; i++)
        {
            if (SaveLoadSystem.saveLoadInstance.saveLoadInfo.indexEnemies.Contains(i))//if enemy at this position is killed, skip
            {
                playerQuest.enemies.Add(null);
                continue;
            }

            int randomPrefab;
            //int randomPositon;
            //random a enemy prefab
            do
            {
                randomPrefab = Random.Range(0, human.Count);
            }
            while (human[randomPrefab].tag != "Enemy");

            //random position
            //randomPositon = Random.Range(0, enemyPositions.Count);
            var enemy = human[randomPrefab].SpawnEntityHavingReturnGameObject(enemyPositions[i].transform.position);
            
            playerQuest.enemies.Add(enemy);
        }
    }
}
