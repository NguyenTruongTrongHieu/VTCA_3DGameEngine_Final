using Mono.Cecil;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class HumanSpawner : MonoBehaviour
{
    [SerializeField] private List<SpawnerHumanScriptable> human = new List<SpawnerHumanScriptable>();
    [SerializeField] private List<Vector3> enemyPositions = new List<Vector3>();
    [SerializeField] private List<Vector3> hostagePositions = new List<Vector3>();

    private void Awake()
    {
        //Find position for enemy and hostage
        AddHostagePositons();
        AddEnemyPositions();

        var player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent <PlayerQuest>().totalHostages = hostagePositions.Count;

        if (SaveLoadSystem.saveLoadInstance.saveLoadInfo != null && SaveLoadSystem.saveLoadInstance.saveLoadInfo.state == "NotOver" && 
            SaveLoadSystem.saveLoadInstance.isLoadGame)
        {
            //Random and spawn prefab hostage
            SpawnHostages(SaveLoadSystem.saveLoadInstance.saveLoadInfo.hostageRescued + SaveLoadSystem.saveLoadInstance.saveLoadInfo.hostageKilled);

            //Random and spawn prefab enemy
            SpawnEnemies(SaveLoadSystem.saveLoadInstance.saveLoadInfo.enemyKilled);
        }
        else
        {
            //Random and spawn prefab hostage
            SpawnHostages(0);

            //Random and spawn prefab enemy
            SpawnEnemies(0);
        }

    }

    private void Start()
    {
    }

    void AddEnemyPositions()
    {
        foreach (var enemyPosition in GameObject.FindGameObjectsWithTag("EnemyPosition"))
        {
            enemyPositions.Add(enemyPosition.transform.position);
            Destroy(enemyPosition);
        }
    }

    void AddHostagePositons()
    {
        foreach (var hostagePosition in GameObject.FindGameObjectsWithTag("HostagePosition"))
        {
            hostagePositions.Add(hostagePosition.transform.position);
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
            human[randomPrefab].SpawnEntity(hostagePositions[randomPositon]);
            hostagePositions.RemoveAt(randomPositon);
        }
    }

    void SpawnEnemies(int beginLoop)
    {
        int numberOfLoop = enemyPositions.Count;
        for (int i = beginLoop; i < numberOfLoop; i++)
        {
            int randomPrefab;
            int randomPositon;
            //random a enemy prefab
            do
            {
                randomPrefab = Random.Range(0, human.Count);
            }
            while (human[randomPrefab].tag != "Enemy");

            //random position
            randomPositon = Random.Range(0, enemyPositions.Count);
            human[randomPrefab].SpawnEntity(enemyPositions[randomPositon]);
            enemyPositions.RemoveAt(randomPositon);
        }
    }
}
