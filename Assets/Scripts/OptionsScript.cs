using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsScript : MonoBehaviour {

    public AudioMixer audioMixer;
    public Image scrollbar;
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }
	
    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

}
