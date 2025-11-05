using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Key Fragment Progress")]
    [SerializeField] private int totalKeyFragmentsNeeded = 3;
    [SerializeField] private int collectedKeyFragments = 0;

    [Header("Final Key")]
    public bool hasCarKey = false;   // กุญแจรถ (field ธรรมดา ใช้ Header ได้)

    private void Awake()
    {
        // Simple Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    public void CollectItem(string itemId)
    {
        if (itemId == "KeyFragment")
        {
            collectedKeyFragments++;
            if (collectedKeyFragments > totalKeyFragmentsNeeded)
                collectedKeyFragments = totalKeyFragmentsNeeded;

            Debug.Log($"Key Fragment Collected: {collectedKeyFragments}/{totalKeyFragmentsNeeded}");
        }
        else if (itemId == "CarKey")
        {
            hasCarKey = true;
            Debug.Log("Car key collected.");
        }
    }

    public bool HasAllKeyFragments()
    {
        return collectedKeyFragments >= totalKeyFragmentsNeeded;
    }

    public int GetCollectedFragments()
    {
        return collectedKeyFragments;
    }

    public int GetTotalFragmentsNeeded()
    {
        return totalKeyFragmentsNeeded;
    }
}
