using UnityEngine;
using UnityEngine.AI;

public class EnemyLightVision : MonoBehaviour
{
    public Light playerLight;
    public float viewAngle = 70f;
    public float viewDist = 10f;
    public float patrolSpeed = 1.8f;
    public float chaseSpeed = 4.0f;
    public Transform[] patrolPoints;

    NavMeshAgent agent;
    Transform player;
    int patrolIndex;
    float waitTimer;
    public float patrolWait = 2f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        bool chasing = false;

        if (playerLight != null && playerLight.enabled)
        {
            Vector3 toPlayer = player.position - transform.position;
            float dist = toPlayer.magnitude;

            if (dist <= viewDist)
            {
                float ang = Vector3.Angle(transform.forward, toPlayer);
                if (ang <= viewAngle)
                {
                    // ไล่!!!
                    agent.speed = chaseSpeed;
                    agent.SetDestination(player.position);
                    chasing = true;
                }
            }
        }

        if (!chasing)
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        agent.speed = patrolSpeed;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= patrolWait)
            {
                patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
                agent.SetDestination(patrolPoints[patrolIndex].position);
                waitTimer = 0f;
            }
        }
    }
}
