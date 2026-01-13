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
    [SerializeField] private AudioSource sfxLocked;

    [Header("Start State")]
    [SerializeField] private bool startOpen = true; // 입구문에만 true로

    private bool isLocked;

    private bool isOpen;
    private bool isMoving;
    private Quaternion closedLocalRot;
    private Quaternion openLocalRot;

    public string GetPrompt()
    {
        // 현재 상태 기준으로 "다음 행동"을 표시
        if (isLocked) return "E : Locked";
        return isOpen ? "E : Close" : "E : Open";
    }

    private void Awake()
    {
        closedLocalRot = transform.localRotation;
        openLocalRot = closedLocalRot * Quaternion.Euler(0f, openAngle, 0f);
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
        StartCoroutine(RotateDoor(isOpen ? openLocalRot : closedLocalRot));

        if (sfxSource != null)
            sfxSource.Play();
    }

    private IEnumerator RotateDoor(Quaternion target)
    {
        isMoving = true;
        Quaternion start = transform.localRotation;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.01f, duration);
            transform.localRotation = Quaternion.Slerp(start, target, t);
            yield return null;
        }

        transform.localRotation = target;
        isMoving = false;
    }

    private void Start()
    {
        if (startOpen)
        {
            // 문이 "열린 상태"가 되도록 즉시 회전/상태 세팅
            SetOpenImmediate(true);
        }
    }

    public void SetOpenImmediate(bool open)
    {
        isOpen = open;
        transform.localRotation = open ? openLocalRot : closedLocalRot;
    }

    public void Open()
    {
        if (isMoving) return;
        isOpen = true;
        StopAllCoroutines();
        StartCoroutine(RotateDoor(openLocalRot));
        if (sfxSource != null) sfxSource.Play();
    }

    public void Close()
    {
        if (isMoving) return;
        isOpen = false;
        StopAllCoroutines();
        StartCoroutine(RotateDoor(closedLocalRot));
        if (sfxSource != null) sfxSource.Play();
    }

}
