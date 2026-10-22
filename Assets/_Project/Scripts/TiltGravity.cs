using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

// Tilting the phone tilts gravity: the ball "falls" toward the lowest edge of the screen
public class TiltGravity : MonoBehaviour
{
    [Header("Tuning")]
    [SerializeField] private float sensitivity = 1.6f;                // 1 = real tilt, >1 = more responsive
    [SerializeField, Range(0f, 0.3f)] private float deadZone = 0.04f; // ignore tiny tilts
    [SerializeField, Range(0.1f, 1f)] private float maxTilt = 0.5f;   // ~30 degrees max
    [SerializeField] private float smoothing = 10f;                   // low-pass filter strength
    [SerializeField] private bool invertX;
    [SerializeField] private bool invertY;

    [Header("UI (optional)")]
    [SerializeField] private GameObject joystickToHide;               // hidden when tilt is available
    [SerializeField] private TMP_Text debugText;                      // shows raw values on the device

    private static readonly Vector3 DefaultGravity = new Vector3(0f, -9.81f, 0f);

    private Vector3 smoothed;
    private Vector3 neutral;
    private bool hasSensor;
    private GameManager gm;

    void OnEnable()
    {
        // Sensor values follow the screen orientation (landscape) instead of the device axes
        InputSystem.settings.compensateForScreenOrientation = true;

        hasSensor = Accelerometer.current != null;   // null in the editor (except with Unity Remote)
        if (hasSensor)
            InputSystem.EnableDevice(Accelerometer.current);

        // Fallback: keep the virtual joystick when there is no sensor
        if (joystickToHide != null)
            joystickToHide.SetActive(!hasSensor);
    }

    void Start()
    {
        gm = GameManager.Instance;
        gm.StateChanged += OnStateChanged;
    }

    void OnDisable()
    {
        if (Accelerometer.current != null)
            InputSystem.DisableDevice(Accelerometer.current);  // save battery
        Physics.gravity = DefaultGravity;                      // gravity is global: always restore it
    }

    void OnDestroy()
    {
        if (gm != null) gm.StateChanged -= OnStateChanged;
    }

    private void OnStateChanged(GameState state)
    {
        // Each time the game starts or resumes, the current hand position becomes "flat"
        if (state == GameState.Playing) Calibrate();
    }

    public void Calibrate()
    {
        if (!hasSensor) return;
        neutral = Accelerometer.current.acceleration.ReadValue();
        smoothed = neutral;
    }

    void Update()
    {
        if (!hasSensor) return;

        Vector3 raw = Accelerometer.current.acceleration.ReadValue();

        // 1) Low-pass filter (frame-rate independent) to remove sensor noise
        float t = 1f - Mathf.Exp(-smoothing * Time.unscaledDeltaTime);
        smoothed = Vector3.Lerp(smoothed, raw, t);

        // 2) Calibration: tilt relative to the neutral position
        Vector2 tilt = new Vector2(smoothed.x - neutral.x, smoothed.y - neutral.y);
        if (invertX) tilt.x = -tilt.x;
        if (invertY) tilt.y = -tilt.y;

        // 3) Dead zone + 4) clamp
        if (tilt.magnitude < deadZone) tilt = Vector2.zero;
        tilt = Vector2.ClampMagnitude(tilt, maxTilt);

        // 5) Screen X -> world X, screen Y -> world Z (the camera looks forward)
        Vector3 horizontal = new Vector3(tilt.x, 0f, tilt.y) * (9.81f * sensitivity);
        Physics.gravity = DefaultGravity + horizontal;

        if (debugText != null)
            debugText.text = $"acc {raw.x:F2} {raw.y:F2} {raw.z:F2}\ntilt {tilt.x:F2} {tilt.y:F2}";
    }
}