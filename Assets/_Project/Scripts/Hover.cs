using UnityEngine;

// TRANSLATION : fait flotter l'objet de haut en bas (mouvement sinusoïdal)
public class Hover : MonoBehaviour
{
    [SerializeField] private float amplitude = 0.15f;  // hauteur du flottement (en mètres)
    [SerializeField] private float frequency = 0.8f;   // nombre d'allers-retours par seconde

    private Vector3 startLocalPosition;
    private float phase;

    void Start()
    {
        startLocalPosition = transform.localPosition;  // on retient la position de départ (en local)
        phase = Random.Range(0f, 10f);                 // décalage aléatoire : les orbes ne bougent pas toutes en même temps
    }

    void Update()
    {
        float offset = Mathf.Sin((Time.time + phase) * frequency * Mathf.PI * 2f) * amplitude;
        transform.localPosition = startLocalPosition + Vector3.up * offset;
    }
}