using UnityEngine;

public class BreathHold : MonoBehaviour
{
    public KeyCode holdKey = KeyCode.LeftControl;
    public float maxHoldTime = 4f;
    public float recoverTime = 2f;

    public bool IsHolding { get; private set; }
    public bool IsGasping { get; private set; }

    float holdTimer = 0f;
    float recoverTimer = 0f;

    PlayerNoise noise;

    void Awake()
    {
        noise = GetComponent<PlayerNoise>();
    }

    void Update()
    {
        if (IsGasping)
        {
            recoverTimer += Time.deltaTime;
            if (recoverTimer >= recoverTime)
            {
                IsGasping = false;
                recoverTimer = 0f;
            }
            return;
        }

        if (Input.GetKey(holdKey) && !IsGasping)
        {
            IsHolding = true;
            holdTimer += Time.deltaTime;

            // ระหว่างกลั้นหายใจ เราไม่ปล่อยเสียงเดิน
            // ตัดการ EmitNoise ชั่วคราว
            // (วิธีง่าย: เราจะไม่เรียก EmitNoise จาก Update เดิน ถ้า IsHolding จริง
            // เดี๋ยวไปปรับใน PlayerNoise เพิ่มทีหลังได้)

            if (holdTimer >= maxHoldTime)
            {
                // หมดลม -> สะอึกดัง
                IsHolding = false;
                IsGasping = true;
                holdTimer = 0f;

                if (noise != null)
                    noise.EmitNoise(noise.runNoise * 1.5f); // เสียงดังพิเศษ ดึงผี
            }
        }
        else
        {
            // ปล่อยหายใจ
            if (IsHolding)
            {
                IsHolding = false;
                holdTimer = 0f;
                IsGasping = true; // หลังกลั้นจะหอบสักพัก -> ยังเสียงหน่อย
                if (noise != null)
                    noise.EmitNoise(noise.walkNoise * 0.5f); // เบาหน่อย
            }
        }
    }
}
