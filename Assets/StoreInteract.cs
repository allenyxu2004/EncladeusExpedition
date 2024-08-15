using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreInteract : MonoBehaviour, Interactable
{

    public GameObject shopUI;
    public GameObject interactUI;
    bool defaultShopState = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Press E
    public void Interact(GameObject interacting)
    {
        //npcSpeak.SayDialogue("The ship needs energy. Put monster meat in the incinerator to charge the ship.", interactSFX, true);
        //StartCoroutine(npcSpeak.SayMultiLineDialogue(interactTextLines, interactAudioLines));
        defaultShopState = !defaultShopState;
        shopUI.SetActive(defaultShopState);
        if (defaultShopState)
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }
        else
        {
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

    }

    public Transform GetTransform()
    {
        return transform;
    }

    public string GetTextFromInteract()
    {
        return "Talk to Robot";
    }

    // UI Pop up
    public void SetUI(bool active)
    {
        interactUI.SetActive(active);
    }
}
