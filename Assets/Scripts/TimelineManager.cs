using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    [SerializeField] private Camera timelineCamera;
    [SerializeField] private GameObject timelineRoot;
    [SerializeField] private PlayableDirector timeline;
    [SerializeField] private GameObject gameCanvas;
    [SerializeField] private GameObject pauseCanvas;

    [SerializeField] private float time;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (!SaveLoadSystem.saveLoadInstance.isLoadGame)
        {
            StartTimeline();

            Invoke("StopTimeline", time);
        }
        else
        { 
            StopTimeline();  
        }
    }

    void StartTimeline()
    {
        GameState.gameStateInstance.currentGameState = GameState.State.cutscene;
        gameCanvas.SetActive(false);
        pauseCanvas.SetActive(false);

        AudioManager.audioInstance.PlayMusic("Cutscene");
        timeline.Play();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void StopTimeline()
    {
        GameState.gameStateInstance.currentGameState = GameState.State.playing;
        gameCanvas.SetActive(true);
        pauseCanvas.SetActive(true);
        timeline.Stop();

        AudioManager.audioInstance.PlayMusic("Game");

        DestroyTimeline();
    }

    void DestroyTimeline()
    {
        Destroy(timelineRoot);
        Destroy(timelineCamera.gameObject);
    }
}
