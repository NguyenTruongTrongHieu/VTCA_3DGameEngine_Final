using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TextMeshProUGUI textContent;
    [SerializeField] private string[] content;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tutorialPanel.SetActive(true);
        textContent.text = content[0];
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(ShowContent());
    }

    IEnumerator ShowContent()
    {
        for (int i = 0; i < content.Length; i++)
        {
            textContent.text = "";
            textContent.text = content[i];
            yield return new WaitForSeconds(3f);
        }
        gameObject.SetActive(false);
    }
}
