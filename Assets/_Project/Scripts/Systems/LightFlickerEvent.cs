using System.Collections;
using UnityEngine;

public class LightFlickerEvent : MonoBehaviour
{
    [SerializeField] private Light targetLight;
    [SerializeField] private int flickerCount = 5;
    [SerializeField] private float interval = 0.12f;

    private Coroutine running;

    public void Play()
    {
        if (targetLight == null) return;

        if (running != null)
            StopCoroutine(running);

        running = StartCoroutine(FlickerRoutine());
    }

    private IEnumerator FlickerRoutine()
    {
        bool original = targetLight.enabled;

        for (int i = 0; i < flickerCount; i++)
        {
            targetLight.enabled = !targetLight.enabled;
            yield return new WaitForSeconds(interval);
        }

        targetLight.enabled = original;
        running = null;
    }
}
