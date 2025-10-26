using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Ending UI")]
    public GameObject endingPanel;   // Panel ดำเต็มจอ + Text ข้างใน
    public TMP_Text endingText;      // ข้อความตอนจบ

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        Time.timeScale = 1f; // reset เผื่อเล่นใหม่
        if (endingPanel != null)
            endingPanel.SetActive(false);
    }

    public void ShowEnding(string message)
    {
        // เปิดจอจบ
        if (endingPanel != null)
            endingPanel.SetActive(true);

        if (endingText != null)
            endingText.text = string.IsNullOrEmpty(message)
                ? "You may run from here,\nbut you can't outrun the truth."
                : message;

        // หยุดเกม
        Time.timeScale = 0f;

        // ปลดเมาส์ ให้ดูข้อความ
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // ปิดการควบคุมผู้เล่น
        DisablePlayerControl();
    }

    void DisablePlayerControl()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        // 1) ปิดสคริปต์ควบคุมเดิน/หมุน
        var ctrl = player.GetComponent<PlayerController>();
        if (ctrl != null) ctrl.enabled = false;

        // 2) ปิด CharacterController ก็ได้ถ้าอยากให้หยุดเด้ง
        var cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;
    }

    public void ShowNeedMoreMessage()
    {
        Debug.Log("ยังหากุญแจไม่ครบ / need more fragments");
        // ตรงนี้คุณจะปล่อย Debug.Log ไว้ก่อนก็ได้
        // หรือทำ popup ชั่วคราวทีหลัง
    }

    // ปุ่ม restart / quit (ไว้ต่อยอด UI)
    public void Restart()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
