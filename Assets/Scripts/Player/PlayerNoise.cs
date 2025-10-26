using UnityEngine;

public class PlayerNoise : MonoBehaviour
{
    [Header("Noise Levels")]
    public float walkNoise = 8f;
    public float runNoise = 16f;
    public float interactNoise = 20f; // เช่นเปิดประตูดัง

    public float noiseCooldown = 0.2f;

    float lastNoiseTime;
    Vector3 lastPos;

    public float CurrentNoiseRadius { get; private set; }

    void Start()
    {
        lastPos = transform.position;
    }

    void Update()
    {
        float dist = Vector3.Distance(transform.position, lastPos);
        lastPos = transform.position;

        if (Time.time - lastNoiseTime >= noiseCooldown)
        {
            // ถ้าเราเดิน/วิ่ง
            if (dist > 0.01f)
            {
                bool isRunning = Input.GetKey(KeyCode.LeftShift);
                EmitNoise(isRunning ? runNoise : walkNoise);
            }
        }
    }

    public void EmitNoise(float radius)
    {
        lastNoiseTime = Time.time;
        CurrentNoiseRadius = radius;

        // ไว้ให้ศัตรู query ทีหลัง (EnemyHearing จะมองหาผู้เล่นใกล้สุดที่ส่งเสียง)
    }
}
