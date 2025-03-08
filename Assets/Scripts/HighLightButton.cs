using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HighLightButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_FontAsset fontAsset;
    [SerializeField] private TMP_FontAsset fontAssetHighlight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnPointerIn()
    {
    }

    void OnPointerOut()
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        text.font = fontAssetHighlight;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        text.font = fontAsset;
    }
}
