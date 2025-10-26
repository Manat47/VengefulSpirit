using UnityEngine;

public class EscapeZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance.HasAllKeyFragments())
            {
                Debug.Log("YOU ESCAPED. BAD ENDING...");
                // TODO: แสดง UI Ending / โหลดฉากจบ / Fade screen
            }
            else
            {
                Debug.Log("You need more key fragments.");
                // TODO: แสดง UI "ยังหากุญแจไม่ครบ"
            }
        }
    }
}
