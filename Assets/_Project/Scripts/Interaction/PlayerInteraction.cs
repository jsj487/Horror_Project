using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float range = 3f;
    [SerializeField] private LayerMask interactMask; // Interactable 레이어만

    [Header("UI (Optional)")]
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private string promptMessage = "E : Interact";

    private IInteractable currentTarget;

    private void Start()
    {
        SetPrompt(false);
    }

    private void Update()
    {
        UpdateTarget();

        if (Input.GetKeyDown(KeyCode.E) && currentTarget != null)
        {
            currentTarget.Interact();
        }
    }

    private void UpdateTarget()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, range, interactMask, QueryTriggerInteraction.Ignore))
        {
            // 자식 collider 맞아도 부모에서 찾기
            var target = hit.collider.GetComponentInParent<IInteractable>();

            if (target != null)
            {
                // 타겟이 바뀌었을 때만 갱신(불필요한 SetActive/텍스트 변경 방지)
                if (!ReferenceEquals(currentTarget, target))
                {
                    currentTarget = target;
                    SetPrompt(true);
                }
                return;
            }
        }

        // 아무것도 못 맞추거나, 맞췄는데 interactable이 아니면
        if (currentTarget != null)
        {
            currentTarget = null;
            SetPrompt(false);
        }
    }

    private void SetPrompt(bool visible)
    {
        if (promptText == null) return;

        if (visible)
        {
            if (!promptText.gameObject.activeSelf)
                promptText.gameObject.SetActive(true);

            if (promptText.text != promptMessage)
                promptText.text = promptMessage;
        }
        else
        {
            if (promptText.gameObject.activeSelf)
                promptText.gameObject.SetActive(false);
        }
    }
}
