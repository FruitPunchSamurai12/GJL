using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cuscene : MonoBehaviour
{
    public static bool completed = false;

    [SerializeField]
    Sprite[] images;
    int index = 0;

    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }


    private void Update()
    {
        if(Controller.Instance.LeftClick)
        {
            index++;
            if(index>=images.Length)
            {
                completed = true;
            }
            else
            {
                image.sprite = images[index];
            }
        }
    }
}
