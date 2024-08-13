using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, Interactable
{
    //public AudioClip interactSFX;
    public GameObject interactUI;
    public string[] interactTextLines;
    public AudioClip[] interactAudioLines;

    NPCSpeak npcSpeak;

    // Start is called before the first frame update
    void Start()
    {
        npcSpeak = GetComponent<NPCSpeak>();
    }


    public void Interact(GameObject interacting)
    {
        //npcSpeak.SayDialogue("The ship needs energy. Put monster meat in the incinerator to charge the ship.", interactSFX, true);
        StartCoroutine(npcSpeak.SayMultiLineDialogue(interactTextLines, interactAudioLines));
     }

    public Transform GetTransform()
    {
        return transform;
    }

    public string GetTextFromInteract()
    {
        return "Talk to Robot";
    }

    public void SetUI(bool active)
    {
        interactUI.SetActive(active);
    }
}
