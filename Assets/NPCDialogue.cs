using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialogue : MonoBehaviour
{
    public enum FSMStates
    {
        Idle,
        Talking,
        Walking,
        Dancing
    }

    public GameObject player;
    public Text dialogueText;
    public AudioClip greetingsSFX;
    public Transform head;


    Animator animator;
    bool isPlayerClose;

    public FSMStates currentState = FSMStates.Idle;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }


        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case FSMStates.Idle:
                break;
            case FSMStates.Talking:
                UpdateTalkingState();
                break;
            case FSMStates.Dancing:
                UpdateDancingState();
                break;
            case FSMStates.Walking:
                UpdateWalkingState();
                break;
        }
    }

    // Play talking animation
    void UpdateTalkingState()
    {
        Vector3 lookDirection = (player.transform.position - head.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        head.rotation = Quaternion.Slerp(head.rotation, lookRotation, Time.deltaTime * 20f);
    }

    void UpdateDancingState()
    {
        
    }
    
    void UpdateWalkingState()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isPlayerClose = true;
            dialogueText.text = this.name + ": " + "Hello, Sailor!";

            animator.SetTrigger("playerClose");
            AudioSource.PlayClipAtPoint(greetingsSFX, transform.position);
            currentState = FSMStates.Talking;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            dialogueText.text = "";
            isPlayerClose = false;
        }
    }
}
