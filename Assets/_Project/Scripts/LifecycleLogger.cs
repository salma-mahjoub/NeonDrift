using UnityEngine;

// Affiche dans la Console l'ordre d'appel des méthodes du cycle de vie
public class LifecycleLogger : MonoBehaviour
{
    private bool fixedLogged, updateLogged, lateLogged;

    void Awake()     => Debug.Log($"[{name}] 1. Awake");
    void OnEnable()  => Debug.Log($"[{name}] 2. OnEnable");
    void Start()     => Debug.Log($"[{name}] 3. Start");

    void FixedUpdate()
    {
        if (!fixedLogged) { Debug.Log($"[{name}] 4. FixedUpdate (physique, intervalle fixe)"); fixedLogged = true; }
    }

    void Update()
    {
        if (!updateLogged) { Debug.Log($"[{name}] 5. Update (chaque image)"); updateLogged = true; }
    }

    void LateUpdate()
    {
        if (!lateLogged) { Debug.Log($"[{name}] 6. LateUpdate (après tous les Update)"); lateLogged = true; }
    }

    void OnDisable() => Debug.Log($"[{name}] 7. OnDisable");
    void OnDestroy() => Debug.Log($"[{name}] 8. OnDestroy");
}