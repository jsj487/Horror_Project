using System.Collections;
using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    [Header("Door")]
    [SerializeField] private float openAngle = 90f;      // Y축 회전
    [SerializeField] private float duration = 0.2f;      // 열리는 시간

    [Header("Audio (Optional)")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Lock")]
    [SerializeField] private bool startLocked = true;
    [SerializeField] private AudioSource sfxLocked; // 없어도 됨

    private bool isLocked;

    private bool isOpen;
    private bool isMoving;
    private Quaternion closedRot;
    private Quaternion openRot;

    public string GetPrompt()
    {
        // 현재 상태 기준으로 "다음 행동"을 표시
        if (isLocked) return "E : Locked";
        return isOpen ? "E : Close" : "E : Open";
    }

    private void Awake()
    {
        closedRot = transform.rotation;
        openRot = closedRot * Quaternion.Euler(0f, openAngle, 0f);
        isLocked = startLocked;
    }

    public void Unlock()
    {
        isLocked = false;
    }

    public void Lock()
    {
        isLocked = true;
    }

    public void Interact()
    {
        if (isLocked)
        {
            if (sfxLocked != null) sfxLocked.Play();
            return;
        }

        if (isMoving) return;

        isOpen = !isOpen;
        StopAllCoroutines();
        StartCoroutine(RotateDoor(isOpen ? openRot : closedRot));

        if (sfxSource != null)
            sfxSource.Play();
    }

    private IEnumerator RotateDoor(Quaternion target)
    {
        isMoving = true;
        Quaternion start = transform.rotation;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.01f, duration);
            transform.rotation = Quaternion.Slerp(start, target, t);
            yield return null;
        }

        transform.rotation = target;
        isMoving = false;
    }
}
