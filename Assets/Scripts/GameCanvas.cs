using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Load data if player choose continue previous save
        if (SaveLoadSystem.saveLoadInstance.isLoadGame)
        {
            SaveLoadSystem.saveLoadInstance.LoadInfoToPlayer();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GameOverManager.overInstance.isOver)
        {
            gameObject.SetActive(false);
        }
    }
}
