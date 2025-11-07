using UnityEngine;
using UnityEngine.SceneManagement; // ⭐️ (สำคัญ) ต้องมี!

public class SceneLoader : MonoBehaviour
{
    // (ฟังก์ชันนี้ใช้ "ปลดล็อค" เวลา ก่อนโหลดฉากใหม่)
    private void ResumeGameTime()
    {
        Time.timeScale = 1f; // ⭐️ ทำให้เวลาเดินต่อ (สำคัญมาก)
        Cursor.lockState = CursorLockMode.Locked; // ⭐️ (Optional) ซ่อนเมาส์กลับ
        Cursor.visible = false;
    }

    // ⭐️ (ฟังก์ชันนี้จะเชื่อมกับ "ปุ่ม Restart")
    public void RestartGame()
    {
        ResumeGameTime();
        // โหลด "ฉากปัจจุบัน" ซ้ำ
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ⭐️ (ฟังก์ชันนี้จะเชื่อมกับ "ปุ่ม Main Menu")
    public void LoadMainMenu()
    {
        ResumeGameTime();
        // ⭐️ (คุณต้องมีฉากชื่อ "MainMenu" ใน Build Settings)
        SceneManager.LoadScene(0);
    }
}