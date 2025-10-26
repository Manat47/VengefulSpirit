using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string itemId = "KeyFragment";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ป้องกันเผื่อ GameManager ยังไม่พร้อม
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CollectItem(itemId);
                Debug.Log("Picked up: " + itemId);
            }
            else
            {
                Debug.LogWarning("No GameManager.Instance in scene when picking up " + itemId);
            }

            Destroy(gameObject);
        }
    }
}
