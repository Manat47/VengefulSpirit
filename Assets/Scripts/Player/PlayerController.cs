using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Refs")]
    public Transform cameraRoot; // CameraRoot

    [Header("Move")]
    public float walkSpeed = 2.0f;
    public float runSpeed = 4.0f;
    public float crouchSpeed = 1.2f;
    public float gravity = -9.81f;
    public float mouseSensitivityX = 2.0f;
    public float mouseSensitivityY = 2.0f;
    public bool canRun = true;

    private CharacterController cc;
    private Vector3 vel;
    private float camPitch = 0f;

    void Awake()
    {
        cc = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Look();
        Move();
    }

    void Look()
    {
        float mx = Input.GetAxis("Mouse X") * mouseSensitivityX;
        float my = Input.GetAxis("Mouse Y") * mouseSensitivityY;

        // หมุนแนวนอน (yaw)
        transform.Rotate(Vector3.up * mx);

        // ก้ม/เงย (pitch)
        camPitch -= my;
        camPitch = Mathf.Clamp(camPitch, -80f, 80f);
        cameraRoot.localRotation = Quaternion.Euler(camPitch, 0f, 0f);
    }

    void Move()
    {
        // ปุ่มเดิน
        float ix = Input.GetAxisRaw("Horizontal");
        float iz = Input.GetAxisRaw("Vertical");

        // กด Shift = วิ่ง
        bool running = canRun && Input.GetKey(KeyCode.LeftShift);
        float speed = running ? runSpeed : walkSpeed;

        // ทิศการเดินตามมุมผู้เล่น
        Vector3 move = (transform.right * ix + transform.forward * iz).normalized;
        Vector3 horiz = move * speed;

        // แรงโน้มถ่วง
        if (cc.isGrounded && vel.y < 0f)
            vel.y = -2f; // กดลงพื้นไว้

        vel.y += gravity * Time.deltaTime;

        // สุดท้าย apply การเคลื่อนที่
        cc.Move((horiz + vel) * Time.deltaTime);
    }
}
