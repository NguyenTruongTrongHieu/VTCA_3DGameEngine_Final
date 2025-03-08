using UnityEngine;
using UnityEngine.EventSystems;

public class TurnOffPanelPlay : MonoBehaviour, IPointerClickHandler
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //gameObject.SetActive(false);
    }
}
