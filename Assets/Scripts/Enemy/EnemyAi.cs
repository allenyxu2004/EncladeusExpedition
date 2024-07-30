using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public enum FSMStates
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Dead
    }

    public FSMStates currentState;

    public float attackDistance = 5f;
    public float chaseDistance = 10f;
    public float enemySpeed = 5f;
    public GameObject player;
    public GameObject[] spellProjectiles;
    public GameObject wandTip;
    public float shootRate = 10f;
    public GameObject deadVFX;

    GameObject[] wanderPoints;
    Vector3 nextDestination;

    Animator anim;

    float distanceToPlayer;
    float elaspedTime = 0;

    EnemyHealth enemyHealth;
    int health;

    int currentDestinationIndex = 0;

    Transform deadTransform;
    bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        health = enemyHealth.currentHealth;

        switch(currentState)
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

        elaspedTime += Time.deltaTime;

        if (health <= 0)
        {
            currentState = FSMStates.Dead;
        }
    }

    void Initialize()
    {
        wanderPoints = GameObject.FindGameObjectsWithTag("WanderPoint");
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        wandTip = GameObject.FindGameObjectWithTag("WandTip");

        enemyHealth = GetComponent<EnemyHealth>();
        health = enemyHealth.currentHealth;

        isDead = false;

        currentState = FSMStates.Patrol;
        FindNextPoint();
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
            FindNextPoint();
        }
        else if(distanceToPlayer <= chaseDistance)
        {
            currentState = FSMStates.Chase;
        }

        FaceTarget(nextDestination);

        // Moving done by animation
        // transform.position = Vector3.MoveTowards(transform.position, nextDestination, enemySpeed * Time.deltaTime);
    }

    void UpdateChaseState()
    {
        // move the enemy towards the player
        Debug.Log("Chasing");

        anim.SetInteger("animState", 2);

        nextDestination = player.transform.position;

        if (distanceToPlayer <= attackDistance)
        {
            currentState = FSMStates.Attack;
        }
        else if (distanceToPlayer > chaseDistance)
        {
            currentState = FSMStates.Patrol;
        }

        FaceTarget(nextDestination);
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

        FaceTarget(nextDestination);
        anim.SetInteger("animState", 3);
        EnemySpellCast();
    }

    void UpdateDeadState()
    {
        // play the death animation
        Debug.Log("Enemy is dead");
        isDead = true;
        anim.SetInteger("animState", 4);
        deadTransform = gameObject.transform;

        Destroy(gameObject, 3f);
    }

    void FindNextPoint()
    {
        nextDestination = wanderPoints[currentDestinationIndex].transform.position;

        currentDestinationIndex = (currentDestinationIndex + 1) % wanderPoints.Length;
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }

    void EnemySpellCast()
    {
        if (!isDead)
        {
            if (elaspedTime >= shootRate)
            {
                float animDuration = anim.GetCurrentAnimatorStateInfo(0).length;

                Invoke("SpellCasting", animDuration);
                elaspedTime = 0f;
            }
        }
    }

    void SpellCasting()
    {
        // Randomly select a spell to cast
        int randProjectileIndex = Random.Range(0, spellProjectiles.Length);
        GameObject spellProjectile = spellProjectiles[randProjectileIndex];

        Instantiate(spellProjectile, wandTip.transform.position, wandTip.transform.rotation);
    }

    private void OnDestroy()
    {
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
