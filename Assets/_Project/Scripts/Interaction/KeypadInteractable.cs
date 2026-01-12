using UnityEngine;
using TMPro;

public class KeypadInteractable : MonoBehaviour, IInteractable
{
    [Header("Keypad")]
    [SerializeField] private string correctCode = "1234";
    [SerializeField] private int codeLength = 4;

    [Header("Targets")]
    [SerializeField] private DoorInteractable doorToUnlock;

    [Header("UI")]
    [SerializeField] private TMP_Text keypadText; // UI/KeypadText 연결

    [Header("Audio (Optional)")]
    [SerializeField] private AudioSource sfxOk;
    [SerializeField] private AudioSource sfxFail;

    [Header("Lock Control (Optional)")]
    [SerializeField] private MonoBehaviour lookScript; // FirstPersonLook 연결
    [SerializeField] private MonoBehaviour moveScript; // FirstPersonMove 연결
    [SerializeField] private bool unlockCursor = true;

    private bool isEntering;
    private string buffer = "";

    public string GetPrompt()
    {
        if (EventFlags.Instance != null && EventFlags.Instance.keypadSolved)
            return "E : Interact"; // 이미 해결된 경우 (원하면 "E : Use" 같은 문구로 바꿔도 됨)

        return isEntering ? "E : Exit" : "E : Use Keypad";
    }

    public void Interact()
    {
        if (EventFlags.Instance != null && EventFlags.Instance.keypadSolved)
            return;

        isEntering = !isEntering;

        if (isEntering)
        {
            buffer = "";
            SetInputLocked(true);
            ShowUI();
        }
        else
        {
            SetInputLocked(false);
            HideUI();
        }
    }


    private void Update()
    {
        if (!isEntering) return;

        // ESC로 종료
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isEntering = false;
            SetInputLocked(false);
            HideUI();
            return;
        }


        // Backspace
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (buffer.Length > 0)
                buffer = buffer.Substring(0, buffer.Length - 1);

            RefreshUI();
            return;
        }

        // Enter 제출
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Submit();
            return;
        }

        // 숫자 입력(0~9)
        // inputString은 그 프레임에 들어온 문자들이라 간단히 처리 가능
        var s = Input.inputString;
        if (!string.IsNullOrEmpty(s))
        {
            foreach (char c in s)
            {
                if (c < '0' || c > '9') continue;
                if (buffer.Length >= codeLength) break;

                buffer += c;
            }

            RefreshUI();
        }
    }

    private void Submit()
    {
        if (buffer.Length != codeLength)
        {
            Fail("Need 4 digits");
            return;
        }

        if (buffer == correctCode)
        {
            Succeed();
        }
        else
        {
            Fail("Access Denied");
        }
    }

    private void Succeed()
    {
        if (EventFlags.Instance != null)
            EventFlags.Instance.keypadSolved = true;

        if (doorToUnlock != null)
            doorToUnlock.Unlock();

        if (sfxOk != null) sfxOk.Play();

        isEntering = false;
        SetInputLocked(false);
        HideUI();

        Debug.Log("[Keypad] Solved");
    }

    private void Fail(string msg)
    {
        if (sfxFail != null) sfxFail.Play();
        buffer = "";
        ShowUI(msg);
        Debug.Log($"[Keypad] Fail: {msg}");
    }

    private void ShowUI(string status = null)
    {
        if (keypadText == null) return;
        keypadText.gameObject.SetActive(true);
        RefreshUI(status);
    }

    private void HideUI()
    {
        if (keypadText == null) return;
        keypadText.text = "";
        keypadText.gameObject.SetActive(false);
    }

    private void RefreshUI(string status = null)
    {
        if (keypadText == null) return;

        string masked = new string('*', buffer.Length).PadRight(codeLength, '_');
        if (string.IsNullOrEmpty(status))
            keypadText.text = $"CODE: {masked}\nEnter: Submit / Backspace: Delete / Esc: Exit";
        else
            keypadText.text = $"{status}\nCODE: {masked}\nEnter: Submit / Backspace: Delete / Esc: Exit";
    }

    private void SetInputLocked(bool locked)
    {
        if (lookScript != null) lookScript.enabled = !locked;
        if (moveScript != null) moveScript.enabled = !locked;

        if (!unlockCursor) return;

        if (locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

}
