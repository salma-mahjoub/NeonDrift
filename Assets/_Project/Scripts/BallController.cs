using UnityEngine;
using UnityEngine.InputSystem;

// Contrôle de la bille via les INPUT ACTIONS (abstraction : clavier, manette, tactile, XR…)
[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    [Header("Mouvement")]
    [SerializeField] private float moveForce = 12f;
    [SerializeField] private float sprintMultiplier = 1.8f;
    [SerializeField] private float jumpImpulse = 3.5f;

    [Header("Détection")]
    [SerializeField] private float groundCheckDistance = 0.4f;
    [SerializeField] private float bumpThreshold = 2f;

    private Rigidbody rb;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;

    private Vector3 moveInput;
    private bool jumpRequested;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // On récupère les actions par leur nom « ActionMap/Action »
        moveAction   = InputSystem.actions.FindAction("Player/Move");
        jumpAction   = InputSystem.actions.FindAction("Player/Jump");
        sprintAction = InputSystem.actions.FindAction("Player/Sprint");
    }

    // ÉVÉNEMENTS : on s'abonne à l'activation, et on se désabonne TOUJOURS à la désactivation
    void OnEnable()  => jumpAction.performed += OnJump;
    void OnDisable() => jumpAction.performed -= OnJump;

    // SONDAGE : pour une valeur continue (le déplacement), on lit à chaque image
    void Update()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();     // (x, y) entre -1 et 1
        moveInput = new Vector3(input.x, 0f, input.y);       // le Y de l'écran devient le Z du monde 3D
        moveInput = Vector3.ClampMagnitude(moveInput, 1f);

        if (sprintAction.IsPressed())
            moveInput *= sprintMultiplier;
    }

    // Appelée automatiquement quand l'action Jump est validée (performed)
    private void OnJump(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            jumpRequested = true;
            Debug.Log($"🦘 Saut via : {context.control.device.displayName}");
        }
    }

    // PHYSIQUE : les forces sont appliquées dans FixedUpdate
    void FixedUpdate()
    {
        rb.AddForce(moveInput * moveForce, ForceMode.Force);

        if (jumpRequested)
        {
            rb.AddForce(Vector3.up * jumpImpulse, ForceMode.Impulse);
            jumpRequested = false;
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
    }

    void OnCollisionEnter(Collision collision)
    {
        float impact = collision.relativeVelocity.magnitude;
        if (impact > bumpThreshold)
            Debug.Log($"💥 Bump sur {collision.gameObject.name} (force {impact:F1})");
    }
}