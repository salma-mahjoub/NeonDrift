using UnityEngine;

// ROTATION : fait tourner l'objet en continu
public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 degreesPerSecond = new Vector3(0f, 90f, 0f);
    [SerializeField] private Space space = Space.World;

    void Update()
    {
        // * Time.deltaTime : même vitesse quel que soit le nombre de FPS
        transform.Rotate(degreesPerSecond * Time.deltaTime, space);
    }
}