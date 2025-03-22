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


    }

    private void Start()
    {
        SpawnEnemies(0);
        SpawnHostages(0);

        var playerQuest = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerQuest>();
        playerQuest.totalHostages = playerQuest.hostages.Count - playerQuest.hostageKilled;
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
        var playerQuest = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerQuest>();

        int numberOfLoop = hostagePositions.Count;
        for (int i = beginLoop; i < numberOfLoop; i++)
        {
            if (SaveLoadSystem.saveLoadInstance.saveLoadInfo.indexHostages.Contains(i))//if hostage at this position is null, skip
            {
                playerQuest.hostages.Add(null);
                continue;
            }

            int randomPrefab;
            //Random a hostage prefab
            do
            {
                randomPrefab = Random.Range(0, human.Count);
            }
            while (human[randomPrefab].tag != "Hostage");

            var hostage = human[randomPrefab].SpawnEntityHavingReturnGameObject(hostagePositions[i].transform.position);
            
            playerQuest.hostages.Add(hostage);
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
            //random a enemy prefab
            do
            {
                randomPrefab = Random.Range(0, human.Count);
            }
            while (human[randomPrefab].tag != "Enemy");

            var enemy = human[randomPrefab].SpawnEntityHavingReturnGameObject(enemyPositions[i].transform.position);
            
            playerQuest.enemies.Add(enemy);
        }
    }
}
