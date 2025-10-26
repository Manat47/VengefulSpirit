using UnityEngine;
using TMPro; // ถ้าใช้ TextMeshProUGUI

public class FragmentHUD : MonoBehaviour
{
    public TMP_Text counterText;

    void Update()
    {
        // กัน null ทุกเคสที่เป็นไปได้
        if (counterText == null) return;
        if (GameManager.Instance == null) return;

        int have = GameManager.Instance.GetCollectedFragments();
        int need = GameManager.Instance.GetTotalFragmentsNeeded();

        counterText.text = have + "/" + need;
        // ถ้าคุณอยากให้มีคำว่า Fragments ข้างหน้า:
        // counterText.text = "Fragments\n" + have + "/" + need;
    }
}
