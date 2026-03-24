using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem; 
#endif

public class PenguinController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float normalSpeed = 12f;
    public float turnSpeed = 120f;

    [Header("Slingshot Settings")]
    public float chargeRate = 150f;
    public float maxLaunchForce = 60f;   // แรงพุ่งไปข้างหน้า
    public float launchUpForce = 25f;    // แรงยกตัว (กระโดดสูง)
    public float cooldownTime = 2f;
    public float speedDecayRate = 1.5f;  // ยิ่งน้อยยิ่งไหลไปไกล

    private float currentLaunchForce = 0f;
    private float cooldownTimer = 0f;
    private float bonusSpeed = 0f;

    [HideInInspector] public bool isCharging = false;
    [HideInInspector] public bool isReady = true;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            // ล็อคแกนไม่ให้ล้ม และตั้งค่า Damping เพื่อความสมูท
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            rb.linearDamping = 0.5f; // เพิ่ม Damping นิดหน่อยเพื่อให้คุมง่ายขึ้นบนพื้นทั่วไป
            rb.angularDamping = 0.1f;
        }
    }

    void Update()
    {
        // ระบบ Cooldown
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            isReady = false;
        }
        else
        {
            isReady = true;
        }

        // ระบบชาร์จและพุ่ง
        if (isReady)
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null)
            {
                if (Keyboard.current.spaceKey.wasPressedThisFrame) { isCharging = true; currentLaunchForce = 0; }
                if (Keyboard.current.spaceKey.wasReleasedThisFrame && isCharging) Launch();
            }
#else
            if (Input.GetKeyDown(KeyCode.Space)) { isCharging = true; currentLaunchForce = 0; }
            if (Input.GetKeyUp(KeyCode.Space) && isCharging) Launch();
#endif

            if (isCharging)
            {
                currentLaunchForce += chargeRate * Time.deltaTime;
                currentLaunchForce = Mathf.Min(currentLaunchForce, maxLaunchForce);
            }
        }
    }

    void Launch()
    {
        // ใส่แรงพุ่งทะยานแบบหนังสติ๊ก
        Vector3 launchDirection = (transform.forward * currentLaunchForce) + (transform.up * launchUpForce);
        rb.AddForce(launchDirection, ForceMode.VelocityChange);

        bonusSpeed = currentLaunchForce;
        cooldownTimer = cooldownTime;
        currentLaunchForce = 0f;
        isCharging = false;
    }

    void FixedUpdate()
    {
        if (rb != null && !rb.isKinematic)
        {
            // ความเร็วปกติ + แรงเฉื่อยจากการพุ่ง
            float moveSpeed = isCharging ? normalSpeed * 0.1f : normalSpeed + bonusSpeed;

            // ค่อยๆ ลดแรงเฉื่อยลง
            bonusSpeed = Mathf.Lerp(bonusSpeed, 0, speedDecayRate * Time.fixedDeltaTime);

            rb.linearVelocity = transform.forward * moveSpeed + new Vector3(0, rb.linearVelocity.y, 0);

            // ระบบเลี้ยว (ใช้ค่า turnSpeed ปกติเสมอ ไม่สนความเร็วแล้ว)
            float turn = 0;
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null) {
                if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) turn = -1;
                if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) turn = 1;
            }
#else
            turn = Input.GetAxis("Horizontal");
#endif
            transform.Rotate(Vector3.up, turn * turnSpeed * Time.fixedDeltaTime);
        }
    }
}