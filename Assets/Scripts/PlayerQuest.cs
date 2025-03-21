using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerQuest : MonoBehaviour
{
    public List<GameObject> hostages = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();

    public int totalHostages;
    public int hostageRescued;
    public int enemyKilled;
    public int hostageKilled;

    [SerializeField] private TextMeshProUGUI hostageRescuedText;
    [SerializeField] private TextMeshProUGUI enemyKilledText;
    [SerializeField] private TextMeshProUGUI alertText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AddHostages();
        SetUpQuestText();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.gameStateInstance.currentGameState != GameState.State.playing)
        {
            return;
        }

        SetUpEnemyKilled();
        SetUpHostageRescued();

        SetUpQuestText();
    }

    void AddEnemies()
    {
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Add(enemy);
        }
    }

    void AddHostages()
    {
        foreach (var hostage in GameObject.FindGameObjectsWithTag("Hostage"))
        {
            hostages.Add(hostage);
        }
    }

    void SetUpEnemyKilled()
    {
        List<int> indexInList = new List<int>();
        int index = 0;

        foreach (var enemy in enemies)
        {
            if (enemy == null)
            {
                if (!SaveLoadSystem.saveLoadInstance.saveLoadInfo.indexEnemies.Contains(index))
                {
                    SaveLoadSystem.saveLoadInstance.saveLoadInfo.indexEnemies.Add(index);
                    indexInList.Add(index);
                    enemyKilled++;
                }
            }

            index++;
        }

        //Delete from list
        for (int i = 0; i < indexInList.Count; i++)
        {
            //enemies.RemoveAt(indexInList[i]);
        }
    }

    void SetUpHostageRescued()
    {
        List<int> indexInList = new List<int>();
        int index = 0;

        foreach (var hostage in hostages)
        {
            if (hostage == null)
            {
                hostageKilled++;

                //if 3 hostages have been killed => lose
                if (hostageKilled >= 3)
                {
                    GameOverManager.overInstance.UpdateInfo(hostageRescued, enemyKilled, hostageKilled);
                    GameOverManager.overInstance.Lose();
                    //GameOverManager.overInstance.gameOverPanel.SetActive(true);
                }

                StartCoroutine(SetUpHostagesKilled());

                indexInList.Add(index);
                //hostages.Remove(hostage);
                continue;
            }

            if (hostage.GetComponent<Hostage>().isRescue)
            {
                hostageRescued++;

                //if all hostages have been rescued => win
                if (hostageRescued >= totalHostages)
                {
                    GameOverManager.overInstance.UpdateInfo(hostageRescued, enemyKilled, hostageKilled);
                    GameOverManager.overInstance.Win();
                    //GameOverManager.overInstance.gameOverPanel.SetActive(true);
                }

                indexInList.Add(index);
                //hostages.Remove(hostage);
            }

            index++;
        }

        //Delete from list
        for (int i = 0; i < indexInList.Count; i++)
        {
            hostages.RemoveAt(indexInList[i]);
        }
    }

    void SetUpQuestText()
    {
        hostageRescuedText.text = $"Hostages saved: {hostageRescued}/{totalHostages}";
        enemyKilledText.text = $"Enemy killed: {enemyKilled}";
        Debug.Log(enemyKilled);
    }

    IEnumerator SetUpHostagesKilled()
    {
        if (!alertText.gameObject.activeSelf)
        {
            alertText.text = $"{hostageKilled} hostages have been killed";

            alertText.gameObject.SetActive(true);
            alertText.transform.localPosition = Vector3.zero;

            while (alertText.transform.localPosition.y < 280f)
            {
                alertText.transform.localPosition += new Vector3(0, Time.deltaTime * 800, 0);
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);
            alertText.gameObject.SetActive(false);
        }
        yield return null;
    }
}
