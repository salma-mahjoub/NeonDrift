using UnityEngine;
using UnityEngine.InputSystem;

// Contrôle de la bille au clavier (temporaire : au chapitre 6, ce sera l'inclinaison du téléphone)
[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour
{
    [Header("Mouvement")]
    [SerializeField] private float moveForce = 12f;      // ForceMode.Force : poussée continue
    [SerializeField] private float jumpImpulse = 3.5f;   // ForceMode.Impulse : coup unique

    [Header("Détection")]
    [SerializeField] private float groundCheckDistance = 0.4f;
    [SerializeField] private float bumpThreshold = 2f;

    private Rigidbody rb;
    private Vector3 moveInput;      // fait le lien entre Update et FixedUpdate
    private bool jumpRequested;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();  // une seule fois, et pas dans Update
    }

    // 1) LIRE les entrées : à chaque image
    void Update()
    {
        Keyboard kb = Keyboard.current;
        if (kb == null) return;

        float x = 0f, z = 0f;
        if (kb.leftArrowKey.isPressed  || kb.aKey.isPressed) x -= 1f;
        if (kb.rightArrowKey.isPressed || kb.dKey.isPressed) x += 1f;
        if (kb.upArrowKey.isPressed    || kb.wKey.isPressed) z += 1f;
        if (kb.downArrowKey.isPressed  || kb.sKey.isPressed) z -= 1f;

        moveInput = new Vector3(x, 0f, z).normalized;  // normalized : la diagonale ne va pas plus vite

        if (kb.spaceKey.wasPressedThisFrame && IsGrounded())
            jumpRequested = true;
    }

    // 2) APPLIQUER les forces : à intervalle fixe, synchronisé avec la physique
    void FixedUpdate()
    {
        rb.AddForce(moveInput * moveForce, ForceMode.Force);

        if (jumpRequested)
        {
            rb.AddForce(Vector3.up * jumpImpulse, ForceMode.Impulse);
            jumpRequested = false;
        }
    }

    // RAYCAST : un rayon vers le bas pour savoir si la bille touche le sol
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
    }

    // COLLISION : appelée au moment d'un impact avec un objet solide
    void OnCollisionEnter(Collision collision)
    {
        float impact = collision.relativeVelocity.magnitude;
        if (impact > bumpThreshold)
            Debug.Log($"💥 Bump sur {collision.gameObject.name} (force {impact:F1})");
    }
}