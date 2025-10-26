using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string itemId = "KeyFragment"; // ระบุชนิด เช่น "KeyFragment", "Battery", etc.

    private void OnTriggerEnter(Collider other)
    {
        // ตรวจว่า Player ชน
        if (other.CompareTag("Player"))
        {
            // บอก GameManager ว่าเก็บได้แล้ว
            GameManager.Instance.CollectItem(itemId);

            // ทำเอฟเฟกต์ (เช่นเสียงหรือเอฟเฟกต์วิบวับได้ทีหลัง)
            Debug.Log("Picked up: " + itemId);

            // ทำลายตัวเองออกจากซีน
            Destroy(gameObject);
        }
    }
}
