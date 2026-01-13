using UnityEngine;
using TMPro;

public class ObjectivePresenter : MonoBehaviour
{
    [SerializeField] private TMP_Text objectiveText;

    public void SetText(string text)
    {
        if (objectiveText == null) return;
        objectiveText.text = text ?? "";
    }
}
