using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class HostageHealth : Health
{
    [SerializeField] private Slider healthSlider;//Thanh mau
    [SerializeField] private CinemachineFreeLookModifier cinemachineCamera;

    private int isDeadHash;
    private Animator animator;

    public bool alive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    new void Start()
    {
        cinemachineCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineFreeLookModifier>();
        animator = GetComponentInChildren<Animator>();
        isDeadHash = Animator.StringToHash("isDead");

        base.Start();
        UpdateSliderHP();
        alive = true;
    }

    // Update is called once per frame
    void Update()
    {

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
        Debug.Log("Hostage Die");
    }

    private void UpdateSliderHP()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}
