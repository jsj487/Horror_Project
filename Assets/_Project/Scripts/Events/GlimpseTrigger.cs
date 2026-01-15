using UnityEngine;

public class GlimpseTrigger : MonoBehaviour
{
    [SerializeField] private GlimpseEvent glimpseEvent;
    [SerializeField] private string flagKey = "glimpse_01"; // EventFlags에 bool로 추가해도 되고, 문자열 키로 관리해도 됨(너 구조에 맞춰)
    [SerializeField] private bool triggerOnce = true;

    private bool triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (triggerOnce && triggered) return;
        triggered = true;

        if (glimpseEvent != null)
            glimpseEvent.Play();
    }
}
