using UnityEngine;
using System.Collections;

public class TeleportGhost : MonoBehaviour
{
    // 1. ช่องนี้ "ลาก Prefab" ของผีมาใส่
    public GameObject ghostPrefab; // <-- ใช้ตัวแปรนี้

    // 2. ลาก "จุดผีโผล่" (GhostSpawnPoint) มาใส่
    public Transform teleportTarget;

    // 3. ลาก "Player" มาใส่
    public Transform playerTransform;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;

            // --- นี่คือส่วนที่เปลี่ยน ---

            // 1. "สร้าง" ผีจาก Prefab ณ ตำแหน่งของจุดเป้าหมาย
            GameObject newGhost = Instantiate(ghostPrefab, teleportTarget.position, teleportTarget.rotation);

            // 2. (ทางเลือก) สั่งให้ผีที่เพิ่งสร้าง หันหน้าหาผู้เล่น
            if (playerTransform != null)
            {
                newGhost.transform.LookAt(playerTransform);
            }

            // 3. สั่งให้เริ่มนับเวลา 3 วินาที เพื่อ "ทำลาย" ผีตัวนี้ทิ้ง
            StartCoroutine(DestroyAfterTime(newGhost, 1.0f));
        }
    }

    // ฟังก์ชันนี้จะทำลาย GameObject ที่ส่งเข้ามา หลังจากเวลาที่กำหนด
    private IEnumerator DestroyAfterTime(GameObject ghostToDestroy, float delay)
    {
        yield return new WaitForSeconds(delay);

        // เมื่อครบ 3 วินาที... สั่ง "ทำลาย" ทิ้ง
        Destroy(ghostToDestroy);

        // (ทางเลือก) ทำให้ Trigger นี้กลับมาทำงานได้อีกครั้ง
        // hasTriggered = false; 
    }
}