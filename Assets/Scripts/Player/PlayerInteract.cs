using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Interact Settings")]
    public float interactRange = 3f;     // ระยะเอื้อม
    public LayerMask interactLayer;      // เลเยอร์ของของที่กด E ได้

    private Interactable currentLookAt;
    private Camera cam;

    void Awake()
    {
        cam = Camera.main; // กล้องหลัก
    }

    void Update()
    {
        CheckLookTarget();
        HandleInteractInput();
    }

    void CheckLookTarget()
    {
        currentLookAt = null;

        if (cam == null)
            return;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange, interactLayer))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                currentLookAt = interactable;

                // ให้ InteractPrompt โชว์ข้อความ
                if (InteractPrompt.Instance != null)
                {
                    InteractPrompt.Instance.Show(interactable.promptText);
                }
                return;
            }
        }

        // ถ้าไม่เจออะไรให้ซ่อน prompt
        if (InteractPrompt.Instance != null)
        {
            InteractPrompt.Instance.Hide();
        }
    }

    void HandleInteractInput()
    {
        if (currentLookAt == null) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentLookAt.Interact();

            // หลังจากกดแล้วซ่อน prompt
            if (InteractPrompt.Instance != null)
            {
                InteractPrompt.Instance.Hide();
            }
        }
    }
}
