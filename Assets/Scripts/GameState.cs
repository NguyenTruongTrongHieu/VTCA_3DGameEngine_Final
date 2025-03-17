using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState gameStateInstance;

    public enum State
    { 
        over,
        playing,
        pause,
        cutscene
    }

    public State currentGameState;

    private void Awake()
    {
        if (gameStateInstance == null)
        {
            gameStateInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        { 
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentGameState = State.playing;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
