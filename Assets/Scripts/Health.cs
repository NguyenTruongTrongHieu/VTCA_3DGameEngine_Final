using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    [SerializeField] private float healingRate;
    [SerializeField] private float timeWaitForHealing = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Tru mau khi nhan sat thuong
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
    }

    //hoi mau nho binh mau hoac chieu thuc
    public void TakeHealing(float heal)
    {
        currentHealth += heal;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }

    //hoi mau tu dong
    public IEnumerator Healing()
    {
        while (currentHealth / maxHealth * 100 <= 25)
        {
            //Moi 1 giay hoi mau 1 lan
            currentHealth += healingRate * Time.deltaTime;
            //Neu mau hoi vuot qua maxHealth thi dung lai
            currentHealth = Mathf.Min(currentHealth, maxHealth);
            yield return new WaitForSeconds(timeWaitForHealing);
        }
    }
}
