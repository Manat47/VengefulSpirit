using UnityEngine;
using Cinemachine;

public class CarOnPath : MonoBehaviour
{
    [Header("Refs")]
    public CinemachineDollyCart cart;
    public Rigidbody rb;
    public Transform lateral;

    [Header("Move")]
    public float normalSpeed = 12f;
    public float blackoutSpeed = 10f;  
    public AnimationCurve veerCurve;   
    public float veerWidth = 2.0f;     
    public float veerDuration = 2.0f;  

    [Header("Crash handoff")]
    public float physicsEnableLead = 0.3f;
    bool blackingOut = false;
    float veerT = 0f;
    float timeToCrash = 999f;

    void Reset() { rb = GetComponent<Rigidbody>(); cart = GetComponent<CinemachineDollyCart>(); }

    void Start()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
        if (!lateral) lateral = transform;
        if (cart == null) cart = GetComponent<Cinemachine.CinemachineDollyCart>();
        rb.isKinematic = true;
        if (cart) cart.m_Speed = normalSpeed;   // มีค่อยตั้ง
    }

    public void BeginBlackout(float secondsUntilCrash)
    {
        if (blackingOut) return;
        blackingOut = true;
        veerT = 0f;
        timeToCrash = secondsUntilCrash; 
        cart.m_Speed = blackoutSpeed;
    }

    void Update()
    {
        if (!blackingOut) return;

        if (veerDuration > 0f && veerCurve != null)
        {
            veerT = Mathf.Min(veerT + Time.deltaTime / veerDuration, 1f);
            float offset = veerCurve.Evaluate(veerT) * veerWidth;
            var lp = lateral.localPosition;
            lp.x = offset;
            lateral.localPosition = lp;
        }

        timeToCrash -= Time.deltaTime;
        if (timeToCrash <= 0f)
        {
            HandoffToPhysics();
        }
    }

    public void HandoffToPhysics()
    {
        if (!rb || !rb.isKinematic) return;
        if (cart) cart.m_Speed = 0f;       
        rb.isKinematic = false;
        rb.velocity = transform.forward * blackoutSpeed;
        enabled = false;
    }
}
