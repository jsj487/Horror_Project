using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float range = 3f;
    [SerializeField] private LayerMask interactMask;

    [Header("UI")]
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private string fallbackPrompt = "E : Interact";

    private IInteractable currentTarget;
    private string lastPrompt;

    private void Start()
    {
        SetPrompt(null);
    }

    private void Update()
    {
        UpdateTargetAndPrompt();

        if (Input.GetKeyDown(KeyCode.E) && currentTarget != null)
        {
            currentTarget.Interact();

            // 상호작용으로 상태가 바뀌면(문 열림/라이트 토글) 프롬프트도 즉시 갱신
            SetPrompt(SafePrompt(currentTarget));
        }
    }

    private void UpdateTargetAndPrompt()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, range, interactMask, QueryTriggerInteraction.Ignore))
        {
            var target = hit.collider.GetComponentInParent<IInteractable>();
            if (target != null)
            {
                currentTarget = target;
                SetPrompt(SafePrompt(target)); // 매 프레임 갱신(상태 기반 프롬프트 대응)
                return;
            }
        }

        currentTarget = null;
        SetPrompt(null);
    }

    private string SafePrompt(IInteractable target)
    {
        if (target == null) return null;

        string p = null;
        try
        {
            p = target.GetPrompt();
        }
        catch
        {
            // 인터페이스 구현 누락/예외 등 방어
            p = null;
        }

        if (string.IsNullOrWhiteSpace(p))
            p = fallbackPrompt;

        return p;
    }

    private void SetPrompt(string message)
    {
        if (promptText == null) return;

        if (string.IsNullOrEmpty(message))
        {
            if (promptText.gameObject.activeSelf)
                promptText.gameObject.SetActive(false);

            lastPrompt = null;
            return;
        }

        if (!promptText.gameObject.activeSelf)
            promptText.gameObject.SetActive(true);

        if (lastPrompt != message)
        {
            promptText.text = message;
            lastPrompt = message;
        }
    }
}
