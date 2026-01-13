using System.Collections;
using UnityEngine;

public class KeypadSolvedSequence : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private DoorInteractable entranceDoor; // 처음 들어온 Door_Hinge
    [SerializeField] private GameObject silhouetteRoot;     // Silhouette_Entrance
    [SerializeField] private AudioSource stinger;           // 선택

    [Header("Timing")]
    [SerializeField] private float silhouetteDuration = 0.8f;
    [SerializeField] private float closeDelay = 0.15f; // 실루엣 보이고 살짝 후 닫히게

    [Header("After Close")]
    [SerializeField] private bool lockAfterClose = true;

    [Header("Camera Focus (Optional)")]
    [SerializeField] private Transform playerCamera;          // Player_Camera의 Transform
    [SerializeField] private MonoBehaviour lookScript;         // FirstPersonLook
    [SerializeField] private MonoBehaviour moveScript;         // FirstPersonMove
    [SerializeField] private Transform lookTarget;             // 문(또는 문 근처 Empty)의 Transform
    [SerializeField] private float focusDuration = 1.2f;        // 총 고정 시간
    [SerializeField] private float rotateSpeed = 8f;            // 시점 회전 속도
    [SerializeField] private float preFocusTime = 0.6f;
    [SerializeField] private bool unlockCursorDuringFocus = false;


    private bool played;

    public void Play()
    {
        if (played) return;
        played = true;
        StartCoroutine(CoPlay());
    }

    private IEnumerator CoPlay()
    {
        SetControlLocked(true);

        if (silhouetteRoot != null) silhouetteRoot.SetActive(true);
        if (stinger != null) stinger.Play();

        // 1) 먼저 시점 고정(문을 바라보게 만들기)
        float t = 0f;
        while (t < preFocusTime)
        {
            t += Time.deltaTime;
            FocusStep();
            yield return null;
        }

        // 2) 약간의 여유(원하면 0으로)
        if (closeDelay > 0f)
            yield return new WaitForSeconds(closeDelay);

        // 3) 이제 문을 닫고 잠금
        if (entranceDoor != null)
        {
            entranceDoor.Close();
            if (lockAfterClose) entranceDoor.Lock();
        }

        // 4) 문 닫히는 동안 계속 포커스 유지
        float elapsed = 0f;
        while (elapsed < silhouetteDuration)
        {
            elapsed += Time.deltaTime;
            FocusStep();
            yield return null;
        }

        if (silhouetteRoot != null) silhouetteRoot.SetActive(false);

        SetControlLocked(false);
    }


    private void FocusStep()
    {
        if (playerCamera == null || lookTarget == null) return;

        Vector3 dir = (lookTarget.position - playerCamera.position);
        dir.y = 0f; // 상하 흔들림 싫으면 고정(원하면 이 줄 제거)

        if (dir.sqrMagnitude < 0.0001f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir.normalized, Vector3.up);
        playerCamera.rotation = Quaternion.Slerp(playerCamera.rotation, targetRot, Time.deltaTime * rotateSpeed);
    }

    private void SetControlLocked(bool locked)
    {
        if (lookScript != null) lookScript.enabled = !locked;
        if (moveScript != null) moveScript.enabled = !locked;

        if (!unlockCursorDuringFocus) return;

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
