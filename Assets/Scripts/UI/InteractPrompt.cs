using UnityEngine;
using TMPro;

public class InteractPrompt : MonoBehaviour
{
    public static InteractPrompt Instance { get; private set; }

    [Header("UI")]
    public TMP_Text promptLabel;   // ใช้ TextMeshPro ตัวเดิม

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // ถ้ายังไม่ได้ลากใน Inspector ให้ไปเอา TMP_Text จากตัวเอง
        if (promptLabel == null)
            promptLabel = GetComponent<TMP_Text>();

        Hide();    // เริ่มเกมซ่อนข้อความไว้ก่อน
    }

    public void Show(string message)
    {
        if (promptLabel == null) return;

        promptLabel.text = message;
        gameObject.SetActive(true);    // แสดง InteractPrompt (ตัวเดียวที่มี TMP)
    }

    public void Hide()
    {
        gameObject.SetActive(false);   // ซ่อน InteractPrompt
    }
}
