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

    CharacterController cc;
    Vector3 vel;
    float camPitch = 0f;

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

        // yaw (หมุนตัวแนวนอน)
        transform.Rotate(Vector3.up * mx);

        // pitch (เงย/ก้ม)
        camPitch -= my;
        camPitch = Mathf.Clamp(camPitch, -80f, 80f);
        cameraRoot.localRotation = Quaternion.Euler(camPitch, 0f, 0f);
    }

    void Move()
    {
        // input เดิน
        float ix = Input.GetAxisRaw("Horizontal");
        float iz = Input.GetAxisRaw("Vertical");

        // shift = วิ่ง
        bool running = canRun && Input.GetKey(KeyCode.LeftShift);
        float speed = running ? runSpeed : walkSpeed;

        Vector3 move = (transform.right * ix + transform.forward * iz).normalized;
        Vector3 horiz = move * speed;

        // แรงตกจากแรงโน้มถ่วง
        if (cc.isGrounded && vel.y < 0f)
            vel.y = -2f;

        vel.y += gravity * Time.deltaTime;

        // สุดท้าย apply
        cc.Move((horiz + vel) * Time.deltaTime);
    }
}
