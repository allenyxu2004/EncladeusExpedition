using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCSpeak : MonoBehaviour
{
    public Text dialogueText;

    float timer;
    bool useTalkingAction;
    bool multiLineDialogue;

    NPCBehavior npcBehavior;

    private void Start()
    {
        npcBehavior = GetComponent<NPCBehavior>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            if (useTalkingAction && npcBehavior.currentState != NPCBehavior.FSMStates.Talking)
                npcBehavior.currentState = NPCBehavior.FSMStates.Talking;
            timer -= Time.deltaTime;
        }
        else
        {
            timer = 0;
            dialogueText.text = "";

            if (npcBehavior.currentState == NPCBehavior.FSMStates.Talking && !multiLineDialogue)
                npcBehavior.currentState = NPCBehavior.FSMStates.Idle;

            useTalkingAction = false;
        }
    }

    public void SayDialogue(string dialogueText, AudioClip dialogueAudio)
    {
        SayDialogue(dialogueText, dialogueAudio, false);
    }

    public void SayDialogue(string dialogueText, AudioClip dialogueAudio, bool withAction)
    {

        if (timer <= 0)
        {
            useTalkingAction = withAction;

            timer = dialogueAudio.length;
            this.dialogueText.text = dialogueText;
            AudioSource.PlayClipAtPoint(dialogueAudio, transform.position);
        }

    }

    public void StopDialogue()
    {
        timer = 0;
        dialogueText.text = "";
    }

    public IEnumerator SayMultiLineDialogue(string[] dialogueLines, AudioClip[] audioLines)
    {
        for (int i = 0; i < dialogueLines.Length; i++)
        {
            multiLineDialogue = true;
            SayDialogue(dialogueLines[i], audioLines[i], true);
            yield return new WaitForSeconds(audioLines[i].length);
        }

        multiLineDialogue = false;

    }

    public void SayMultiLineDialogue(Dictionary<string, AudioClip> dialogueLines)
    {
        foreach (KeyValuePair<string, AudioClip> line in dialogueLines)
        {
            SayDialogue(line.Key, line.Value, true);
            timer = 1;
        }
    }
}
