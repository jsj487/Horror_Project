using UnityEngine;

public class LightSwitchInteractable : MonoBehaviour, IInteractable
{
    [Header("Target")]
    [SerializeField] private Light targetLight;

    [Header("Audio (Optional)")]
    [SerializeField] private AudioSource sfxSource;

    public void Interact()
    {
        if (targetLight != null)
            targetLight.enabled = !targetLight.enabled;

        if (sfxSource != null)
            sfxSource.Play();
    }
}
