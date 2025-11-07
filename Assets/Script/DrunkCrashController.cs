using System.Collections;
using UnityEngine;

public class DrunkCrashController : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject blackoutPanel; // ลาก BlackoutPanel จาก Hierarchy มาใส่
    public MonoBehaviour carMovementScript; // ลาก Script ที่ใช้ขับรถปกติมาใส่

    [Header("Crash Settings")]
    public float surgeSpeed = 30f; // ความเร็วตอนพุ่งไปชน

    private bool isCrashing = false; // สถานะว่ากำลังจะชนหรือไม่
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // ปิดหน้าจอดำไว้ก่อนตอนเริ่ม
        if (blackoutPanel != null)
        {
            blackoutPanel.SetActive(false);
        }
    }

    // --- 1. เมื่อขับรถไปชน Trigger Zone ---
    private void OnTriggerEnter(Collider other)
    {
        // เช็กว่าชนกับ "CrashTriggerZone" และยังไม่ได้เริ่มเหตุการณ์
        if (other.gameObject.name == "CrashTriggerZone" && !isCrashing)
        {
            isCrashing = true;

            // ปิด script การขับรถปกติ (ถ้ามี)
            if (carMovementScript != null)
            {
                carMovementScript.enabled = false;
            }

            // หยุดรถให้สนิทก่อน (ถ้าใช้ฟิสิกส์)
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // เริ่มลำดับเหตุการณ์เมา
            StartCoroutine(CrashSequence());
        }
    }

    // --- 2. ลำดับเหตุการณ์ "เมา" (ภาพตัด) ---
    IEnumerator CrashSequence()
    {
        // --- เอฟเฟกต์ "ภาพตัด" ---
        // ครั้งที่ 1
        blackoutPanel.SetActive(true);
        yield return new WaitForSeconds(0.15f); // จอดำ 0.15 วินาที
        blackoutPanel.SetActive(false);
        yield return new WaitForSeconds(0.8f); // เห็นภาพ 0.8 วินาที

        // ครั้งที่ 2
        blackoutPanel.SetActive(true);
        yield return new WaitForSeconds(0.25f); // จอดำ 0.25 วินาที
        blackoutPanel.SetActive(false);
        yield return new WaitForSeconds(0.5f); // เห็นภาพ 0.5 วินาที
        // --- จบเอฟเฟกต์ ---

        // 3. สั่งให้รถ "พุ่ง" ไปข้างหน้า
        // เราจะใช้ ForceMode.Impulse เพื่อ "กระแทก" รถไปข้างหน้าทีเดียว
        rb.AddForce(transform.forward * surgeSpeed, ForceMode.Impulse);
    }

    // --- 4. เมื่อชนรถเป้าหมาย (CarB) ---
    private void OnCollisionEnter(Collision collision)
    {
        // เช็กว่าชนกับ "CarB"
        // (แนะนำให้ไปตั้ง Tag ให้ CarB เป็น "TargetCar" แล้วเช็กด้วย collision.gameObject.CompareTag("TargetCar"))
        if (collision.gameObject.CompareTag("TargetCar")) // ใช้วิธีเช็กชื่อไปก่อนได้
        {
            // เปิดหน้าจอดำค้างไว้
            blackoutPanel.SetActive(true);

            // หยุดเวลาในเกม (Optional)
            Time.timeScale = 0;
        }
    }
}