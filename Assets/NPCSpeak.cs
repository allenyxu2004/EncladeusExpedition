using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCSpeak : MonoBehaviour
{
    public Text dialogueText;

    float timer;

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = 0;
            dialogueText.text = "";
        }
    }

    public void SayDialogue(string dialogueText, AudioClip dialogueAudio)
    {
        timer = dialogueAudio.length;
        this.dialogueText.text = dialogueText;
        AudioSource.PlayClipAtPoint(dialogueAudio, transform.position);
    }
}
