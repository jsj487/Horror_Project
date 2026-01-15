using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilhouetteController : MonoBehaviour
{
    [Header("Refs (Optional)")]
    [SerializeField] private Transform moveRoot; // 비워도 됨. 비면 transform 사용

    [Header("Renderers")]
    [SerializeField] private List<Renderer> renderers = new List<Renderer>();

    private Coroutine co;

    private void Awake()
    {
        AutoCollectIfEmpty();
        SetVisible(false); // 시작은 숨김
    }

    private void OnValidate()
    {
        AutoCollectIfEmpty();
    }

    private void AutoCollectIfEmpty()
    {
        if (renderers == null) renderers = new List<Renderer>();
        if (renderers.Count > 0) return;

        // 자식까지 Renderer 전부 자동 수집
        var found = GetComponentsInChildren<Renderer>(true);
        foreach (var r in found)
        {
            // 필요하면 여기서 특정 타입만 허용도 가능
            renderers.Add(r);
        }
    }

    public void SetVisible(bool visible)
    {
        AutoCollectIfEmpty();

        foreach (var r in renderers)
        {
            if (r == null) continue;
            r.enabled = visible;
        }

        Debug.Log($"[Silhouette] visible={visible}, rendererCount={renderers.Count}");

    }

    public void ShowAt(Transform point, float duration)
    {
        if (point != null)
        {
            var root = moveRoot != null ? moveRoot : transform;
            root.SetPositionAndRotation(point.position, point.rotation);
        }

        SetVisible(true);

        if (co != null) StopCoroutine(co);
        co = StartCoroutine(CoHide(duration));
    }

    private IEnumerator CoHide(float duration)
    {
        yield return new WaitForSeconds(duration);
        SetVisible(false);
        co = null;
    }

}
