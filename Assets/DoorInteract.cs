using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteract : MonoBehaviour, Interactable
{
    //public AudioClip interactSFX;
    public GameObject interactUI;

    LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.FindObjectOfType<LevelManager>();
    }


    public void Interact(GameObject interacting)
    {
        levelManager.DeliverPackage();
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public string GetTextFromInteract()
    {
        return "Deliver Cargo";
    }

    public void SetUI(bool active)
    {
        interactUI.SetActive(active);
    }
}
