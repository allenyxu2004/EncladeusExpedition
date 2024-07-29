using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static float levelDuration = 40f;
    public Slider waveTimer;
    public Text gameText;
    //public Text scoreText;

    public AudioClip gameOverSFX;
    public AudioClip gameWonSFX;
    public AudioSource hitSFX;

    public static bool isGameOver = false;

    public string nextLevel;

    public static float countDown = 0.0f;

    private GameObject player;
    private PlayerHealth playerHealth;


    void Start()
    {
        isGameOver = false;
        countDown = levelDuration;
        UpdateTimer();
        player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    public void hurtSound()
    {
        hitSFX.Play();
    }

    void Update()
    {
        if (!isGameOver)
        {
            if (countDown > 0 )
            {
                if (playerHealth.ShipHasEnergy())
                {
                    countDown -= Time.deltaTime;
                }
            }
            else
            {
                countDown = 0.0f;
                LevelBeat();
            }
            UpdateTimer();
        }
       
    }

    private void OnGUI()
    {
        // GUI.Box(new Rect(10, 10, 40, 20), countDown.ToString("0.00"));
    }

    void UpdateTimer()
    {
        waveTimer.value = countDown / levelDuration;
    }

    //public void SetScoreText(int score)
    //{
    //    scoreText.text = "SCORE: " + score.ToString();
    //}

    public void LevelLost()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            gameText.text = "GAME OVER!";
            gameText.gameObject.SetActive(true);

            if (gameOverSFX != null)
            {
                AudioSource.PlayClipAtPoint(gameOverSFX, Camera.main.transform.position);
            }

            Invoke("LoadCurrentLevel", 2);
        }
    }

    public void LevelBeat()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            gameText.text = "YOU WIN!";
            gameText.gameObject.SetActive(true);

            if (gameWonSFX != null)
            {
                AudioSource.PlayClipAtPoint(gameWonSFX, Camera.main.transform.position);
            }

            if (!string.IsNullOrEmpty(nextLevel))
            {
                Invoke("LoadNextLevel", 2);
            }
            else
            {
                Invoke("LoadCurrentLevel", 2);
            }
        }
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }

    void LoadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
