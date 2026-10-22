using UnityEngine;

// Le portail de sortie : fermé tant qu'il reste des orbes
public class ExitPortal : MonoBehaviour
{
    [SerializeField] private GameObject openEffect;  // ce qui s'allume quand le portail s'ouvre

    private bool isOpen;

    void Start()
    {
        openEffect.SetActive(false);
    }

    public void Open()
    {
        isOpen = true;
        openEffect.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (isOpen)
            GameManager.Instance.LevelComplete();
        else
            Debug.Log($"🔒 Portail fermé : il reste {GameManager.Instance.OrbsTotal - GameManager.Instance.OrbsCollected} orbe(s).");
    }
}