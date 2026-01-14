using TMPro;
using UnityEngine;

public class ObjectiveUI : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private bool powerObjectiveDone;

    private void Start()
    {
        SetText("Objective: Enter the corridor");
    }

    private void Update()
    {
        if (EventFlags.Instance == null) return;

        if (!powerObjectiveDone && EventFlags.Instance.firstPowerOnTriggered)
        {
            powerObjectiveDone = true;
            SetText("Objective: Open the door and proceed");
        }
    }

    private void SetText(string msg)
    {
        if (text == null) return;
        text.text = msg;
    }
}
