using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRescue : MonoBehaviour
{
    [SerializeField] private float rescueDistance = 2f;
    [SerializeField] private Button rescueButton;
    [SerializeField] private List<GameObject> hostages;
    private bool isRescue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rescueButton = GameObject.FindWithTag("RescueButton").GetComponent<Button>();
        rescueButton.gameObject.SetActive(false);
        isRescue = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.gameStateInstance.currentGameState != GameState.State.playing)
        {
            return;
        }

        GameObject.FindGameObjectsWithTag("Hostage", hostages);
        foreach (GameObject hostage in hostages) 
        {
            if (Vector3.Distance(transform.position, hostage.transform.position) <= rescueDistance && !hostage.GetComponent<Hostage>().isRescue)// && !hostage.GetComponent<Hostage>().isRescue
            {
                rescueButton.gameObject.SetActive(true);
                break;
            }
            else
            {
                rescueButton.gameObject.SetActive(false);
            }
        }
    }

    void TurnButton(Transform target)
    {
        if (Vector3.Distance(transform.position, target.position) <= rescueDistance)
        {
            rescueButton.gameObject.SetActive(true);
        }
        else
        {
            rescueButton.gameObject.SetActive(false);
        }
    }
}
