using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    Animator _animator;
    [SerializeField]
    SpinnySpin _spinnySpin;
    [SerializeField]
    GameObject _menuPanel;
    [SerializeField]
    GameObject _startText;

    bool _click = false;

    private void Awake()
    {
        _click = false;
        _menuPanel.SetActive(false);
        _startText.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(!_click && Controller.Instance.LeftClick)
        {
            _click = true;
            _animator.SetTrigger("Click");
            _spinnySpin.BeginSpinning();
            _menuPanel.SetActive(true);
            _startText.SetActive(false);
        }
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}
