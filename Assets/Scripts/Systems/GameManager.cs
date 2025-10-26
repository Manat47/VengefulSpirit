using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int keyFragmentsCollected = 0;
    public int keyFragmentsNeeded = 3;

    private void Awake()
    {
        // singleton ง่าย ๆ
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CollectItem(string itemId)
    {
        if (itemId == "KeyFragment")
        {
            keyFragmentsCollected++;
            Debug.Log("Key Fragment Collected: " + keyFragmentsCollected + "/" + keyFragmentsNeeded);

            // TODO: อัพเดต UI
        }
    }

    public bool HasAllKeyFragments()
    {
        return keyFragmentsCollected >= keyFragmentsNeeded;
    }
}
