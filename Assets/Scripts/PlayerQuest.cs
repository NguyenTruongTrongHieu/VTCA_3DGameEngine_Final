using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerQuest : MonoBehaviour
{
    [SerializeField] private List<GameObject> hostages = new List<GameObject>();
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();

    [SerializeField] private int totalHostages;
    [SerializeField] private int hostageRescued;
    [SerializeField] private int enemyKilled;
    [SerializeField] private int hostageKilled;

    [SerializeField] private TextMeshProUGUI hostageRescuedText;
    [SerializeField] private TextMeshProUGUI enemyKilledText;
    [SerializeField] private TextMeshProUGUI alertText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AddEnemies();
        AddHostages();

        totalHostages = hostages.Count;
    }

    // Update is called once per frame
    void Update()
    {
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
        foreach (var enemy in enemies)
        {
            if (enemy == null)
            {
                enemyKilled++;
                enemies.Remove(enemy);
            }
        }
    }

    void SetUpHostageRescued()
    {
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
                    GameOverManager.overInstance.gameOverPanel.SetActive(true);
                }

                StartCoroutine(SetUpHostagesKilled());
                hostages.Remove(hostage);
                continue;
            }

            if (hostage.GetComponent<Hostage>().isRescue)
            {
                hostageRescued++;

                //if all hostages have been rescued => win
                if (hostageKilled >= 3)
                {
                    GameOverManager.overInstance.UpdateInfo(hostageRescued, enemyKilled, hostageKilled);
                    GameOverManager.overInstance.Win();
                    GameOverManager.overInstance.gameOverPanel.SetActive(true);
                }

                hostages.Remove(hostage);
            }
        }
    }

    void SetUpQuestText()
    {
        hostageRescuedText.text = $"Hostages saved: {hostageRescued}:{totalHostages}";
        enemyKilledText.text = $"Enemy killed: {enemyKilled}";
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

            yield return new WaitForSeconds(2f);
            alertText.gameObject.SetActive(false);
        }
        yield return null;
    }
}
