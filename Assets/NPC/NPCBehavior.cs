using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class NPCBehavior : MonoBehaviour
{
    public enum FSMStates
    {
        Idle,
        Talking,
        Walking,
        Dancing
    }

    public GameObject player;
    public AudioClip greetingsSFX;
    public FSMStates currentState = FSMStates.Idle;

    public Transform npcEyes;
    public float greetDistance = 10f;
    public float fieldOfView = 45f;

    FSMStates[] neutralStates = { FSMStates.Idle, FSMStates.Walking, FSMStates.Dancing };
    FSMStates[] stillStates = { FSMStates.Idle, FSMStates.Dancing };
    NPCSpeak npcSpeak;
    Animator animator;
    NavMeshAgent agent;
    bool isPlayerClose;
    FSMStates lastState;

    GameObject[] wanderPoints;
    Vector3 nextDestination;
    int currentDestinationIndex = 0;

    bool greetedPlayer;

    static int idleState = 0;
    static int walkingState = 1;
    static int dancingState = 2;
    static int talkingState1 = 3;
    static int talkingState2 = 4;

    float actionTimer;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        npcSpeak = GetComponent<NPCSpeak>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        wanderPoints = GameObject.FindGameObjectsWithTag("WanderPoint");

        actionTimer = 0;
        isPlayerClose = false;
        greetedPlayer = false;

        FindNextPoint();
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(transform.position, nextDestination) < 1f)
        {
            FindNextPoint();
            if (lastState == FSMStates.Walking)
            {
                SetTarget(nextDestination);
            }

        }

        if (IsPlayerInClearFOV())
        {
            if (!greetedPlayer)
            {
                GreetPlayer();
                greetedPlayer = true;
            }
        }
        else
        {
            greetedPlayer = false;
        }

        if (currentState == FSMStates.Talking)
        {
            actionTimer = 0;
        }


        if (actionTimer > 0)
        {
            actionTimer -= Time.deltaTime;
        }
        else
        {
            actionTimer = 0;
            lastState = currentState;

            switch (currentState)
            {
                case FSMStates.Idle:
                    UpdateIdleState();
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


    }

    void UpdateIdleState()
    {
        //Debug.Log("Idle state");

        if (isPlayerClose)
        {
            FSMStates nextStillState = stillStates[Random.Range(0, stillStates.Length)];
            currentState = nextStillState;
        }
        else
        {
            FSMStates nextNeutralState = neutralStates[Random.Range(0, neutralStates.Length)];
            currentState = nextNeutralState;
        }

        agent.destination = transform.position;
        animator.SetInteger("animState", 0);
        actionTimer = Random.Range(3, 5);


    }

    // Play talking animation
    void UpdateTalkingState()
    {
        //Debug.Log("Talking state");
        agent.destination = transform.position;
        FaceTarget(player.transform.position);
        int talkingState = Random.Range(talkingState1, talkingState2);
        animator.SetInteger("animState", talkingState);
    }

    void UpdateDancingState()
    {
        //Debug.Log("Dancing state");

        if (isPlayerClose)
        {
            FSMStates nextStillState = stillStates[Random.Range(0, stillStates.Length)];
            currentState = nextStillState;
        }
        else
        {
            FSMStates nextNeutralState = neutralStates[Random.Range(0, neutralStates.Length)];
            currentState = nextNeutralState;
        }

        agent.destination = transform.position;
        animator.SetInteger("animState", 2);
        actionTimer = 5;

    }

    void UpdateWalkingState()
    {
        //Debug.Log("We walking");
        FSMStates nextNeutralState = neutralStates[Random.Range(0, neutralStates.Length)];
        currentState = nextNeutralState;

        if (agent.destination != nextDestination)
        {
            SetTarget(nextDestination);
        }

        actionTimer = Random.Range(8, 10);
        animator.SetInteger("animState", 1);


    }

    void FindNextPoint()
    {
        nextDestination = wanderPoints[currentDestinationIndex].transform.position;

        currentDestinationIndex = (currentDestinationIndex + 1) % wanderPoints.Length;
    }

    void SetTarget(Vector3 target)
    {
        agent.SetDestination(target);
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 lookDirection = (target - transform.position).normalized;
        lookDirection.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 20f);
    }

    bool IsPlayerInClearFOV()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = player.transform.position - npcEyes.position;

        if (Vector3.Angle(directionToPlayer, npcEyes.forward) <= fieldOfView)
        {
            if (Physics.Raycast(npcEyes.position, directionToPlayer, out hit, greetDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
            else
            {
            }
        }


        return false;
    }

    void GreetPlayer()
    {
        agent.destination = transform.position;
        FaceTarget(player.transform.position);

        string greetText = this.name + ": " + "Hello, Sailor!";
        npcSpeak.SayDialogue(greetText, greetingsSFX);
        animator.SetTrigger("playerClose");

        actionTimer = greetingsSFX.length;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            isPlayerClose = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isPlayerClose = false;
        }
    }

    private void OnDrawGizmos()
    {


        Vector3 frontRayPoint = npcEyes.position + (transform.forward * greetDistance);
        Vector3 leftRayPoint = Quaternion.Euler(0, fieldOfView * .5f, 0) * frontRayPoint;
        Vector3 rightRayPoint = Quaternion.Euler(0, -fieldOfView * .5f, 0) * frontRayPoint;

        Debug.DrawLine(npcEyes.position, frontRayPoint, Color.yellow);
        Debug.DrawLine(npcEyes.position, leftRayPoint, Color.green);
        Debug.DrawLine(npcEyes.position, rightRayPoint, Color.green);

    }

}