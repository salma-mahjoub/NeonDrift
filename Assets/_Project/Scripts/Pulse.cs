using UnityEngine;

// ÉCHELLE : fait « respirer » l'objet (il grossit puis rétrécit légèrement)
public class Pulse : MonoBehaviour
{
    [SerializeField] private float amount = 0.12f;  // 0.12 = la taille varie de ±12 %
    [SerializeField] private float speed = 4f;

    private Vector3 baseScale;
    private float phase;

    void Start()
    {
        baseScale = transform.localScale;
        phase = Random.Range(0f, 10f);
    }

    void Update()
    {
        float factor = 1f + Mathf.Sin((Time.time + phase) * speed) * amount;
        transform.localScale = baseScale * factor;
    }
}