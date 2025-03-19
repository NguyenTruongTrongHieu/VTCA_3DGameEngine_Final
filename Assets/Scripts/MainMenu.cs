using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button buttonPlay;
    [SerializeField] private Button buttonExit;
    [SerializeField] private GameObject loadGame;
    [SerializeField] private Button buttonContinue;
    [SerializeField] private Button buttonAcceptContinue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.audioInstance.PlayMusic("Menu");
        GameState.gameStateInstance.currentGameState = GameState.State.playing;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPlay()
    {
        SetSfxButton();
        SaveLoadSystem.saveLoadInstance.isLoadGame = false;
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

    public void LoadGame()
    {
        if (SaveLoadSystem.saveLoadInstance.saveLoadInfo != null)
        {
            if (SaveLoadSystem.saveLoadInstance.saveLoadInfo.state == "NotOver")
            {
                loadGame.SetActive(true);
            }
        }

        SetSfxButton();
    }

    public void AcceptLoadGame()
    {
        SetSfxButton();
        SaveLoadSystem.saveLoadInstance.isLoadGame = true;
        SceneManager.LoadScene(1);
    }
}
