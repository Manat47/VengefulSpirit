using UnityEngine;

public class KeyChest : Interactable   // ให้สืบจาก Interactable
{
    [Header("Final key to spawn")]
    public GameObject carKeyPrefab;     // Prefab กุญแจรถ
    public Transform spawnPoint;        // จุด spawn กุญแจ

    [Header("Opening Animation (Legacy Animation)")]
    public Animation chestAnimation;    // component Animation บนหีบ
    public string openClipName = "ChestAnim";   // ชื่อคลิปใน Animation

    private bool isOpened = false;

    // ถูกเรียกจาก PlayerInteract ตอนเรากด E ใส่หีบ
    public override void Interact()
    {
        // 1) ป้องกันกดซ้ำหลังเปิดแล้ว
        if (isOpened)
        {
            Debug.Log("Chest already opened.");
            return;
        }

        // 2) เช็ค fragment ก่อน
        var gm = GameManager.Instance;
        if (gm == null)
        {
            Debug.LogWarning("No GameManager in scene.");
            return;
        }

        if (!gm.HasAllKeyFragments())
        {
            Debug.Log("You need all key fragments before opening this chest.");
            UIManager.Instance?.ShowNeedMoreMessage();
            return;
        }

        // 3) ผ่านทุกเงื่อนไข เปิดหีบ
        isOpened = true;
        Debug.Log("Chest: OPEN!");

        // เล่นอนิเม
        if (chestAnimation != null)
        {
            chestAnimation.Play(openClipName);
        }
        else
        {
            Debug.LogWarning("Chest has no Animation assigned.");
        }

        // 4) Spawn กุญแจรถ
        if (carKeyPrefab != null)
        {
            Transform t = (spawnPoint != null) ? spawnPoint : transform;
            Instantiate(carKeyPrefab, t.position, t.rotation);
            Debug.Log("Car key spawned at chest.");
        }
        else
        {
            Debug.LogWarning("No carKeyPrefab set on KeyChest.");
        }

        // 5) ปิด collider ของหีบ ไม่ให้โดนยิง Ray อีก
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        // *** ไม่เรียก base.Interact() เพราะไม่อยากให้หีบโดน Destroy ***
    }
}
