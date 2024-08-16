using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipLandScript : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;
    private PlayerHealth playerHealth;
    public AudioSource landingAnnouncement;
    public string nextLevel;
    public Animator animator;

    private bool hasTriggered = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasTriggered && playerHealth.GetCurrentHealth() >= 85)
        {
            hasTriggered = true;
            print("Landing");
            landingAnnouncement.Play();
            Invoke("FadeToLevel", 2);
            Invoke("LoadNextLevel", 5);
        }
    }

    public void FadeToLevel()
    {
        animator.SetTrigger("Transition");
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }
}
