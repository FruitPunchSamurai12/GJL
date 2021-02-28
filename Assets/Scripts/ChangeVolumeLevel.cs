using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChangeVolumeLevel : MonoBehaviour
{
    public string whatValue;
    public Slider thisSlider;
    public float SoundVolume;
    public float MusicVolume;
    // Start is called before the first frame update
    void Start()
    {
        thisSlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSpecificVolume()
    {
        float sliderValue = thisSlider.value;
        
        if (whatValue == "Music_Volume")
        {
            MusicVolume = thisSlider.value;
            AkSoundEngine.SetRTPCValue("Master_Music_Bus", MusicVolume);
        }

        if (whatValue == "Sound_Volume")
        {
            SoundVolume = thisSlider.value;
            AkSoundEngine.SetRTPCValue("Master_Audio_Bus", SoundVolume);
        }
    }
}
