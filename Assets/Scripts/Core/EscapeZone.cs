using UnityEngine;

public class EscapeZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameManager.Instance.HasAllKeyFragments())
        {
            // ส่งข้อความอังกฤษเท่านั้น
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowEnding(
                    "You may run from here,\nbut you can't outrun the truth."
                );
            }
            else
            {
                Debug.Log("YOU ESCAPED. BAD ENDING...");
            }
        }
        else
        {
            // ยังหนีไม่ได้
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowNeedMoreMessage();
            }
            else
            {
                Debug.Log("You need more key fragments.");
            }
        }
    }
}
