using UnityEngine;

public enum ObjectiveStep
{
    EnterCorridor = 0,
    FindCodeAndUseKeypad = 1,
    RestorePower = 2,
    Done = 99
}

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField] private ObjectivePresenter presenter;

    public ObjectiveStep Current { get; private set; } = ObjectiveStep.EnterCorridor;

    private void Start()
    {
        Apply(Current);
    }

    public void SetStep(ObjectiveStep step)
    {
        Current = step;
        Apply(step);
    }

    private void Apply(ObjectiveStep step)
    {
        if (presenter == null) return;

        switch (step)
        {
            case ObjectiveStep.EnterCorridor:
                presenter.SetText("Objective: Enter the corridor");
                break;
            case ObjectiveStep.FindCodeAndUseKeypad:
                presenter.SetText("Objective: Find the code and enter it on the keypad");
                break;
            case ObjectiveStep.RestorePower:
                // 진행도(0/3)는 PowerRestoreManager가 덮어쓰기 전에 기본 문구만 세팅
                presenter.SetText("Objective: Restore power (0/3)");
                break;
            case ObjectiveStep.Done:
                presenter.SetText("Objective: Proceed");
                break;
        }
    }
}
