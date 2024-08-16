using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public float defaultSens = 1.0f;
    public void StartGame()
    {
        // Player starts with a pistol
        PlayerPrefs.SetInt("Gun", 0);
        PlayerPrefs.SetFloat("Sensitivity", defaultSens);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
