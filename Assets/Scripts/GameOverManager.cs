using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager overInstance;

    public TextMeshProUGUI titleText;
    public Button returnMenu;
    public Button replay;
    public GameObject gameOverPanel;

    public TextMeshProUGUI hostageRescuedText;
    public TextMeshProUGUI enemyKilledText;
    public TextMeshProUGUI hostageKilledText;

    public bool isOver;

    private void Awake()
    {
        overInstance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        returnMenu.onClick.AddListener(ReturnMenu);
        replay.onClick.AddListener(Replay);

        isOver = false;
        gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateInfo(int hostageRescued, int enemyKilled, int hostageKilled)
    {
        hostageRescuedText.text = $"Rescued: {hostageRescued.ToString()} hostages";
        enemyKilledText.text = $"Killed: {enemyKilled.ToString()} enemmies";
        hostageKilledText.text = $"Dead hostages: {hostageKilled.ToString()}";
    }

    public void Win()
    {
        //Save game
        SaveLoadSystem.saveLoadInstance.SaveData("Win");

        isOver = true;
        titleText.text = "You Win";
        replay.gameObject.SetActive(false);
        gameOverPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        GameState.gameStateInstance.currentGameState = GameState.State.over;
    }

    public void Lose()
    {
        //Save game
        SaveLoadSystem.saveLoadInstance.SaveData("Lose");

        isOver = true;
        titleText.text = "You Lose";
        replay.gameObject.SetActive(true);
        gameOverPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        GameState.gameStateInstance.currentGameState = GameState.State.over;
    }

    public void ReturnMenu()
    {
        SetSfxButton();
        SceneManager.LoadScene(0);
    }

    public void Replay()
    {
        SetSfxButton();

        GameState.gameStateInstance.currentGameState = GameState.State.playing;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetSfxButton()
    {
        AudioManager.audioInstance.PlaySFX("ButtonClick");
    }
}
