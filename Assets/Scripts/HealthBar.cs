using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;

    public void SetMaxHealth(int health)
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }

    public void SetHealth(int health)
    {
        healthSlider.value = health;
    }

}
