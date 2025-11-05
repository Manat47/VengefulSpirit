using UnityEngine;

public class BlackoutTrigger : MonoBehaviour
{
    public CarOnPath carA;           // ลาก CarA มาวาง
    public float secondsUntilCrash = 1.2f; // กะเวลาให้ชนตรงภาพ

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return; // ให้ CarA ใส่ Tag "Player"
        carA?.BeginBlackout(secondsUntilCrash);
        GetComponent<Collider>().enabled = false;
    }
}
