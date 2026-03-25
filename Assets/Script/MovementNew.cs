using UnityEngine;


public class PenguinRunner : MonoBehaviour
{


    [Header("Basic Movement")]
    public float forwardSpeed = 12f;
    public float strafeSpeed = 15f;
    public float brakeSpeed = 1f;

    [Header("Pounce Settings")]
    public float chargeRate = 120f;
    public float maxDashForce = 60f;
    public float jumpUpForce = 8f;
    public float dashDecay = 2.5f;
    public float maxChargeTime = 0.5f;

    [Header("Ground Check")]
    public float groundCheckDistance = 0.3f;
    public LayerMask groundLayer;

    [Header("System")]
    public float cooldownTime = 5.0f;
    public float landingDelay = 8.0f;

    private float cooldownTimer = 0f;
    private float currentDashBonus = 0f;
    private float chargeAmount = 0f;
    private float currentChargeTimer = 0f;
    private float timeOnGround = 0f;

    private bool isCharging = false;
    private bool isGrounded = true;

    private Animator anim;

    private Rigidbody rb;

    private Vector3 moveDirection = Vector3.forward;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        rb.linearDamping = 0.5f;
    }

    void Update()
    {

        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, groundCheckDistance + 0.1f, groundLayer);

        if (isGrounded) timeOnGround += Time.deltaTime;
        else timeOnGround = 0f;

        if (cooldownTimer > 0) cooldownTimer -= Time.deltaTime;


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


        if (anim != null)
        {

            float animSpeed = isCharging ? 0 : 1f;
            anim.SetFloat("Speed", animSpeed);


            anim.SetBool("isCharging", isCharging);


            anim.SetBool("isPouncing", !isGrounded);


            anim.SetFloat("VerticalVelocity", rb.linearVelocity.y);
        }
    }

    void ExecutePounce()
    {
        isCharging = false;
        currentChargeTimer = 0f;
        cooldownTimer = cooldownTime;


        Vector3 pounceForce = (transform.forward * chargeAmount) + (Vector3.up * jumpUpForce);
        rb.AddForce(pounceForce, ForceMode.VelocityChange);

        currentDashBonus = chargeAmount * 0.4f;
        timeOnGround = 0f;
    }

    public void ChangeDirection(Vector3 newDir)
    {

        moveDirection = newDir.normalized;
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float baseSpeed = isCharging ? brakeSpeed : forwardSpeed;

        currentDashBonus = Mathf.Lerp(currentDashBonus, 0, dashDecay * Time.fixedDeltaTime);
        float finalForwardSpeed = baseSpeed + currentDashBonus;


        Vector3 forwardMove = moveDirection * finalForwardSpeed;
        Vector3 sideDir = Vector3.Cross(Vector3.up, moveDirection);
        Vector3 sideMove = sideDir * (horizontalInput * strafeSpeed);


        Vector3 finalVelocity = forwardMove + sideMove;
        finalVelocity.y = rb.linearVelocity.y;

        rb.linearVelocity = finalVelocity;


        float targetTilt = -horizontalInput * 20f;
        Quaternion targetRotation = Quaternion.Euler(0, transform.eulerAngles.y, targetTilt);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, transform.eulerAngles.y, targetTilt), Time.fixedDeltaTime * 10f);
    }
}