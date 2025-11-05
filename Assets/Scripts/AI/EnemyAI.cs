using UnityEngine;
using UnityEngine.AI;

// ติดบนตัวศัตรู (ตัวที่มี NavMeshAgent + Collider)
// อย่าลืมเซ็ต player, patrolPoints ใน Inspector

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        Patrol,
        Chase
    }

    [Header("Refs")]
    public Transform player;           // ลาก Player ของนายมาวาง
    public Transform[] patrolPoints;   // ลาก PatrolPoint_01 .. _04 ใส่เรียง

    [Header("Vision / Detection")]
    public float sightRange = 10f;     // ระยะที่มันเริ่มเห็นเรา
    public float viewHeight = 1.6f;    // ความสูงตาจากพื้นเวลายิง Ray
    public float giveUpTime = 3f;      // ถ้าหลุดสายตานานเท่านี้จะหยุดไล่

    [Header("Movement Speeds")]
    public float patrolSpeed = 1.5f;   // ความเร็วเดินปกติ
    public float chaseSpeed = 3.5f;    // ความเร็วตอนวิ่งไล่

    private NavMeshAgent agent;
    private State currentState = State.Patrol;
    private int patrolIndex = 0;
    private float loseTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // เริ่มด้วยโหมดลาดตระเวน
        currentState = State.Patrol;
        agent.speed = patrolSpeed;
        GoToNextPatrolPoint();
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Patrol:
                UpdatePatrol();
                LookForPlayer();
                break;

            case State.Chase:
                UpdateChase();
                break;
        }
    }

    // ---------------- PATROL STATE ----------------
    void UpdatePatrol()
    {
        // ถ้าเดินถึงจุด -> เปลี่ยนเป้าหมายจุดต่อไป
        if (!agent.pathPending && agent.remainingDistance < 0.3f)
        {
            GoToNextPatrolPoint();
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        agent.speed = patrolSpeed;
        agent.SetDestination(patrolPoints[patrolIndex].position);

        patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
    }

    // ---------------- CHASE STATE ----------------
    void UpdateChase()
    {
        // ไล่ player ตลอด
        if (player != null)
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position);
        }

        // ยังมองเห็นอยู่ไหม?
        if (CanSeePlayer())
        {
            loseTimer = giveUpTime; // reset timer เพราะยังเห็น
        }
        else
        {
            // มองไม่เห็น → เริ่มนับถอยหลังจะวาร์ปกลับ Patrol
            loseTimer -= Time.deltaTime;
            if (loseTimer <= 0f)
            {
                // เลิกไล่ กลับไป Patrol
                currentState = State.Patrol;
                GoToNextPatrolPoint();
            }
        }
    }

    // ---------------- DETECTION ----------------
    void LookForPlayer()
    {
        // ในสถานะ Patrol: ถ้าเราเข้าใกล้ + มี line of sight → เปลี่ยนเป็น Chase
        if (CanSeePlayer())
        {
            currentState = State.Chase;
            loseTimer = giveUpTime; // เริ่มนับเวลายอมแพ้ตอน future
        }
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector3 myEye = transform.position + Vector3.up * viewHeight;
        Vector3 dirToPlayer = (player.position - myEye);
        float dist = dirToPlayer.magnitude;

        if (dist > sightRange)
            return false; // ไกลเกิน

        // ยิง raycast ไปยัง player ถ้ามีอะไรขวางก็คือมองไม่เห็น
        if (Physics.Raycast(myEye, dirToPlayer.normalized, out RaycastHit hit, sightRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    // ---------------- DEBUG / GIZMOS ----------------
    void OnDrawGizmosSelected()
    {
        // แสดงระยะมองเห็นใน Scene View
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
