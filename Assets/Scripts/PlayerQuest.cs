using System;
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
            if (hostage.GetComponent<Hostage>().isRescue)
            {
                hostageRescued++;
                hostages.Remove(hostage);
            }
        }
    }

    void SetUpQuestText()
    {
        hostageRescuedText.text = $"Hostages saved: {hostageRescued}/{totalHostages}";
        enemyKilledText.text = $"Enemy killed: {enemyKilled}";
    }
}
