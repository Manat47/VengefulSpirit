using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement; // << (1) เพิ่ม: สำหรับโหลดซีน

public class CarOnPath : MonoBehaviour
{
    [Header("Refs")]
    public CinemachineDollyCart cart;
    public Rigidbody rb;
    public Transform lateral;
    public GameObject blackoutPanel;

    [Header("Crash Target")]
    public Transform crashTarget;
    public float crashHomingSpeed = 50f;

    [Header("End Game UI")]
    public GameObject endGameScreen;
    public Transform scrollingText;
    public float scrollSpeed = 50f;

    [Header("Scene Transition")] // << (2) เพิ่ม: ส่วนใหม่
    public string nextSceneName = "DemoScene"; // << (3) ใส่ชื่อซีนถัดไป
    public float delayBeforeLoad = 18f; // << (4) เวลา 18 วินาที

    [Header("Move")]
    public float normalSpeed = 60f;

    // --- ตัวแปรภายใน ---
    private bool isCrashing = false;
    private bool isHoming = false;
    private bool gameHasEnded = false;
    private bool sceneIsLoading = false; // << (5) เพิ่ม: กันโหลดซีนซ้ำ

    void Reset() { rb = GetComponent<Rigidbody>(); cart = GetComponent<CinemachineDollyCart>(); }

    void Start()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        if (!lateral) lateral = transform;
        if (cart == null) cart = GetComponent<Cinemachine.CinemachineDollyCart>();
        rb.isKinematic = true;
        if (cart) cart.m_Speed = normalSpeed;

        if (blackoutPanel != null) blackoutPanel.SetActive(false);
        if (endGameScreen != null) endGameScreen.SetActive(false);

        // เคลียร์ค่า Time.timeScale เผื่อไว้
        Time.timeScale = 1f;
    }

    // --- 1. เมื่อชน Trigger (เหมือนเดิม) ---
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CrashTrigger") && !isCrashing)
        {
            isCrashing = true;
            StartCoroutine(CrashSequence());
        }
    }

    // --- 2. ภาพตัด (Flicker) (เหมือนเดิม) ---
    IEnumerator CrashSequence()
    {
        if (cart) cart.m_Speed = 0f;

        blackoutPanel.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        blackoutPanel.SetActive(false);
        yield return new WaitForSeconds(0.8f);

        blackoutPanel.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        blackoutPanel.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        if (crashTarget == null) yield break;

        if (cart) cart.enabled = false;
        rb.isKinematic = false;
        isHoming = true;
    }

    // --- 3. บังคับเลี้ยว (ฟิสิกส์) (เหมือนเดิม) ---
    void FixedUpdate()
    {
        if (!isHoming) return;

        Vector3 directionToTarget = (crashTarget.position - rb.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 5.0f));
        rb.velocity = transform.forward * crashHomingSpeed;
    }

    // --- 4. เมื่อชน CarB (เหมือนเดิม) ---
    private void OnCollisionEnter(Collision collision)
    {
        if (isHoming && collision.gameObject.CompareTag("TargetCar"))
        {
            isHoming = false;
            rb.isKinematic = true;

            Time.timeScale = 0f; // หยุดเกม

            blackoutPanel.SetActive(false);
            endGameScreen.SetActive(true);

            gameHasEnded = true; // << บอกให้ Update() เริ่มทำงาน
        }
    }

    // --- 5. ฟังก์ชัน Update (สำหรับ UI และนับถอยหลัง) ---
    void Update()
    {
        // ทำงานเฉพาะเมื่อเกมจบ และยังไม่ได้สั่งโหลดซีน
        if (gameHasEnded && !sceneIsLoading)
        {
            // --- A. เลื่อนข้อความ (เหมือนเดิม) ---
            scrollingText.Translate(Vector3.up * scrollSpeed * Time.unscaledDeltaTime);

            // --- B. นับถอยหลัง (ส่วนที่เพิ่มใหม่) ---
            if (delayBeforeLoad > 0)
            {
                // ลดเวลา delay ด้วย "เวลาจริง" (ไม่สน Time.timeScale)
                delayBeforeLoad -= Time.unscaledDeltaTime;
            }
            else
            {
                // เวลาหมดแล้ว!
                sceneIsLoading = true; // ตั้งธงว่ากำลังจะโหลด (กันรันซ้ำ)

                // *** สำคัญมาก: คืนค่า Time.timeScale ก่อนโหลดซีนใหม่ ***
                Time.timeScale = 1f;

                // สั่งโหลดซีน
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}