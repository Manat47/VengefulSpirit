using UnityEngine;
using TMPro; // ⭐️ อย่าลืมเพิ่มบรรทัดนี้

public class InteractionUIManager : MonoBehaviour
{
    public static InteractionUIManager Instance;

    // ลาก Text "PickupPromptUI" (ตัว Text เลย) มาใส่ใน Inspector
    [SerializeField] private TextMeshProUGUI promptText;

    private void Awake()
    {
        // Singleton (เหมือน GameManager)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // เริ่มเกมโดย "ซ่อน" UI นี้ไว้ก่อน
        promptText.gameObject.SetActive(false);
    }

    // ⭐️ ฟังก์ชัน "โชว์" UI (พร้อมข้อความที่กำหนดเอง)
    public void ShowPrompt(string message)
    {
        promptText.text = message; // เปลี่ยนข้อความใน UI
        promptText.gameObject.SetActive(true); // เปิด UI
    }

    // ⭐️ ฟังก์ชัน "ซ่อน" UI
    public void HidePrompt()
    {
        promptText.gameObject.SetActive(false); // ปิด UI
    }
}