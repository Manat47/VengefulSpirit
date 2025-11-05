using UnityEngine;

public class EscapeZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        var gm = GameManager.Instance;
        if (gm == null)
        {
            Debug.LogWarning("No GameManager in scene.");
            return;
        }

        // ยังเก็บ fragment ไม่ครบ
        if (!gm.HasAllKeyFragments())
        {
            Debug.Log("You still need key fragments.");
            UIManager.Instance?.ShowNeedMoreMessage();
            return;
        }

        // fragment ครบแล้ว แต่ยังไม่มีกุญแจรถ
        if (!gm.hasCarKey)
        {
            Debug.Log("You need the car key from the chest.");
            UIManager.Instance?.ShowNeedMoreMessage();
            return;
        }

        // มี CarKey แล้ว → BAD ENDING ตามแผน
        Debug.Log("YOU ESCAPED. BAD ENDING...");
        UIManager.Instance?.ShowEnding(
            "You may run from here,\n" +
            "but you can’t outrun the truth."
        );
    }
}
