using UnityEngine;

public class PowerRestoreManager : MonoBehaviour
{
    [Header("Progress")]
    [SerializeField] private int requiredOnCount = 3;

    [Header("UI")]
    [SerializeField] private ObjectivePresenter objective;     // 진행도 표시용
    [SerializeField] private ObjectiveManager objectiveManager; // 단계 변경용(완료 후 Done)
    [Header("On Completed (Optional)")]
    [SerializeField] private DoorInteractable[] doorsToUnlock;
    [SerializeField] private MonoBehaviour[] enableWhenRestored; // KeypadInteractable 등 활성화용

    private int currentOnCount;
    private bool completed;

    private void Start()
    {
        currentOnCount = 0;
        completed = false;
        RefreshObjective();
    }

    public void NotifySwitchChanged(bool isOn)
    {
        if (completed) return;

        currentOnCount += isOn ? 1 : -1;
        if (currentOnCount < 0) currentOnCount = 0;
        if (currentOnCount > requiredOnCount) currentOnCount = requiredOnCount;

        RefreshObjective();

        if (currentOnCount >= requiredOnCount)
            Complete();
    }

    private void RefreshObjective()
    {
        if (objective == null) return;
        objective.SetText($"Objective: Restore power ({currentOnCount}/{requiredOnCount})");
    }


    private void Complete()
    {
        completed = true;

        // 전역 플래그 쓰고 있으면 같이 세팅
        if (EventFlags.Instance != null)
            EventFlags.Instance.powerRestored = true; // EventFlags에 bool powerRestored 없으면 추가 필요

        if (doorsToUnlock != null)
        {
            foreach (var d in doorsToUnlock)
                if (d != null) d.Unlock();
        }

        if (enableWhenRestored != null)
        {
            foreach (var m in enableWhenRestored)
                if (m != null) m.enabled = true;
        }

        if (objectiveManager != null)
            objectiveManager.SetStep(ObjectiveStep.Done);

        Debug.Log("[Power] Restored");
    }
}
