using UnityEngine;

// Le « chef d'orchestre » du niveau : il compte les orbes et ouvre le portail
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }  // accessible depuis n'importe quel script

    [SerializeField] private ExitPortal exitPortal;

    public int OrbsTotal { get; private set; }
    public int OrbsCollected { get; private set; }
    public bool AllCollected => OrbsCollected >= OrbsTotal;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Tous les OnEnable (donc le comptage des orbes) ont eu lieu avant ce Start
        OrbsTotal = Collectible.ActiveCount;
        Debug.Log($"🎯 Orbes à collecter : {OrbsTotal}");
    }

    public void CollectOrb()
    {
        OrbsCollected++;
        Debug.Log($"✨ Orbe {OrbsCollected}/{OrbsTotal}");

        if (AllCollected)
        {
            Debug.Log("🌀 Toutes les orbes ! Le portail est ouvert.");
            exitPortal.Open();
        }
    }

    public void LevelComplete()
    {
        Debug.Log("🏁 NIVEAU TERMINÉ !");
    }
}