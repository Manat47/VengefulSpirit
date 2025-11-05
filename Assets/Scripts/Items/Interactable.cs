using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{
    [Header("Interact Info")]
    public string itemId = "";         // id ของของชิ้นนั้น
    [TextArea]
    public string promptText = "Press [E] to interact";     // ข้อความขึ้นบนจอ


    // เรียกจาก PlayerInteract เวลาเรากด E ใส่ชิ้นนี้
    public virtual void Interact()
    {
        // เคสเก็บของทั่วไป
        if (!string.IsNullOrEmpty(itemId) && GameManager.Instance != null)
        {
            GameManager.Instance.CollectItem(itemId);
        }

        // ถ้าเป็นไอเท็มเก็บแล้วหาย
        if (!string.IsNullOrEmpty(itemId))
        {
            Destroy(gameObject);
        }
    }
}
