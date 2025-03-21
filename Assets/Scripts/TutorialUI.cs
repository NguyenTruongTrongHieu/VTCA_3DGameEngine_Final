using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TextMeshProUGUI textContent;
    [SerializeField] private string[] content;

    private bool check = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tutorialPanel.SetActive(true);
        textContent.text = "";
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SaveLoadSystem.saveLoadInstance.isLoadGame)
        {
            tutorialPanel.SetActive(false);
            return;
        }
        if (check)
        {
            StartCoroutine(ShowContent());
            check = false;
        } 
    }

    IEnumerator ShowContent()
    {
        for (int i = 0; i < content.Length; i++)
        {
            textContent.text = "";
            textContent.text = content[i];
            yield return new WaitForSeconds(1.5f);
        }
        gameObject.SetActive(false);
    }
}
