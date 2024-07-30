using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavAi : MonoBehaviour
{
    public enum FSMStates
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Dead
    }

    public GameObject player;
    public FSMStates currentState;
    public float chaseDistance = Mathf.Infinity;
    public float attackDistance = 1f;
    public GameObject deadVFX;
    public GameObject lootPrefab;

    public float updateTimeDelay = 1.0f;
    public float updateDistanceThreshold = 3f;


    Animator anim;
    NavMeshAgent agent;

    Vector3 nextDestination; 
    float distanceToPlayer;

    bool isDead;
    Transform deadTransform;

    float agentSpeed;
    float timer;

    EnemyHealth enemyHealth;
    int health;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemyHealth = GetComponent<EnemyHealth>();

        isDead = false;

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        nextDestination = player.transform.position;
        agentSpeed = agent.speed;
        timer = updateTimeDelay;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;


        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        health = enemyHealth.currentHealth;

        switch (currentState)
        {
            case FSMStates.Idle:
                break;
            case FSMStates.Patrol:
                UpdatePatrolState();
                break;
            case FSMStates.Chase:
                UpdateChaseState();
                break;
            case FSMStates.Attack:
                UpdateAttackState();
                break;
            case FSMStates.Dead:
                UpdateDeadState();
                break;
        }

        if (health <= 0)
        {
            currentState = FSMStates.Dead;
        }
    }

    void UpdatePatrolState()
    {
        // move the enemy
        // if the enemy is close to the player, switch to chase state

        Debug.Log("Patrolling");

        anim.SetInteger("animState", 1);
        //nextDestination = wanderPoints[currentDestinationIndex].transform.position;

        if (Vector3.Distance(transform.position, nextDestination) < 2f)
        {
            //FindNextPoint();
        }
        else if (distanceToPlayer <= chaseDistance)
        {
            currentState = FSMStates.Chase;
        }

        //FaceTarget(nextDestination);

        // Moving done by animation
        // transform.position = Vector3.MoveTowards(transform.position, nextDestination, enemySpeed * Time.deltaTime);
    }

    void UpdateChaseState()
    {
        // move the enemy towards the player
        Debug.Log("Chasing");

        anim.SetInteger("animState", 1);

        nextDestination = player.transform.position;

        if (distanceToPlayer <= attackDistance)
        {
            currentState = FSMStates.Attack;
        }
        else if (distanceToPlayer > chaseDistance)
        {
            currentState = FSMStates.Patrol;
        }

        SetTarget(nextDestination);

        //FaceTarget(nextDestination);
        // transform.position = Vector3.MoveTowards(transform.position, nextDestination, enemySpeed * Time.deltaTime);
    }

    void UpdateAttackState()
    {
        // attack the player
        Debug.Log("Attacking");

        nextDestination = player.transform.position;

        if (distanceToPlayer <= attackDistance)
        {
            currentState = FSMStates.Attack;
        }
        else if (distanceToPlayer > attackDistance && distanceToPlayer <= chaseDistance)
        {
            currentState = FSMStates.Chase;
        }
        else if (distanceToPlayer > chaseDistance)
        {
            currentState = FSMStates.Patrol;
        }


        SetTarget(nextDestination);
        anim.SetInteger("animState", 2);
        //EnemySpellCast();
    }

    void UpdateDeadState()
    {
        // play the death animation
        Debug.Log("Enemy is dead");
        isDead = true;
        anim.SetInteger("animState", 4);
        agent.speed = 0;
        deadTransform = gameObject.transform;

        Destroy(gameObject, 3f);
    }

    void SetTarget(Vector3 target)
    {
       //Debug.Log("Setting target to: " + target);

        if (timer < 0)
        {
            float distanceDifference = Vector3.Distance(player.transform.position, agent.destination);
            if (distanceDifference > updateDistanceThreshold)
            {
                agent.SetDestination(target);
            }
            timer = updateTimeDelay;
        }
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }

    private void OnDestroy()
    {
        Instantiate(lootPrefab, deadTransform.position, deadTransform.rotation);
        Instantiate(deadVFX, deadTransform.position, deadTransform.rotation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }

}
