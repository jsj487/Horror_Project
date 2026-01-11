using UnityEngine;

public class LightSwitchInteractable : MonoBehaviour, IInteractable
{
    [Header("Target")]
    [SerializeField] private Light targetLight;

    [Header("Audio (Optional)")]
    [SerializeField] private AudioSource sfxSource;

    [Header("One-shot Event (Optional)")]
    [SerializeField] private LightFlickerEvent firstPowerOnEvent;

    [Header("Unlock Targets (Optional)")]
    [SerializeField] private DoorInteractable[] unlockDoorsOnFirstPowerOn;


    public string GetPrompt()
    {
        if (targetLight == null) return "E : Interact";
        // enabled=true면 끄는 행동이 다음 행동
        return targetLight.enabled ? "E : Lights Off" : "E : Lights On";
    }

    public void Interact()
    {
        if (targetLight != null)
        {
            bool wasOn = targetLight.enabled;
            targetLight.enabled = !targetLight.enabled;

            // OFF -> ON 되는 순간, 최초 1회 이벤트
            if (!wasOn && targetLight.enabled)
            {
                TryTriggerFirstPowerOn();
            }
        }

        if (sfxSource != null)
            sfxSource.Play();
    }
    private void TryTriggerFirstPowerOn()
    {
        if (EventFlags.Instance == null) return;
        if (EventFlags.Instance.firstPowerOnTriggered) return;

        EventFlags.Instance.firstPowerOnTriggered = true;

        if (unlockDoorsOnFirstPowerOn != null)
        {
            foreach (var d in unlockDoorsOnFirstPowerOn)
            {
                if (d != null) d.Unlock();
            }
        }

        if (firstPowerOnEvent != null)
            firstPowerOnEvent.Play();

        Debug.Log("[Event] First power on triggered");



    }

}
