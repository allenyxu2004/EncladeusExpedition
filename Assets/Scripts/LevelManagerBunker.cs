using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManagerBunker : MonoBehaviour
{
    public AudioSource gameOverSFX;
    public AudioSource gameWonSFX;
    public AudioSource backgroundMusic;

    public Text gameText;

    public static bool isGameOver = false;
    public int maxEnemies = 20;
    public bool spawnEnemies;

    private GameObject player;
    private PlayerHealth playerHealth;

    GameObject[] enemySpawners;

    string currentScene;

    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;

        isGameOver = false;

        player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        if (backgroundMusic == null)
        {
            backgroundMusic = GameObject.FindGameObjectWithTag("BgMusic").GetComponent<AudioSource>();
            backgroundMusic.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleEnemySpawning();

        
    }

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
        }
    }

    void clearLoot()
    {
        GameObject[] droppedLoot = GameObject.FindGameObjectsWithTag("DroppedLoot");
        for (int i = 0; i < droppedLoot.Length; i++)
        {
            Destroy(droppedLoot[i]);
        }
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
}
