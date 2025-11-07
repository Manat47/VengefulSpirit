using Ilumisoft.HealthSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class GhostAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    private Transform playerTarget;

    public bool isHunting = false;
    [SerializeField] private float attackDuration = 1.5f;
    [SerializeField] private float attackDamageDelay = 0.5f;
    [SerializeField] private float attackDamage = 25; // ⭐️ (ดาเมจที่ผีตี)
    private bool isAttacking = false;
    private bool isPlayerInAttackZone = false;

    // (ฟังก์ชัน StartHunting() และ Triggers เหมือนเดิมครับ)
    #region Start & Triggers
    public void StartHunting(Transform spawnPoint)
    {
        // 1. "หา" Component "เดี๋ยวนี้"
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) playerTarget = playerObject.transform;

        if (agent == null) Debug.LogError("GHOSTAI: หา NavMeshAgent ไม่เจอ!");
        if (anim == null) Debug.LogError("GHOSTAI: หา Animator ไม่เจอ!");

        // 2. "เปิด" (SetActive) ผีก่อน
        gameObject.SetActive(true);
        isHunting = true;
        isAttacking = false;
        isPlayerInAttackZone = false;

        agent.stoppingDistance = 0f; // ⭐️ (สำคัญ) ให้มันวิ่งชน

        // 3. "ลอง" วาร์ป (Warp)
        if (agent.Warp(spawnPoint.position))
        {
            transform.rotation = spawnPoint.rotation;
            Debug.Log("GHOSTAI: Warp สำเร็จ! เริ่มการไล่ล่า!");
        }
        else
        {
            Debug.LogError("!!! GHOSTAI: Warp ล้มเหลว! จุดเกิด (SpawnPoint) ไม่ได้อยู่บน NavMesh (พรมสีฟ้า) !!!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInAttackZone = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInAttackZone = false;
        }
    }
    #endregion

    // ⭐️⭐️ นี่คือ "Update()" เวอร์ชันใหม่ (แก้บั๊กแล้ว) ⭐️⭐️
    void Update()
    {
        if (!isHunting || playerTarget == null)
        {
            agent.isStopped = true;
            return;
        }

        // 1. "ตัดสินใจ" (ตี หรือ วิ่ง)

        // (เงื่อนไขใหม่: ถ้า "อยู่ในเขต" และ "ยังไม่ได้ตี")
        if (isPlayerInAttackZone && !isAttacking)
        {
            // -> "ตี"
            StartCoroutine(AttackSequence());
        }
        // (เงื่อนไขใหม่: ถ้า "ไม่อยู่ในเขต" และ "ยังไม่ได้ตี")
        else if (!isPlayerInAttackZone && !isAttacking)
        {
            // -> "วิ่ง"
            agent.isStopped = false;
            agent.SetDestination(playerTarget.position);
        }
        // (ถ้า "กำลังตี" (isAttacking=true) -> ก็ไม่ต้องทำอะไร, รอ Coroutine ปลดล็อค)
    }
    // ⭐️ (สำคัญ) ฟังก์ชันนี้จะถูก "เรียก" โดย Animation Event "จังหวะที่มือฟาดโดน"
    // ⭐️ (สำคัญ) ฟังก์ชันนี้จะถูก "เรียก" โดย Animation Event "จังหวะที่มือฟาดโดน"
    public void HitPlayer()
    {
        Debug.Log("GHOST HIT LANDED!");

        if (playerTarget != null)
        {
            // 1. ค้นหา "HealthComponent" (สคริปต์เลือดของ Asset)
            HealthComponent playerHealth = playerTarget.GetComponent<HealthComponent>();

            // 2. สั่ง "ลดเลือด"
            if (playerHealth != null)
            {
                playerHealth.ApplyDamage(attackDamage);
            }
        }
    }
    // ⭐️ (AttackSequence() ที่ "ปลอดภัย" ขึ้น) ⭐️
    private IEnumerator AttackSequence()
    {
        isAttacking = true; // (A) "ล็อค"
        agent.isStopped = true;
        transform.LookAt(playerTarget);

        anim.SetTrigger("Attack"); // (B) สั่ง "เล่นท่า" ตี

        // ⭐️ (C) นี่คือ "Event" แบบใหม่ ⭐️
        // "รอ" (ตามเวลาที่เราตั้ง) ก่อนที่ "มือจะฟาดโดน"
        yield return new WaitForSeconds(attackDamageDelay);

        // ⭐️ (D) สั่ง "ลดเลือด" ⭐️
        // (เช็กก่อนว่า Player ยังยืนอยู่ในเขตไหม)
        if (isPlayerInAttackZone)
        {
            HitPlayer(); // <<<<< นี่ไงครับ! ง่ายกว่าเดิม!
        }

        // ⭐️ (E) "รอ" ให้ท่าตีเล่นจบ (ส่วนที่เหลือ)
        yield return new WaitForSeconds(attackDuration - attackDamageDelay);

        isAttacking = false; // (F) "ปลดล็อค"
    }
}