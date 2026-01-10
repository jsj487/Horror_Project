using UnityEngine;

public class LightSwitchInteractable : MonoBehaviour, IInteractable
{
    [Header("Target")]
    [SerializeField] private Light targetLight;

    [Header("Audio (Optional)")]
    [SerializeField] private AudioSource sfxSource;

    public string GetPrompt()
    {
        if (targetLight == null) return "E : Interact";
        // enabled=true면 끄는 행동이 다음 행동
        return targetLight.enabled ? "E : Lights Off" : "E : Lights On";
    }

    public void Interact()
    {
        if (targetLight != null)
            targetLight.enabled = !targetLight.enabled;

        if (sfxSource != null)
            sfxSource.Play();
    }
}
