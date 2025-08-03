using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // REFERENCES
    private Camera cam;

    private Rigidbody rb;
    public Animator anim;
    [SerializeField] private GameObject mesh;

    [SerializeField] private float lookRotationSpeed;

    public bool isDashPressed = false;
    public InputAction movementAction;
    public Vector2 movementVector;
    public Vector3 relativeMovementVector;

    private bool lastDashPressed = false;
    private Vector3 gravity;

    [Header("Movement Stats")]
    [SerializeField] private float acceleration;

    [field: SerializeField] public float MoveSpeed { get; private set; }
    [SerializeField] private float dashForce;

    private bool canDash;
    private float dashTimer;
    [SerializeField] private float dashCooldown;

    [Tooltip("Slerp rotate the player's velocity towards their desired movement direction to turn faster than acceleration allows.")]
    [SerializeField] private float velocitySlerp = 5;

    // ANIMATION
    public float bounceSpeed = 5;
    public float bounceAmplitude = 15;

    public Vector3 LookPoint;

    private Vector3 camForward;
    private Vector3 camRight;


    private void Start()
    {
        gravity = Physics.gravity;
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();

        movementAction = InputSystem.actions.FindAction("Move");
    }

    private void Update()
    {
        movementVector = movementAction.ReadValue<Vector2>();

        if (!canDash)
        {
            dashTimer += Time.deltaTime;

            if (dashTimer >= dashCooldown)
            {
                dashTimer = 0;
                canDash = true;
            }
        }
    }

    private void LateUpdate()
    {
        mesh.transform.position = transform.position;
        if (relativeMovementVector != Vector3.zero)
        {
            Quaternion rotationTarget = Quaternion.LookRotation(relativeMovementVector, transform.up);
            Quaternion rotationYOnly = Quaternion.Euler(transform.rotation.eulerAngles.x, rotationTarget.eulerAngles.y, transform.rotation.eulerAngles.z);
            mesh.transform.rotation = Quaternion.Slerp(mesh.transform.rotation, rotationYOnly, Time.deltaTime * lookRotationSpeed);

            mesh.transform.rotation = Quaternion.Euler(Mathf.Sin(Time.time * bounceSpeed) * bounceAmplitude, mesh.transform.rotation.eulerAngles.y, mesh.transform.rotation.eulerAngles.z);
        }

    }

    private void FixedUpdate()
    {

        bool isDashThisFrame = isDashPressed && isDashPressed != lastDashPressed;
        lastDashPressed = isDashPressed;

        camForward = cam.transform.forward;
        camRight = cam.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        Vector3 forwardRelative = movementVector.y * camForward.normalized;
        Vector3 rightRelative = movementVector.x * camRight.normalized;

        relativeMovementVector = (forwardRelative + rightRelative).normalized * MoveSpeed;
        relativeMovementVector.y = 0f;

        float rateOfVelocityChange = acceleration;

        if (relativeMovementVector.sqrMagnitude > Mathf.Epsilon)
        {
            if (Vector3.Dot(rb.linearVelocity.normalized, relativeMovementVector) > 0)
            { //accelerating
              // rotate the player's velocity towards their desired movement direction faster
              // than acceleration would normally allow so they don't feel like they're on ice.
                float blend = 1 - Mathf.Pow(0.5f, Time.fixedDeltaTime * velocitySlerp);
                rb.linearVelocity = Vector3.RotateTowards(rb.linearVelocity, relativeMovementVector.normalized * rb.linearVelocity.magnitude + Vector3.up * rb.linearVelocity.y, blend, 0);
            }
            else
            { //decelerating
              // abruptly stop when trying to run the opposite way.
                rb.linearVelocity = Vector3.ProjectOnPlane(rb.linearVelocity, relativeMovementVector.normalized);
            }
        }

        if (isDashThisFrame && canDash)
        { //dashInput.WasPressedThisFrame())
            canDash = false;
            rb.AddRelativeForce(new Vector3(relativeMovementVector.x, 0, relativeMovementVector.z) * dashForce, ForceMode.Impulse);
        }

        rb.linearVelocity += Time.fixedDeltaTime * gravity;

        // acceleration
        rb.linearVelocity = Vector3.MoveTowards(rb.linearVelocity, relativeMovementVector + Vector3.up * rb.linearVelocity.y, rateOfVelocityChange * Time.fixedDeltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + relativeMovementVector);

        if (rb != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + rb.linearVelocity);
        }

    }
}