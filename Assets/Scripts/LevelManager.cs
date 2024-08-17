using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static float levelDuration = 80f;
    public Slider waveTimer;
    public TextMeshProUGUI gameText;
    //public Text scoreText;
    public Animator animator;
    public AudioSource gameOverSFX;
    public AudioSource gameWonSFX;

    public AudioSource backgroundMusic;

    public static bool isGameOver = false;

    public string nextLevel;

    public static float countDown = 0.0f;

    public float boostSpeedModifier = 1.5f;

    public int maxEnemies = 20;
    public bool spawnEnemies;

    private GameObject player;
    private PlayerHealth playerHealth;
    bool isBoosted = false;

    bool packageDelivered = false;

    GameObject[] enemySpawners;

    string currentScene;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;

        isGameOver = false;
        countDown = levelDuration;
        UpdateTimer();
        player = GameObject.FindWithTag("Player");
        if (player != null )
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
        if (backgroundMusic == null)
        {
            backgroundMusic = GameObject.FindGameObjectWithTag("BgMusic").GetComponent<AudioSource>();
            backgroundMusic.Play();
        }

        enemySpawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
        SetEnemySpawning(spawnEnemies);

        if (currentScene == "Main Menu")
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (currentScene == "Tutorial")
        {
            playerHealth.TakeDamage(50);
        }
        if (currentScene == "ShipDockBackup" || currentScene == "ShipDock")
        {
            Invoke("SpawnEnemiesTrue", 20);
        }
        if (currentScene == "ShipOcean")
        {
            Invoke("SpawnEnemiesTrue", 15);
        }


    }

    void Update()
    {
        if (currentScene == "ShipOcean")
        {
            if (!isGameOver)
            {
                if (countDown > 0)
                {
                    if (playerHealth.ShipHasEnergy())
                    {
                        HandleEnemySpawning();
                        if (isBoosted)
                        {
                            countDown -= Time.deltaTime * boostSpeedModifier;
                        }
                        else
                        {
                            countDown -= Time.deltaTime;
                        }
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


        else if (currentScene == "ShipDock" || currentScene == "ShipDockBackup")
        {
            HandleEnemySpawning();
            if (PlayerHealth.publicEnergy >= 100 && PadPressed.padPressed)
            {
                print("You Win");
                LevelBeat();
            }
        }

        else if (currentScene == "ShipBunker")
        {
            HandleEnemySpawning();
            if (packageDelivered)
            {
                LevelBeat();
            }
        }
      
    }


    void SpawnEnemiesTrue()
    {
        SetEnemySpawning(true);
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
            backgroundMusic.Stop();
            isGameOver = true;
            gameText.text = "GAME OVER!";
            gameText.gameObject.SetActive(true);

            if (gameOverSFX != null)
            {
                gameOverSFX.Play();
            }

            clearLoot();
            Invoke("FadeToLevel", 1);

            Invoke("LoadCurrentLevel", 6);
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
                gameWonSFX.Play();
            }

            clearLoot();
            Invoke("FadeToLevel", 1);

            if (!string.IsNullOrEmpty(nextLevel))
            {
                Invoke("LoadNextLevel", 5);
            }
            else
            {
                Invoke("LoadCurrentLevel", 2);
            }
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

    void LoadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void clearLoot()
    {
        GameObject[] droppedLoot = GameObject.FindGameObjectsWithTag("DroppedLoot");
        for (int i = 0; i < droppedLoot.Length; i++)
        {
            Destroy(droppedLoot[i]);
        }
    }

    public void BoostShip()
    {
        isBoosted = true;
    }

    public void SlowShip()
    {
        isBoosted = false;
    }

    void HandleEnemySpawning()
    {
        //Debug.Log("Number of Enemies: " + GameObject.FindGameObjectsWithTag("Enemy").Length + " Max Enemies: " + maxEnemies);
        if (spawnEnemies)
        {
            SetEnemySpawning(true);
            SpawnEnemiesFromSpawners();
        }
        else
        {
            SetEnemySpawning(false);
        }
    }

    void SpawnEnemiesFromSpawners()
    {
        foreach (GameObject spawner in enemySpawners)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length < maxEnemies)
                spawner.GetComponent<EnemySpawner>().SpawnEnemies();
        }
    }

    void SetEnemySpawning(bool active)
    {
        spawnEnemies = active;
        foreach (GameObject spawner in enemySpawners)
        {
            spawner.GetComponent<EnemySpawner>().SetSpawning(active);
        }
    }

    public void DeliverPackage()
    {
        packageDelivered = true;
    }
}
