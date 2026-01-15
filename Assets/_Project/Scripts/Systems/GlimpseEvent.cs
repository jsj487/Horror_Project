using UnityEngine;

public class GlimpseEvent : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private SilhouetteController silhouette;
    [SerializeField] private Transform silhouettePoint;
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
        Debug.Log("[GlimpseEvent] Play called");
        Debug.Log($"[GlimpseEvent] Play called on {name}");
        Debug.Log($"[GlimpseEvent] silhouette={(silhouette ? silhouette.name : "NULL")}, point={(silhouettePoint ? silhouettePoint.name : "NULL")}");

        if (oneShot && played) return;
        played = true;

        if (silhouette != null)
            silhouette.ShowAt(silhouettePoint, showDuration);

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
    }

}
