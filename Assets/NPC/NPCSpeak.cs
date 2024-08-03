using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCSpeak : MonoBehaviour
{
    public Text dialogueText;

    float timer;
    bool useTalkingAction;
    NPCDialogue npcDialogue;

    private void Start()
    {
        npcDialogue = GetComponent<NPCDialogue>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            if (useTalkingAction && npcDialogue.currentState != NPCDialogue.FSMStates.Talking)
                npcDialogue.currentState = NPCDialogue.FSMStates.Talking;
            timer -= Time.deltaTime;
        }
        else
        {
            timer = 0;
            dialogueText.text = "";

            if (npcDialogue.currentState == NPCDialogue.FSMStates.Talking)
                npcDialogue.currentState = NPCDialogue.FSMStates.Idle;

            useTalkingAction = false;
        }
    }

    public void SayDialogue(string dialogueText, AudioClip dialogueAudio)
    {
        SayDialogue(dialogueText, dialogueAudio, false);
    }

    public void SayDialogue(string dialogueText, AudioClip dialogueAudio, bool withAction)
    {
        useTalkingAction = withAction;

        timer = dialogueAudio.length;
        this.dialogueText.text = dialogueText;
        AudioSource.PlayClipAtPoint(dialogueAudio, transform.position);
    }
}
