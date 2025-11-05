using UnityEngine;
using TMPro;

public class Note : MonoBehaviour
{
    [TextArea(3, 8)]
    public string noteText;           // ข้อความบนกระดาษ (ใส่ใน Inspector)
    public string interactPrompt = "Press E to read";
    public GameObject promptUI;       // small UI text "Press E" (optional)
    public GameObject readPanel;      // Panel ที่มี TMP สำหรับแสดง note
    public TextMeshProUGUI readText;  // TMP component ใน Panel
    public AudioClip openSfx;
    public bool disableAfterRead = true;

    bool playerInside = false;
    bool readAlready = false;
    AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        if (promptUI) promptUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (readAlready) return;
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            if (promptUI) promptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            if (promptUI) promptUI.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInside && !readAlready && Input.GetKeyDown(KeyCode.E))
        {
            OpenNote();
        }
    }

    public void OpenNote()
    {
        if (openSfx && audioSource) audioSource.PlayOneShot(openSfx);
        if (readPanel && readText != null)
        {
            readPanel.SetActive(true);
            readText.text = noteText;
        }
        if (promptUI) promptUI.SetActive(false);
        readAlready = true;

        if (disableAfterRead)
        {
            // ซ่อนโมเดลกระดาษ (หรือเปลี่ยนเป็นกระดาษฉีก)
            MeshRenderer mr = GetComponent<MeshRenderer>();
            if (mr) mr.enabled = false;
            Collider c = GetComponent<Collider>();
            if (c) c.enabled = false;
        }
    }

    // เรียกฟังก์ชันนี้จากปุ่ม Close ใน Panel
    public void CloseNotePanel()
    {
        if (readPanel) readPanel.SetActive(false);
    }
}
