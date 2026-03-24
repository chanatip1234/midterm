using UnityEngine;

public class PenguinRunner : MonoBehaviour
{
    [Header("Basic Movement")]
    public float forwardSpeed = 12f;      // เพิ่มความเร็วปกติอีกนิดให้เกมสนุกขึ้น
    public float strafeSpeed = 15f;
    public float brakeSpeed = 1f;

    [Header("Pounce Settings")]
    public float chargeRate = 120f;        // *** ปรับเพิ่ม: เพื่อให้ชาร์จเต็มไวขึ้นใน 1 วินาที
    public float maxDashForce = 60f;      // *** ปรับเพิ่ม: ให้พุ่งแรงสะใจขึ้น
    public float jumpUpForce = 8f;        // *** ปรับลด: ให้พุ่งไปข้างหน้ามากกว่าโดดขึ้นสูง (ฟีลกระโจน)
    public float dashDecay = 2.5f;        // ให้แรงพุ่งค่อยๆ หมดไปแบบนุ่มนวล
    public float maxChargeTime = 0.5f;    // *** ล็อคไว้ที่ 1 วินาทีตามที่ต้องการ

    [Header("Ground Check")]
    public float groundCheckDistance = 0.3f;
    public LayerMask groundLayer;

    [Header("System")]
    public float cooldownTime = 5.0f;     // คูลดาวน์ 5 วินาที
    public float landingDelay = 8.0f;     // *** ปรับเพิ่ม: ต้องวิ่งบนพื้นสักพักถึงจะเริ่มชาร์จใหม่ได้

    private float cooldownTimer = 0f;
    private float currentDashBonus = 0f;
    private float chargeAmount = 0f;
    private float currentChargeTimer = 0f;
    private float timeOnGround = 0f;

    private bool isCharging = false;
    private bool isGrounded = true;

    private Animator anim;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // ล็อคแกนทั้งหมดเพื่อความเสถียร
        anim = GetComponentInChildren<Animator>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        rb.linearDamping = 0.5f; // เพิ่มความหน่วงฟิสิกส์เล็กน้อย
    }

    void Update()
    {
        // --- 1. ระบบเช็คพื้น (ห้ามลบ) ---
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, groundCheckDistance + 0.1f, groundLayer);

        if (isGrounded) timeOnGround += Time.deltaTime;
        else timeOnGround = 0f;

        if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;

        // --- 2. ระบบชาร์จและ Input (ห้ามลบ) ---
        if (isGrounded && timeOnGround >= landingDelay && cooldownTimer <= 0 && Input.GetKeyDown(KeyCode.Space))
        {
            isCharging = true;
            chargeAmount = 0f;
            currentChargeTimer = 0f;
        }

        if (isCharging)
        {
            currentChargeTimer += Time.deltaTime;
            chargeAmount += chargeRate * Time.deltaTime;
            chargeAmount = Mathf.Min(chargeAmount, maxDashForce);

            if (currentChargeTimer >= maxChargeTime || Input.GetKeyUp(KeyCode.Space))
            {
                ExecutePounce();
            }
        }

        if (!isGrounded && isCharging) isCharging = false;

        // --- 3. ส่วนการส่งค่าไปที่ Animator (เพิ่มต่อท้ายตรงนี้!) ---
        if (anim != null)
        {
            // ส่งความเร็วไปบอก Animator (ถ้าชาร์จอยู่ให้หยุดเดิน/วิ่ง)
            float animSpeed = isCharging ? 0 : 1f;
            anim.SetFloat("Speed", animSpeed);

            // ส่งสถานะชาร์จ
            anim.SetBool("isCharging", isCharging);

            // ส่งสถานะลอยตัว (ถ้าไม่ติดพื้น = True)
            anim.SetBool("isPouncing", !isGrounded);

            // ส่งความเร็วแนวตั้ง (ใช้สลับท่า Air/Fall)
            anim.SetFloat("VerticalVelocity", rb.linearVelocity.y);
        }
    }

    void ExecutePounce()
    {
        isCharging = false;
        currentChargeTimer = 0f;
        cooldownTimer = cooldownTime;

        // ใส่แรงพุ่ง (Forward + Up)
        Vector3 pounceForce = (transform.forward * chargeAmount) + (Vector3.up * jumpUpForce);
        rb.AddForce(pounceForce, ForceMode.VelocityChange);

        currentDashBonus = chargeAmount * 0.4f; // ให้ความเร็วต่อเนื่องหลังพุ่ง
        timeOnGround = 0f;
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float baseSpeed = isCharging ? brakeSpeed : forwardSpeed;

        currentDashBonus = Mathf.Lerp(currentDashBonus, 0, dashDecay * Time.fixedDeltaTime);
        float finalForwardSpeed = baseSpeed + currentDashBonus;

        Vector3 vel = new Vector3(horizontalInput * strafeSpeed, rb.linearVelocity.y, finalForwardSpeed);
        rb.linearVelocity = vel;

        // เอียงตัวตอนเลี้ยว (Z Axis)
        float targetTilt = -horizontalInput * 20f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, targetTilt), Time.fixedDeltaTime * 10f);
    }
}