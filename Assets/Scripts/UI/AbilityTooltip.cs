using UnityEngine;
using TMPro;

public class AbilityTooltip : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _nameText;
    [SerializeField]
    TextMeshProUGUI _tooltipText;

    public void ActivateTooltip(string nameText,string tooltipText)
    {
        gameObject.SetActive(true);
        _nameText.SetText(nameText);
        _tooltipText.SetText(tooltipText);
    }

    public void DeactivateTooltip()
    {
        gameObject.SetActive(false);
    }

}
