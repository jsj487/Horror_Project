using System.Collections;
using UnityEngine;

public class AmbientMixerController : MonoBehaviour
{
    [SerializeField] private AudioSource noPower;
    [SerializeField] private AudioSource powerOn;
    [SerializeField] private float fadeSeconds = 1.5f;

    private Coroutine co;

    private void Awake()
    {
        // 시작 상태: 전력 전
        if (noPower != null)
        {
            noPower.volume = 1f;
            if (!noPower.isPlaying) noPower.Play();
        }
        if (powerOn != null)
        {
            powerOn.volume = 0f;
            if (!powerOn.isPlaying) powerOn.Play();
        }
    }

    public void SetPowerRestored(bool restored)
    {
        if (co != null) StopCoroutine(co);
        co = StartCoroutine(Fade(restored));
    }

    private IEnumerator Fade(bool restored)
    {
        float t = 0f;
        float a0 = noPower != null ? noPower.volume : 0f;
        float b0 = powerOn != null ? powerOn.volume : 0f;

        float a1 = restored ? 0f : 1f;
        float b1 = restored ? 1f : 0f;

        while (t < fadeSeconds)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / fadeSeconds);

            if (noPower != null) noPower.volume = Mathf.Lerp(a0, a1, k);
            if (powerOn != null) powerOn.volume = Mathf.Lerp(b0, b1, k);

            yield return null;
        }

        if (noPower != null) noPower.volume = a1;
        if (powerOn != null) powerOn.volume = b1;
    }
}
