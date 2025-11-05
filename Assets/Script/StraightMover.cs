
using UnityEngine;
public class StraightMover : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 40f;
    void Reset() { rb = GetComponent<Rigidbody>(); }
    void FixedUpdate()
    {
        if (rb.isKinematic)
        {
            rb.MovePosition(rb.position + transform.forward * speed * Time.fixedDeltaTime);
        }
    }
}
