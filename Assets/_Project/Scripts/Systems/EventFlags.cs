using UnityEngine;

public class EventFlags : MonoBehaviour
{
    public static EventFlags Instance { get; private set; }

    [Header("Flags")]
    public bool firstPowerOnTriggered;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // 씬 하나만 쓸 거면 굳이 DontDestroyOnLoad 필요 없음
        // DontDestroyOnLoad(gameObject);
    }
}
