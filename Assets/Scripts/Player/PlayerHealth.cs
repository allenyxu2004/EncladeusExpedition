using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float startingHealth = 100;
    public float startingEnergy = 100;
    public float energyUsedPerSecond = 1.5f;
    public AudioClip deadSFX;
    public Slider healthSlider;
    public Slider energySlider;
    public LevelManager levelManager;
    float currentHealth;
    float currentEnergy;

    void Start()
    {
        currentHealth = startingHealth;
        currentEnergy = startingEnergy;
        healthSlider.value = currentHealth;
    }

    void Update()
    {
        currentEnergy -= energyUsedPerSecond * Time.deltaTime;
        energySlider.value = currentEnergy / startingEnergy;
    }

    public void TakeDamage(float damageAmount)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damageAmount;
            healthSlider.value = currentHealth;
        }
        if (currentHealth <= 0)
        {
            PlayerDies();
            Mathf.Clamp(currentHealth, 0, 100);
        }
        healthSlider.value = (float)currentHealth / (float)startingHealth;
        Debug.Log("Current Health: " + currentHealth);
    }

    public void HealPlayer(float healAmount)
    {
        if (currentHealth < 100 && currentEnergy > 0)
        {
            currentHealth += healAmount;
            currentEnergy -= healAmount;
            Mathf.Clamp(currentHealth, 0, 100);
            Mathf.Clamp(currentEnergy, 0, 100);
            healthSlider.value = currentHealth / startingHealth;
        }
    }

    public bool ShipHasEnergy()
    {
        return currentEnergy > 0;
    }

    void PlayerDies()
    {
        Debug.Log("Player is dead...");
        if (deadSFX != null)
        {
            AudioSource.PlayClipAtPoint(deadSFX, transform.position);
        }
        levelManager.LevelLost();
        //transform.Rotate(-90, 0, 0, Space.Self);
    }
}
