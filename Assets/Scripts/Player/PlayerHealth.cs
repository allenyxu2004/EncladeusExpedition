using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public AudioClip deadSFX;
    public Slider healthSlider;
    public LevelManager levelManager;

    int currentHealth;

    void Start()
    {
        currentHealth = startingHealth;
        healthSlider.value = currentHealth;
    }

    void Update()
    {

    }

    public void TakeDamage(int damageAmount)
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

    public void HealPlayer(int healAmount)
    {
        if (currentHealth < 100)
        {
            currentHealth += healAmount;
            Mathf.Clamp(currentHealth, 0, 100);
            healthSlider.value = currentHealth / startingHealth;
        }
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
