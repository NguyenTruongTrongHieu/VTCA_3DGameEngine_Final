using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : Health
{
    [SerializeField] private Slider healthSlider;//Thanh mau
    [SerializeField] private Camera mainCamera;

    private int isDeadHash;
    private Animator animator;

    [SerializeField] private List<GameItem> dropItems = new List<GameItem> ();

    public bool alive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        animator = GetComponentInChildren<Animator>();
        isDeadHash = Animator.StringToHash("isDead");

        base.Start();
        UpdateSliderHP();
        alive = true;
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.transform.LookAt(mainCamera.transform);
    }

    public new void TakeDamage(float damage)
    { 
        base.TakeDamage(damage);
        UpdateSliderHP();
    }

    public void Die()
    {
        if (!alive)
        {
            return;
        }

        Destroy(gameObject, 1.5f);
        alive = false;
        animator.SetBool(isDeadHash, true);
        Destroy(transform.GetChild(0).gameObject);//Huy cay sung cua enemy
        Debug.Log("Enemy Die");

        //Spawn ammo
        DropItem();
    }

    private void UpdateSliderHP()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    void DropItem()
    { 
        int random = UnityEngine.Random.Range(0, 100);

        if (random <= 100)
        { 
            //drop ammo
            var item = dropItems.Find(i => i.name == "Ammo");
            item.SpawnEntities(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z));
        }
    }
}
