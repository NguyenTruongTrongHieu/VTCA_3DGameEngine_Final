using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 100;

    public HealthBar healthBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (SaveLoadSystem.saveLoadInstance.isLoadGame)
        {
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetHealth(currentHealth);
        }
        else
        {
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
            
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Player died");
        GameOverManager.overInstance.UpdateInfo(gameObject.GetComponent<PlayerQuest>().hostageRescued, 
            gameObject.GetComponent<PlayerQuest>().enemyKilled, gameObject.GetComponent<PlayerQuest>().hostageKilled);
        GameOverManager.overInstance.Lose();
    }
}
