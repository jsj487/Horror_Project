using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float range = 3f;
    [SerializeField] private LayerMask interactMask; // Interactable 레이어만

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, range, interactMask, QueryTriggerInteraction.Ignore))
        {
            // 맞은 오브젝트(자식) -> 부모까지 올라가서 인터페이스 찾기
            var target = hit.collider.GetComponentInParent<IInteractable>();
            if (target != null)
                target.Interact();
        }
    }
}
