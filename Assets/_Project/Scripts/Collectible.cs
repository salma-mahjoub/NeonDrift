using UnityEngine;

// Une orbe à ramasser : c'est un TRIGGER, la bille la traverse
[RequireComponent(typeof(Collider))]
public class Collectible : MonoBehaviour
{
    public static int ActiveCount;  // « static » : une seule valeur partagée par toutes les orbes

    void OnEnable()  => ActiveCount++;
    void OnDisable() => ActiveCount--;

    // Reset() est appelée quand on ajoute le composant dans l'éditeur :
    // on en profite pour cocher Is Trigger automatiquement
    void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;  // seule la bille peut ramasser l'orbe

        GameManager.Instance.CollectOrb();
        Destroy(gameObject);
    }
}