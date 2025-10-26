using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public Light spot;
    public KeyCode toggleKey = KeyCode.F;

    [Header("Battery")]
    public float battery = 100f;
    public float drainPerSecond = 1f;
    public float minIntensity = 200f;
    public float maxIntensity = 1200f;

    bool isOn = true;

    void Update()
    {
        // toggle on/off
        if (Input.GetKeyDown(toggleKey))
        {
            isOn = !isOn;
            ApplyLight();
            // TODO: ส่งเสียง "click" ให้ผีได้ยินผ่าน PlayerNoise
        }

        if (isOn && battery > 0f)
        {
            battery -= drainPerSecond * Time.deltaTime;
            if (battery < 0f) battery = 0f;

            float t = battery / 100f;
            if (spot)
                spot.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);

            if (battery <= 0f) // หมดแบต ปิดเอง
            {
                isOn = false;
                ApplyLight();
            }
        }
    }

    void ApplyLight()
    {
        if (spot)
            spot.enabled = isOn && battery > 0f;
    }
}
