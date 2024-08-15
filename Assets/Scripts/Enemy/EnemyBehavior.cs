using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
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
    public GameObject projectile;
    public GameObject tailTip;
    public float shootRate = 10f;
    public GameObject deadVFX;

    public Transform enemyEyes;
    public float fieldOfView = 45f;

    GameObject[] wanderPoints;
    Vector3 nextDestination;

    Rigidbody rb;
    Animator anim;

    float distanceToPlayer;
    float elaspedTime = 0;

    EnemyHealth enemyHealth;
    int health;

    int currentDestinationIndex = 0;

    Transform deadTransform;
    bool isDead;

    List<Vector3> chaseWaypoints;


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

        enemyHealth = GetComponent<EnemyHealth>();
        health = enemyHealth.currentHealth;

        rb = GetComponent<Rigidbody>();

        isDead = false;

        currentState = FSMStates.Attack;
        FindNextPoint();

        chaseWaypoints = new List<Vector3>();
    }



    void UpdatePatrolState()
    {
        // move the enemy
        // if the enemy is close to the player, switch to chase state

        //Debug.Log("Patrolling");

        anim.SetInteger("animState", 1);

        //nextDestination = wanderPoints[currentDestinationIndex].transform.position;

        if (Vector3.Distance(transform.position, nextDestination) < 2f)
        {
            FindNextPoint();
        }
        // First condition is redundant - handled by IsPlayerInClearFOV() raycast distance
        else if (distanceToPlayer <= chaseDistance && IsPlayerInClearFOV())
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

        anim.SetInteger("animState", 2);

        nextDestination = player.transform.position;


        if (IsPlayerInClearFOV() && distanceToPlayer <= attackDistance)
        {
            //Debug.Log("Player in sight, Attacking");
                currentState = FSMStates.Attack;
        }
        else
        {
            UpdateChase();

            if (chaseWaypoints.Count > 0)
            {
                nextDestination = chaseWaypoints[0];

                transform.position = Vector3.MoveTowards(transform.position, nextDestination, enemySpeed * Time.deltaTime);
            }
            else
            {
                currentState = FSMStates.Patrol;
            }
        }

        // transform.position = Vector3.MoveTowards(transform.position, nextDestination, enemySpeed * Time.deltaTime);

    }

    void UpdateChase()
    {
        if (LevelManager.isGameOver) return;

        if (IsPlayerInClearFOV())
        {
            chaseWaypoints.Clear();
            chaseWaypoints.Add(player.transform.position);
            FaceTarget(player.transform.position);
        }
        else if (chaseWaypoints.Count > 0)
        {
            FaceTarget(chaseWaypoints[0]);
        }


        if (chaseWaypoints.Count > 0 && Vector3.Distance(chaseWaypoints[chaseWaypoints.Count - 1], player.transform.position) > 1f)
        {
            chaseWaypoints.Add(player.transform.position);

        }

        if (chaseWaypoints.Count == 0)
        {
            return;
        }

        if (Vector3.Distance(transform.position, chaseWaypoints[0]) < 2f)
        {
            chaseWaypoints.RemoveAt(0);
        }
    }

    void UpdateAttackState()
    {
        // attack the player

        nextDestination = player.transform.position;

        if (distanceToPlayer <= attackDistance)
        {
            if (IsPlayerInClearFOV())
            {
                UpdateChase();
                currentState = FSMStates.Attack;
            }
            else
            {
                currentState = FSMStates.Chase;
            }
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

        EnemyProjectile();
    }

    void UpdateDeadState()
    {
        // play the death animation
        //Debug.Log("Enemy is dead");
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
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }

    void EnemyProjectile()
    {
        if (!isDead)
        {
            if (elaspedTime >= shootRate)
            {
                float animDuration = anim.GetCurrentAnimatorStateInfo(0).length;


                Invoke("ShootProjectile", 1f);

                elaspedTime = 0f;
            }
        }
    }

    void ShootProjectile()
    {

        Instantiate(projectile, tailTip.transform.position, tailTip.transform.rotation);
    }

    private void OnDestroy()
    {
        //Instantiate(deadVFX, deadTransform.position, deadTransform.rotation);
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);


        Vector3 frontRayPoint = enemyEyes.position + (transform.forward * chaseDistance);
        Vector3 leftRayPoint = Quaternion.Euler(0, fieldOfView * .5f, 0) * frontRayPoint;
        Vector3 rightRayPoint = Quaternion.Euler(0, -fieldOfView * .5f, 0) * frontRayPoint;

        Debug.DrawLine(enemyEyes.position, frontRayPoint, Color.yellow);
        Debug.DrawLine(enemyEyes.position, leftRayPoint, Color.green);
        Debug.DrawLine(enemyEyes.position, rightRayPoint, Color.green);

        foreach (Vector3 point in chaseWaypoints)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(point, 1f);
        }

        Debug.DrawRay(enemyEyes.position, enemyEyes.position - player.transform.position, Color.red);

    }
    */

    bool IsPlayerInClearFOV()
    {
        int projectileMask = 1 << 2;

        RaycastHit hit;
        Vector3 directionToPlayer = player.transform.position - enemyEyes.position;

        if (Vector3.Angle(directionToPlayer, enemyEyes.forward) <= fieldOfView)
        {
            if (Physics.Raycast(enemyEyes.position, directionToPlayer, out hit, chaseDistance, ~projectileMask))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
        }

        return false;
    }
}

