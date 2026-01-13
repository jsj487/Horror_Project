using UnityEngine;

public class PowerSwitchInteractable : MonoBehaviour, IInteractable
{
    [Header("Refs")]
    [SerializeField] private PowerRestoreManager manager;

    [Header("State")]
    [SerializeField] private bool startOn = false;
    [SerializeField] private bool oneWayOn = true; // 한번 켜면 다시 끄지 못하게(추천)

    [Header("Visual (Optional)")]
    [SerializeField] private GameObject onIndicator; // 작은 큐브/라이트 등

    [Header("Audio (Optional)")]
    [SerializeField] private AudioSource sfxToggle;

    private bool isOn;

    private void Start()
    {
        isOn = startOn;
        ApplyVisual();
        // 시작부터 ON이면 매니저에 반영
        if (isOn && manager != null)
            manager.NotifySwitchChanged(true);
    }

    public string GetPrompt()
    {
        if (isOn) return "E : Power ON";
        return "E : Flip switch";
    }

    public void Interact()
    {
        if (isOn && oneWayOn) return;

        isOn = !isOn;
        ApplyVisual();

        if (sfxToggle != null) sfxToggle.Play();
        if (manager != null) manager.NotifySwitchChanged(isOn);
    }

    private void ApplyVisual()
    {
        if (onIndicator != null)
            onIndicator.SetActive(isOn);
    }
}
