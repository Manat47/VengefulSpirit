using UnityEngine;
using UnityEngine.AI;

public class EnemyHearing : MonoBehaviour
{
    public float hearRangeMultiplier = 1.0f;
    public float patrolWait = 2f;
    public Transform[] patrolPoints;

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
        // 1) ถ้า player ส่งเสียง ให้วิ่งไปตรงนั้น
        if (playerNoise != null)
        {
            float noiseRadius = playerNoise.CurrentNoiseRadius * hearRangeMultiplier;
            float dist = Vector3.Distance(transform.position, player.position);

            if (dist <= noiseRadius && noiseRadius > 0f)
            {
                agent.speed = 4.0f; // โหมดล่า
                agent.SetDestination(player.position);
                return;
            }
        }

        // 2) ถ้าไม่ได้ยินเสียง -> เดิน patrol ช้า ๆ
        Patrol();
    }

    void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

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
}
