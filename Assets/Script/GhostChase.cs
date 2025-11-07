using UnityEngine;

public class GhostChase : MonoBehaviour
{
    private Transform player;
    public float chaseSpeed = 10.0f; 

    void OnEnable()
    {
        try
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        catch
        {
            Debug.LogError("หา Player ไม่เจอ! ลืมตั้ง Tag 'Player' หรือเปล่า?");
            player = null;
        }
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.Translate(direction * chaseSpeed * Time.deltaTime, Space.World);
            transform.LookAt(player);
        }
    }

    void OnDisable()
    {
        player = null; 
    }
}