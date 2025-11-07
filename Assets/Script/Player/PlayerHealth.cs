using UnityEngine;
using UnityEngine.UI; // ⭐️ (สำคัญ) ต้องเพิ่มบรรทัดนี้เพื่อใช้ UI

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Stats")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("UI")]
    [SerializeField] private Slider hpBarSlider;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHPBar(); // อัปเดต HP Bar ตอนเริ่มเกม (ให้เต็ม)
    }

    // ⭐️ นี่คือ "ฟังก์ชันรับดาเมจ"
    public void TakeDamage(int damageAmount)
    {
        // ถ้าตายแล้ว (เลือด = 0) ก็ไม่ต้องทำอะไร
        if (currentHealth <= 0) return;

        currentHealth -= damageAmount;
        Debug.Log($"Player โดนตี! HP เหลือ: {currentHealth}/{maxHealth}");

        UpdateHPBar(); // (สั่งอัปเดต UI ทันที)

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHPBar()
    {
        // ⭐️ (โค้ดดั้งเดิม) ⭐️
        if (hpBarSlider != null)
        {
            // เราจะเปลี่ยน "value" (0-1) แทน "fillAmount"
            hpBarSlider.value = (float)currentHealth / (float)maxHealth;
        }
    }

    private void Die()
    {
        Debug.Log("Player ตายแล้ว!");
        // (Optional: เล่นเสียง, animation ฯลฯ)

        // ⭐️ สั่ง GameManager ให้ "จบเกม"
        GameManager.Instance.TriggerGameOver();
    }
}