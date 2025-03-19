using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;

    public Button returnMenu;
    public Button continuePlaying;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        returnMenu.onClick.AddListener(ReturnMenu);
        continuePlaying.onClick.AddListener(Continue);

        pausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Pause()
    {
        SetSfxButton();
        Cursor.lockState = CursorLockMode.None;

        //Save game
        SaveLoadSystem.saveLoadInstance.SaveData("Not Over");

        GameState.gameStateInstance.currentGameState = GameState.State.pause;
        pausePanel.SetActive(true);
    }

    public void Continue()
    {
        SetSfxButton();
        Cursor.lockState = CursorLockMode.Locked;
        GameState.gameStateInstance.currentGameState = GameState.State.playing;
        pausePanel.SetActive(false);
    }

    public void ReturnMenu()
    {
        SetSfxButton();
        SceneManager.LoadScene(0);
    }

    public void SetSfxButton()
    {
        AudioManager.audioInstance.PlaySFX("ButtonClick");
    }
}
