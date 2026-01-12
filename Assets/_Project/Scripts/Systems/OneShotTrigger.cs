using UnityEngine;

public class OneShotTrigger : MonoBehaviour
{
    [Header("Trigger")]
    [SerializeField] private string playerTag = "Player"; // Player에 Tag 달아뒀으면 사용
    [SerializeField] private bool useLayerCheck = true;
    [SerializeField] private LayerMask playerLayer;

    [Header("Event")]
    [SerializeField] private Light[] toggleLights;
    [SerializeField] private bool setLightsEnabled = false;
    [SerializeField] private bool targetEnabledState = false;

    [SerializeField] private AudioSource stinger; // 없어도 됨
    [SerializeField] private float autoDisableAfterSeconds = 0f;

    private bool triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        // Tag 방식(간단)
        if (!string.IsNullOrEmpty(playerTag) && other.CompareTag(playerTag))
        {
            Fire();
            return;
        }

        // Layer 방식(태그 안 쓰는 경우)
        if (useLayerCheck)
        {
            int otherLayerMask = 1 << other.gameObject.layer;
            if ((playerLayer.value & otherLayerMask) != 0)
            {
                Fire();
            }
        }
    }

    private void Fire()
    {
        triggered = true;

        if (toggleLights != null)
        {
            foreach (var l in toggleLights)
            {
                if (l == null) continue;
                if (setLightsEnabled) l.enabled = targetEnabledState;
                else l.enabled = !l.enabled;
            }
        }

        if (stinger != null) stinger.Play();

        if (autoDisableAfterSeconds > 0f)
            Invoke(nameof(DisableSelf), autoDisableAfterSeconds);

        Debug.Log($"[Event] OneShotTrigger fired: {name}");
    }

    private void DisableSelf()
    {
        gameObject.SetActive(false);
    }
}
