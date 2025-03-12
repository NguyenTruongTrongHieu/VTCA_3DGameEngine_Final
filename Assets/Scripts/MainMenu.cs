using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button buttonPlay;
    [SerializeField] private Button buttonExit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.audioInstance.PlayMusic("Menu");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPlay()
    {
        SetSfxButton();
        SceneManager.LoadScene(1);
    }

    public void OnExit()
    {
        SetSfxButton();
        Application.Quit();
    }

    public void SetSfxButton()
    {
        AudioManager.audioInstance.PlaySFX("ButtonClick");
    }
}
