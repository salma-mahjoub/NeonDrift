using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

// Gestes : SWIPE = dash de la bille ; PINCH (mobile) ou MOLETTE (PC) = zoom de la caméra
[RequireComponent(typeof(Camera))]
public class TouchGestures : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private Rigidbody ballRigidbody;

    [Header("Swipe → Dash")]
    [SerializeField] private float swipeMinDistanceCm = 1.5f;  // en cm, pas en pixels
    [SerializeField] private float swipeMaxDuration = 0.35f;   // en secondes
    [SerializeField] private float dashSpeed = 4f;             // ForceMode.VelocityChange
    [SerializeField, Range(0f, 1f)] private float controlsZoneHeight = 0.45f; // bas de l'écran = joystick et bouton

    [Header("Zoom")]
    [SerializeField] private float minFov = 35f;
    [SerializeField] private float maxFov = 70f;
    [SerializeField] private float pinchSensitivity = 0.05f;
    [SerializeField] private float scrollStep = 2f;

    private Camera cam;
    private Vector3 pendingDash;  // lien Update → FixedUpdate (règle du chapitre 3)

    void Awake() => cam = GetComponent<Camera>();

    void OnEnable()  => EnhancedTouchSupport.Enable();
    void OnDisable() => EnhancedTouchSupport.Disable();

    void Update()
    {
        // --- MOBILE : deux doigts = pinch ---
        var touches = Touch.activeTouches;
        if (touches.Count == 2)
        {
            HandlePinch(touches[0], touches[1]);
        }
        else
        {
            // --- MOBILE : un doigt qui se lève = peut-être un swipe ---
            foreach (Touch t in touches)
                if (t.phase == TouchPhase.Ended)
                    TryDetectSwipe(t);
        }

        // --- PC : molette ou trackpad = zoom ---
        if (Mouse.current != null)
        {
            float scroll = Mouse.current.scroll.ReadValue().y;
            if (Mathf.Abs(scroll) > 0.01f)
                SetFov(cam.fieldOfView - Mathf.Sign(scroll) * scrollStep);
        }
    }

    void FixedUpdate()
    {
        if (pendingDash != Vector3.zero)
        {
            ballRigidbody.AddForce(pendingDash, ForceMode.VelocityChange);
            pendingDash = Vector3.zero;
        }
    }

    private void TryDetectSwipe(Touch t)
    {
        // 1) On ignore les gestes qui commencent dans la zone des contrôles (en bas)
        if (t.startScreenPosition.y < Screen.height * controlsZoneHeight) return;

        // 2) La durée et la distance du geste
        float duration = (float)(t.time - t.startTime);
        Vector2 deltaPx = t.screenPosition - t.startScreenPosition;
        float dpi = Screen.dpi > 0 ? Screen.dpi : 160f;          // certains appareils renvoient 0
        float distanceCm = deltaPx.magnitude / dpi * 2.54f;

        if (distanceCm < swipeMinDistanceCm || duration > swipeMaxDuration) return;

        // 3) La direction : le haut de l'écran correspond à +Z dans le monde (la caméra regarde vers +Z)
        Vector3 dir = new Vector3(deltaPx.x, 0f, deltaPx.y).normalized;
        pendingDash = dir * dashSpeed;
        Debug.Log($"⚡ Swipe : {distanceCm:F1} cm en {duration:F2} s → dash");
    }

    private void HandlePinch(Touch a, Touch b)
    {
        // La distance entre les deux doigts maintenant, et à l'image précédente (position - delta)
        float previous = Vector2.Distance(a.screenPosition - a.delta, b.screenPosition - b.delta);
        float current  = Vector2.Distance(a.screenPosition, b.screenPosition);
        float diff = current - previous;                          // > 0 : les doigts s'écartent

        SetFov(cam.fieldOfView - diff * pinchSensitivity);        // écarter = zoomer (FOV plus petit)
    }

    private void SetFov(float fov) => cam.fieldOfView = Mathf.Clamp(fov, minFov, maxFov);
}