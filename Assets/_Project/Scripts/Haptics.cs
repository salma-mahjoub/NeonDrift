using UnityEngine;

// Shared helper so every script vibrates the same way, with a global on/off switch
public static class Haptics
{
    public static bool Enabled = true;

    private const float MinInterval = 0.15f;  // avoid continuous buzzing
    private static float lastTime = -1f;

    public static void Pulse()
    {
        if (!Enabled) return;
        if (Time.unscaledTime - lastTime < MinInterval) return;
        lastTime = Time.unscaledTime;

#if UNITY_IOS || UNITY_ANDROID
        Handheld.Vibrate();  // does nothing in the editor, vibrates on the device
#endif
    }
}