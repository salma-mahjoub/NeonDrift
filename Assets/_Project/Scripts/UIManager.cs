using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Listens to GameManager events and updates the screens; contains no game logic
public class UIManager : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject mobileControls;
    [SerializeField] private TMP_Text orbCounterText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Button pauseButton;

    [Header("Panels")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text winTimeText;

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button[] restartButtons;

    private GameManager gm;
    private int lastShownSecond = -1;

    void Start()
    {
        gm = GameManager.Instance;

        // Subscribe to game events
        gm.OrbsChanged += UpdateOrbCounter;
        gm.StateChanged += ShowState;
        gm.LevelCompleted += ShowWin;

        // Wire buttons by code
        playButton.onClick.AddListener(gm.StartGame);
        pauseButton.onClick.AddListener(gm.TogglePause);
        resumeButton.onClick.AddListener(gm.TogglePause);
        foreach (Button button in restartButtons)
            button.onClick.AddListener(gm.RestartLevel);

        // Pull the current values in case the GameManager started first
        UpdateOrbCounter(gm.OrbsCollected, gm.OrbsTotal);
        ShowState(gm.State);
    }

    void OnDestroy()
    {
        if (gm == null) return;
        gm.OrbsChanged -= UpdateOrbCounter;
        gm.StateChanged -= ShowState;
        gm.LevelCompleted -= ShowWin;
    }

    void Update()
    {
        // Rewrite the timer only when the displayed second changes (avoids rebuilding the canvas every frame)
        int seconds = Mathf.FloorToInt(gm.ElapsedTime);
        if (seconds != lastShownSecond)
        {
            lastShownSecond = seconds;
            timerText.text = FormatTime(gm.ElapsedTime, false);
        }

        // PC shortcut: Escape toggles pause
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            gm.TogglePause();
    }

    private void UpdateOrbCounter(int collected, int total)
    {
        orbCounterText.text = $"ORBS {collected}/{total}";
    }

    private void ShowState(GameState state)
    {
        startPanel.SetActive(state == GameState.Menu);
        pausePanel.SetActive(state == GameState.Paused);
        winPanel.SetActive(state == GameState.Won);

        hud.SetActive(state != GameState.Menu);
        mobileControls.SetActive(state == GameState.Playing);
        pauseButton.interactable = state == GameState.Playing;
    }

    private void ShowWin(float time)
    {
        winTimeText.text = $"TIME  {FormatTime(time, true)}";
    }

    private static string FormatTime(float time, bool withHundredths)
    {
        int minutes = (int)(time / 60f);
        int seconds = (int)(time % 60f);
        if (!withHundredths) return $"{minutes:00}:{seconds:00}";
        int hundredths = (int)((time * 100f) % 100f);
        return $"{minutes:00}:{seconds:00}.{hundredths:00}";
    }
}