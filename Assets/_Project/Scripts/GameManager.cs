using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { Menu, Playing, Paused, Won }

// Game logic only: it announces what happens through events and never touches the UI directly
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }  // accessible from any script

    // Events the UI listens to
    public event Action<int, int> OrbsChanged;     // (collected, total)
    public event Action<GameState> StateChanged;
    public event Action<float> LevelCompleted;      // final time in seconds

    [SerializeField] private ExitPortal exitPortal;

    // Static: survives a scene reload, so "Restart" skips the start menu
    private static bool hasStartedOnce;

    public int OrbsTotal { get; private set; }
    public int OrbsCollected { get; private set; }
    public bool AllCollected => OrbsCollected >= OrbsTotal;
    public float ElapsedTime { get; private set; }
    public GameState State { get; private set; } = GameState.Menu;

    // Resets static values each time Play is pressed in the editor
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics() => hasStartedOnce = false;

    void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;  // mobile defaults to 30 FPS, we ask for 60
    }

    void Start()
    {
        // Every Collectible.OnEnable (orb counting) has run before this Start
        OrbsTotal = Collectible.ActiveCount;
        OrbsChanged?.Invoke(OrbsCollected, OrbsTotal);
        Debug.Log($"🎯 Orbs to collect: {OrbsTotal}");

        if (hasStartedOnce) StartGame();
        else SetState(GameState.Menu);
    }

    void Update()
    {
        // The timer only runs while playing
        if (State == GameState.Playing)
            ElapsedTime += Time.deltaTime;
    }

    public void StartGame()
    {
        hasStartedOnce = true;
        SetState(GameState.Playing);
    }

    public void TogglePause()
    {
        if (State == GameState.Playing) SetState(GameState.Paused);
        else if (State == GameState.Paused) SetState(GameState.Playing);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;  // always restore normal speed before reloading
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CollectOrb()
    {
        OrbsCollected++;
        Haptics.Pulse();  // short vibration when an orb is collected
        OrbsChanged?.Invoke(OrbsCollected, OrbsTotal);
        Debug.Log($"✨ Orb {OrbsCollected}/{OrbsTotal}");

        if (AllCollected)
        {
            Debug.Log("🌀 All orbs collected! The portal is open.");
            exitPortal.Open();
        }
    }

    public void LevelComplete()
    {
        if (State != GameState.Playing) return;  // ignore if already won or paused
        Debug.Log($"🏁 LEVEL COMPLETE in {ElapsedTime:F2} s");
        SetState(GameState.Won);
        Haptics.Pulse();  // victory vibration
        LevelCompleted?.Invoke(ElapsedTime);
    }

    private void SetState(GameState newState)
    {
        State = newState;
        // 0 freezes physics and deltaTime (menus, pause); 1 = normal speed
        Time.timeScale = newState == GameState.Playing ? 1f : 0f;
        StateChanged?.Invoke(newState);
    }
}