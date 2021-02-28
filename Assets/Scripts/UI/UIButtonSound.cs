using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonSound : MonoBehaviour
{
    // Start is called before the first frame update
    public void onHover()
    {
        AkSoundEngine.PostEvent("Play_UI_Hover", gameObject);
    }
    public void onClick()
    {
        AkSoundEngine.PostEvent("Play_UI_Click_Regular", gameObject);
    }
    public void onPlayButton()
    {
        AkSoundEngine.PostEvent("Play_UI_Main_Start_Button", gameObject);
    }

}
