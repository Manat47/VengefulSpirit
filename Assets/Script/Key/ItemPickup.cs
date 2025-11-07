using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private string itemId = "KeyFragment";
    private bool playerIsNearby = false;

    // ⭐️ ลบ: private static GameObject s_pickupPromptUI; (เราไม่ใช้แล้ว)
    // ⭐️ ลบ: ฟังก์ชัน Start() ทั้งหมด (เราไม่ใช้แล้ว)

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNearby = true;
            // ⭐️ เปลี่ยน: สั่ง UIManager ให้โชว์ข้อความ "Collect"
            InteractionUIManager.Instance.ShowPrompt("Press [E] to Collect");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNearby = false;
            // ⭐️ เปลี่ยน: สั่ง UIManager ให้ซ่อน
            InteractionUIManager.Instance.HidePrompt();
        }
    }

    // (Update เหมือนเดิม)
    private void Update()
    {
        if (!playerIsNearby)
            return;

        // ⭐️ นี่คือส่วนที่ถูกต้อง ⭐️
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collect(); // 1. ต้องสั่ง "Collect()"
        }
    } // 2. ต้องมี "}" ปิดฟังก์ชัน Update
    private void Collect()
    {
        Debug.Log($"Collected {itemId}");

        // ⭐️ เปลี่ยน: สั่ง UIManager ให้ซ่อน
        InteractionUIManager.Instance.HidePrompt();

        GameManager.Instance.CollectItem(itemId);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        // ⭐️ เปลี่ยน: ถ้า Player อยู่ใกล้ ให้สั่ง UIManager ให้ซ่อน
        if (playerIsNearby)
        {
            InteractionUIManager.Instance.HidePrompt();
        }
    }
}