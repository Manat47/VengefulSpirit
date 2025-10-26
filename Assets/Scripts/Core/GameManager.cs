using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Key Fragment Progress")]
    [SerializeField] private int totalKeyFragmentsNeeded = 3;
    [SerializeField] private int collectedKeyFragments = 0;

    private void Awake()
    {
        // Simple Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // ถ้าต้องการให้ไม่โดนลบตอนเปลี่ยนซีนในอนาคต:
        // DontDestroyOnLoad(gameObject);
    }

    // เรียกจาก PickupItem เมื่อผู้เล่นชน key fragment
    public void CollectItem(string itemId)
    {
        if (itemId == "KeyFragment")
        {
            collectedKeyFragments++;

            // กันไม่ให้เกิน max
            if (collectedKeyFragments > totalKeyFragmentsNeeded)
                collectedKeyFragments = totalKeyFragmentsNeeded;

            Debug.Log($"Key Fragment Collected: {collectedKeyFragments}/{totalKeyFragmentsNeeded}");
        }
    }

    // ใช้เช็คตอนเข้า EscapeZone
    public bool HasAllKeyFragments()
    {
        return collectedKeyFragments >= totalKeyFragmentsNeeded;
    }

    // ===== HUD เรียกถามข้อมูลผ่าน 2 ฟังก์ชันนี้ =====
    public int GetCollectedFragments()
    {
        return collectedKeyFragments;
    }

    public int GetTotalFragmentsNeeded()
    {
        return totalKeyFragmentsNeeded;
    }
}
