using System.Collections;
using UnityEngine;

public class GlimpseEvent : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameObject silhouette;
    [SerializeField] private AudioSource stinger;
    [SerializeField] private Light[] toggleLights;

    [Header("Settings")]
    [SerializeField] private float showDuration = 0.8f;
    [SerializeField] private bool setLightsEnabled = true;
    [SerializeField] private bool targetLightState = false; // false면 끔
    [SerializeField] private bool oneShot = true;

    private bool played;

    public void Play()
    {
        if (oneShot && played) return;
        played = true;

        if (silhouette != null) silhouette.SetActive(true);
        if (stinger != null) stinger.Play();

        if (toggleLights != null && toggleLights.Length > 0)
        {
            foreach (var l in toggleLights)
            {
                if (l == null) continue;
                if (setLightsEnabled) l.enabled = targetLightState;
                else l.enabled = !l.enabled;
            }
        }

        StopAllCoroutines();
        StartCoroutine(HideAfter());
    }

    private IEnumerator HideAfter()
    {
        yield return new WaitForSeconds(showDuration);
        if (silhouette != null) silhouette.SetActive(false);
    }
}
