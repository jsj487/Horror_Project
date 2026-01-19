using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSnapshotDriver : MonoBehaviour
{
    [Header("Mixer")]
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioMixerSnapshot normal;
    [SerializeField] private AudioMixerSnapshot focus;
    [SerializeField] private AudioMixerSnapshot panic;

    [Header("Default Transition")]
    [SerializeField] private float defaultTransition = 0.25f;

    private Coroutine co;

    public void ToNormal(float t = -1f) => Transition(normal, t);
    public void ToFocus(float t = -1f) => Transition(focus, t);
    public void ToPanic(float t = -1f) => Transition(panic, t);

    public void PanicThenNormal(float panicHoldSeconds = 0.35f, float inTime = 0.05f, float outTime = 0.25f)
    {
        if (co != null) StopCoroutine(co);
        co = StartCoroutine(CoPanicThenNormal(panicHoldSeconds, inTime, outTime));
    }

    private void Transition(AudioMixerSnapshot snap, float t)
    {
        if (snap == null) return;
        snap.TransitionTo(t >= 0f ? t : defaultTransition);
    }

    private IEnumerator CoPanicThenNormal(float hold, float inTime, float outTime)
    {
        ToPanic(inTime);
        yield return new WaitForSeconds(hold);
        ToNormal(outTime);
        co = null;
    }
}
