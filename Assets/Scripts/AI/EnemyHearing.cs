using UnityEngine;
using UnityEngine.AI;

public class EnemyHearing : MonoBehaviour
{
    public float hearRangeMultiplier = 1.0f;
    public float patrolWait = 2f;
    public Transform[] patrolPoints;

    [Header("Animation")]
    public Animator anim; // <- ดราก Animator ของซอมบี้ลง Inspector
    readonly int hashIsChasing = Animator.StringToHash("IsChasing");

    NavMeshAgent agent;
    Transform player;
    PlayerNoise playerNoise;

    int patrolIndex;
    float waitTimer;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerNoise = player.GetComponent<PlayerNoise>();
    }

    void Update()
    {
        bool heardPlayer = false;

        if (playerNoise != null)
        {
            float noiseRadius = playerNoise.CurrentNoiseRadius * hearRangeMultiplier;
            float dist = Vector3.Distance(transform.position, player.position);

            if (noiseRadius > 0f && dist <= noiseRadius)
            {
                heardPlayer = true;
            }
        }

        if (heardPlayer)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void ChasePlayer()
    {
        agent.speed = 4.0f;
        agent.SetDestination(player.position);

        if (anim != null)
            anim.SetBool(hashIsChasing, true); // บอก Animator ให้เข้า angry
    }

    void Patrol()
    {
        // เดินตาม patrol points ปกติ
        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                waitTimer += Time.deltaTime;
                agent.speed = 2.0f;

                if (waitTimer >= patrolWait)
                {
                    patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
                    agent.SetDestination(patrolPoints[patrolIndex].position);
                    waitTimer = 0f;
                }
            }
        }

        if (anim != null)
            anim.SetBool(hashIsChasing, false); // กลับ idle
    }
}
