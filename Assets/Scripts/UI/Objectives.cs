using UnityEngine;
using UnityEngine.UI;

public class Objectives : MonoBehaviour
{
    [SerializeField]
    GameObject _objectivesPanel;
    [SerializeField]
    Image[] _checkBoxes;
    [SerializeField]
    Sprite _checkedSprite;
    int _currentObjectiveIndex = 0;

    private void OnEnable()
    {
        GameEvents.Instance.OnObjectiveClear += ObjectiveCleared;
    }

    private void OnDisable()
    {
        GameEvents.Instance.OnObjectiveClear -= ObjectiveCleared;
    }

    public void OnObjectiveBoxClick()
    {
        _objectivesPanel.SetActive(!_objectivesPanel.activeInHierarchy);
    }


    void ObjectiveCleared()
    {
        if (_currentObjectiveIndex >= _checkBoxes.Length)
            return;
        _checkBoxes[_currentObjectiveIndex].sprite = _checkedSprite;
        _currentObjectiveIndex++;
        AkSoundEngine.PostEvent("Play_UI_Objective_Clear", gameObject);
    }

}
