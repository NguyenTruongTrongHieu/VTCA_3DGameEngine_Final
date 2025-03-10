using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRescue : MonoBehaviour
{
    [SerializeField] private float rescueDistance = 2f;
    [SerializeField] private Button rescueButton;
    [SerializeField] private List<GameObject> hostages;
    private bool canRescue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rescueButton = GameObject.FindWithTag("RescueButton").GetComponent<Button>();
        rescueButton.gameObject.SetActive(false);

        GameObject.FindGameObjectsWithTag("Hostage", hostages);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject hostage in hostages) 
        {
            if (Vector3.Distance(transform.position, hostage.transform.position) <= rescueDistance)
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
