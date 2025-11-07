using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Ghost Hunting")]
    [SerializeField] private GhostAI demonGirl;
    [SerializeField] private Transform demonSpawnPoint;

    // ⭐️ (ลบตัวแปรเก่าทิ้ง)
    [Header("Game Over")]
    [SerializeField] private GameObject youDieScreen; // ⭐️ (เก็บไว้แค่อันนี้)

    [Header("Key Fragment Progress")]
    [SerializeField] private int totalKeyFragmentsNeeded = 3;
    [SerializeField] private int collectedKeyFragments = 0;

    [Header("Final Key")]
    public bool hasCarKey = false;   // กุญแจรถ

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void CollectItem(string itemId)
    {
        if (itemId == "KeyFragment")
        {
            collectedKeyFragments++;

            if (collectedKeyFragments == 1)
            {
                if (demonGirl != null && !demonGirl.isHunting)
                {
                    Debug.Log("ผีโผล่เพราะเก็บ Key Fragment ชิ้นที่ 1!");
                    demonGirl.StartHunting(demonSpawnPoint);
                }
            }

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

    // ⭐️ นี่คือฟังก์ชันที่ "HealthComponent" จะเรียก (ถูกต้องแล้ว)
    public void TriggerGameOver()
    {
        Debug.Log("GameManager ได้รับคำสั่ง Game Over!");

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject)
        {
            CharacterController controller = playerObject.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false; // ปิดการเดิน
            }
        }

        GhostAI demonGirl = FindObjectOfType<GhostAI>();
        if (demonGirl != null)
        {
            demonGirl.isHunting = false;
            demonGirl.gameObject.SetActive(false);
        }

        if (youDieScreen != null)
        {
            youDieScreen.SetActive(true);
        }

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // ⭐️ (ลบ Coroutine "ShowGameOverScreen" อันเก่าทิ้งไป) ⭐️
}