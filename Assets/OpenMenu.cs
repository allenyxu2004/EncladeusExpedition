using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMenu : MonoBehaviour
{
    public GameObject[] oldButtons;
    public GameObject[] newButtons;

    bool newMenuOpen = false;

    public void changeMenu()
    {
        if (newMenuOpen)
        {
            foreach (GameObject button in oldButtons)
            {
                button.SetActive(true);
            }
            foreach (GameObject button in newButtons)
            {
                button.SetActive(false);
            }
            newMenuOpen = false;
        }
        else
        {
            foreach (GameObject button in oldButtons)
            {
                button.SetActive(false);
            }
            foreach (GameObject button in newButtons)
            {
                button.SetActive(true);
            }
            newMenuOpen = true;
        }
    }
}
