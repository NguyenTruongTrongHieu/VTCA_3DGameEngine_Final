using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    [SerializeField] private Camera timelineCamera;
    [SerializeField] private GameObject timelineRoot;
    [SerializeField] private PlayableDirector timeline;
    [SerializeField] private GameObject GameCanvas;

    [SerializeField] private float time;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameState.gameStateInstance.currentGameState = GameState.State.cutscene;
        GameCanvas.SetActive(false);
        timeline.Play();

        Invoke("StopTimeline", time);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void StopTimeline()
    {
        GameState.gameStateInstance.currentGameState = GameState.State.playing;
        GameCanvas.SetActive(true);
        timeline.Stop();

        DestroyTimeline();
    }

    void DestroyTimeline()
    {
        Destroy(timelineRoot);
        Destroy(timelineCamera.gameObject);
    }
}
