using UnityEngine;
using System.Collections;
// ⭐️ เรา "ลบ" using Cinemachine; ทิ้งไปแล้ว

public class CarInteract : MonoBehaviour
{
    [Header("UI Feedback")]
    [SerializeField] private GameObject carFeedbackUI;
    [SerializeField] private float feedbackDisplayTime = 3f;

    [Header("Ending Sequence")]
    [SerializeField] private CanvasGroup endingCanvasGroup;
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private float driveSpeed = 5f;

    // ⭐️ 1. นี่คือส่วนที่เปลี่ยนไป
    [Header("Cameras & Player")]
    [SerializeField] private GameObject carCamera; // ⭐️ ลาก "CarCamera" ที่เราเพิ่งสร้าง มาใส่
    private GameObject playerObject; // ⭐️ เราจะใช้เก็บตัว Player เพื่อ "ซ่อน" เขา

    private bool playerIsNearby = false;
    private bool isEnding = false;
    private bool isDriving = false;

    void Start()
    {
        carFeedbackUI?.SetActive(false);
        endingCanvasGroup.gameObject.SetActive(false);
        carCamera.SetActive(false); // ⭐️ สั่งปิดกล้องรถ (เผื่อลืม)
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isEnding)
        {
            playerIsNearby = true;
            playerObject = other.gameObject; // ⭐️ "จำ" ไว้ว่า Player คือใคร
            InteractionUIManager.Instance.ShowPrompt("Press [E] to Drive");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNearby = false;
            InteractionUIManager.Instance.HidePrompt();
            StopCoroutine(ShowFeedbackMessage());
            carFeedbackUI?.SetActive(false);
        }
    }

    void Update()
    {
        if (playerIsNearby && !isEnding && Input.GetKeyDown(KeyCode.E))
        {
            OnInteract();
        }

        if (isDriving)
        {
            // สั่งให้ "รถ" (transform นี้) ขับไปข้างหน้า
            transform.Translate(Vector3.forward * driveSpeed * Time.deltaTime);
        }
    }

    private void OnInteract()
    {
        InteractionUIManager.Instance.HidePrompt();

        if (GameManager.Instance.hasCarKey)
        {
            Debug.Log("มีกุญแจ! เริ่มฉากจบ...");
            isEnding = true;

            // ⭐️ 2. นี่คือ "การสลับกล้อง"
            if (playerObject != null)
            {
                // ซ่อน Player (และ Main Camera ที่เป็นลูก ก็จะถูกปิดไปด้วย)
                playerObject.SetActive(false);
            }

            // เปิดกล้องในรถ
            carCamera.SetActive(true);

            StartCoroutine(StartEndingSequence());
        }
        else
        {
            Debug.Log("ยังไม่มีกุญแจรถ!");
            StartCoroutine(ShowFeedbackMessage());
        }
    }

    // (Coroutine ที่เหลือ: StartEndingSequence และ ShowFeedbackMessage ... เหมือนเดิมครับ)
    #region Coroutines
    private IEnumerator StartEndingSequence()
    {
        isDriving = true;
        yield return new WaitForSeconds(3f);
        endingCanvasGroup.gameObject.SetActive(true);
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            endingCanvasGroup.alpha = Mathf.Clamp01(timer / fadeDuration);
            yield return null;
        }
        endingCanvasGroup.alpha = 1f;
        Debug.Log("จบเกม...");
    }

    private IEnumerator ShowFeedbackMessage()
    {
        carFeedbackUI.SetActive(true);
        yield return new WaitForSeconds(feedbackDisplayTime);
        carFeedbackUI.SetActive(false);
        if (playerIsNearby && !isEnding)
        {
            InteractionUIManager.Instance.ShowPrompt("Press [E] to Drive");
        }
    }
    #endregion
}