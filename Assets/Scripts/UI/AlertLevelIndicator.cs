using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertLevelIndicator : MonoBehaviour
{
    [SerializeField]
    Sprite _susIndicator;
    [SerializeField]
    Sprite _alertIndicator;

    int _numberOfSusEnemies = 0;
    int _numberOfAlertEnemies = 0;
    Image _image;
    

    private void OnEnable()
    {
        _image = GetComponentInChildren<Image>();
        GameEvents.Instance.OnEnemyEnterSuspicious += EnemyEnterSuspicious;
        GameEvents.Instance.OnEnemyExitSuspicious += EnemyExitSuspicious;
        GameEvents.Instance.OnEnemyEnterAlert += EnemyEnterAlert;
        GameEvents.Instance.OnEnemyExitAlert += EnemyExitAlert;
        ChangeImageSprite();
    }

    private void OnDisable()
    {
        GameEvents.Instance.OnEnemyEnterSuspicious -= EnemyEnterSuspicious;
        GameEvents.Instance.OnEnemyExitSuspicious -= EnemyExitSuspicious;
        GameEvents.Instance.OnEnemyEnterAlert -= EnemyEnterAlert;
        GameEvents.Instance.OnEnemyExitAlert -= EnemyExitAlert;
    }

    private void EnemyEnterSuspicious()
    {
        _numberOfSusEnemies++;
        ChangeImageSprite();
    }

    private void EnemyExitSuspicious()
    {
        _numberOfSusEnemies--;
        if (_numberOfSusEnemies < 0)
            _numberOfSusEnemies = 0;
        ChangeImageSprite();
    }

    private void EnemyEnterAlert()
    {
        _numberOfAlertEnemies++;
        ChangeImageSprite();    
    }

    private void EnemyExitAlert()
    {
        _numberOfAlertEnemies--;
        if (_numberOfAlertEnemies < 0)
            _numberOfAlertEnemies = 0;
        ChangeImageSprite();
    }


    private void ChangeImageSprite()
    {
        if(_numberOfAlertEnemies>0)
        {
            _image.enabled = true;
            _image.sprite = _alertIndicator;
            //AkSoundEngine.SetState("Game_States", "Alert");
            
        }
        else if(_numberOfSusEnemies>0)
        {
            _image.enabled = true;
            _image.sprite = _susIndicator;
            //AkSoundEngine.SetState("Game_States", "Suspicious");
        }
        else
        {
            _image.enabled = false;
            //AkSoundEngine.SetState("Game_States", "Idle");
        }
    }
}
