using UnityEngine;
using System.Collections;

public class ChestInteract : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animation animationComponent;
    [SerializeField] private string openAnimationName = "ChestAnim";

    [Header("Reward")]
    [SerializeField] private GameObject keyOfCarObject; // ⭐️ ลาก "Key of car" ที่ซ่อนไว้มาใส่

    [Header("UI")]
    [SerializeField] private GameObject chestFeedbackUI;
    [SerializeField] private float feedbackDisplayTime = 3f;

    private bool playerIsNearby = false;
    private bool isChestOpen = false;

    // ⭐️ ลบ: private static GameObject s_pickupPromptUI; (เราไม่ใช้แล้ว)
    // ⭐️ ลบ: ฟังก์ชัน Start() ที่ค้นหา Tag (เราไม่ใช้แล้ว)
    void Start()
    {
        // (ส่วนที่หา Animation Component และตั้งค่า ยังเก็บไว้)
        if (animationComponent == null)
        {
            animationComponent = GetComponent<Animation>();
        }
        if (chestFeedbackUI != null)
        {
            chestFeedbackUI.SetActive(false);
        }
        if (animationComponent != null)
        {
            animationComponent.playAutomatically = false;
            animationComponent.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isChestOpen)
        {
            playerIsNearby = true;
            // ⭐️ เปลี่ยน: สั่ง UIManager ให้โชว์ข้อความ "Open"
            InteractionUIManager.Instance.ShowPrompt("Press [E] to Open");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNearby = false;
            // ⭐️ เปลี่ยน: สั่ง UIManager ให้ซ่อน
            InteractionUIManager.Instance.HidePrompt();

            // (ส่วนที่เหลือเหมือนเดิม)
            StopCoroutine(ShowFeedbackMessage());
            chestFeedbackUI?.SetActive(false);
        }
    }

    void Update()
    {
        if (!playerIsNearby || isChestOpen)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            OnInteract();
        }
    }

    private void OnInteract()
    {
        // ⭐️ เปลี่ยน: สั่ง UIManager ให้ซ่อน (ไม่ว่าผลจะเป็นยังไง)
        InteractionUIManager.Instance.HidePrompt();

        if (GameManager.Instance.HasAllKeyFragments())
        {
            // --- กรณีสำเร็จ ---
            Debug.Log("เงื่อนไขครบ: เปิดหีบ!");
            isChestOpen = true;

            Collider solidChestCollider = GetComponent<Collider>();
            if (solidChestCollider != null)
            {
                // 2. สั่ง "ปิด" มัน (ทำให้หายแข็ง)
                solidChestCollider.enabled = false;
            }
            if (animationComponent != null)
            {
                animationComponent.Play(openAnimationName);
            }

            //  GameManager.Instance.CollectItem("CarKey");
            if (keyOfCarObject != null)
            {
                keyOfCarObject.SetActive(true); // ⭐️ "เปิด" โมเดลกุญแจให้มองเห็น
            }
        }
        else
        {
            // --- กรณีล้มเหลว ---
            Debug.Log("ยังเก็บ Key Fragments ไม่ครบ!");
            StartCoroutine(ShowFeedbackMessage());
        }
    }

    private IEnumerator ShowFeedbackMessage()
    {
        chestFeedbackUI.SetActive(true);
        yield return new WaitForSeconds(feedbackDisplayTime);
        chestFeedbackUI.SetActive(false);

        // ⭐️ เปลี่ยน: ถ้ายังอยู่ใกล้ ให้โชว์ "Open" กลับมา
        if (playerIsNearby && !isChestOpen)
        {
            InteractionUIManager.Instance.ShowPrompt("Press [E] to Open");
        }
    }
}