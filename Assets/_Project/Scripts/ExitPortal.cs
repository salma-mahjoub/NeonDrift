using UnityEngine;

// Le portail de sortie : fermé tant qu'il reste des orbes
public class ExitPortal : MonoBehaviour
{
    [SerializeField] private GameObject openEffect;  // ce qui s'allume quand le portail s'ouvre

    private bool isOpen;

    void Start()
{
    if (openEffect == null)
    {
        // « this » en 2e paramètre : un clic sur l'erreur sélectionne l'objet fautif dans la Hierarchy
        Debug.LogError($"[ExitPortal] Le champ Open Effect est vide sur l'objet « {name} »", this);
        return;  // on s'arrête proprement au lieu de planter
    }
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