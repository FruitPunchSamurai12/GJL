using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterIndicator : MonoBehaviour
{
    [SerializeField]
    int _characterIndex = 0;
    Transform _trackedCharacter;
    Camera _cam;
    Image[] _images;
    RectTransform rect;

    private void OnEnable()
    {
        var player = FindObjectOfType<Player>();
        _trackedCharacter = player.GetSpecificCharacterTransform(_characterIndex);
        _cam = Camera.main;
        _images = GetComponentsInChildren<Image>();
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector3 screenPoint = _cam.WorldToViewportPoint(_trackedCharacter.position);
        bool onScreen = screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        if(onScreen)
        {
            DeactivateImages();
        }
        else
        {
            ActivateImages();
            Vector3 position = new Vector3(Mathf.Clamp(screenPoint.x,0.05f,0.95f), Mathf.Clamp(screenPoint.y,0.05f,0.95f),1);
            Vector3 screenPos = new Vector3(position.x * Screen.width, position.y * Screen.height, 1f);
            rect.anchoredPosition = new Vector2(screenPos.x, screenPos.y);
            RotateIndicator(screenPoint.x, screenPoint.y);
        }
    }

    void RotateIndicator(float x,float y)
    {
        float angle = 0;
        if (x > 0.95)
        {
            if (y > 0.95)
                angle = -45;
            else if (y < 0.05)
                angle = -135;
            else
                angle = -90;
        }
        else if (x < 0.05)
        {
            if (y > 0.95)
                angle = 45;
            else if (y < 0.05)
                angle = 135;
            else
                angle = 90;
        }
        else if (y < 0.05)
            angle = 180;
        rect.rotation = Quaternion.Euler(0, 0, angle);
    }

    void ActivateImages()
    {
        foreach (var image in _images)
        {
            image.enabled = true;
        }
    }

    void DeactivateImages()
    {
        foreach (var image in _images)
        {
            image.enabled = false;
        }
    }
}
