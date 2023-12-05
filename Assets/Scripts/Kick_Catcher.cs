using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kick_Catcher : MonoBehaviour
{
    public float mulFactor = 8f;
    public float lerpFactorAttack = 20f;
    public float lerpFactorRelease = 8f;

    public static float currentAmplitude = 0f;

    public int spectrumPos = 4;

    public AudioSource audioSource;

    public float amp;

    void Start()
    {
        //Cursor.visible = false;
        //audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

        // Spectrum stuff --------------------------------------------------------------------------------------------------------------------------------------------------------
        float[] spectrum = AudioListener.GetSpectrumData(2048, 0, FFTWindow.Hamming);
        //AudioListener.GetOutputData(spectrum, 0);

        float rawValue = spectrum[spectrumPos];


        if (currentAmplitude < rawValue * mulFactor)
        {
            currentAmplitude = Mathf.Lerp(currentAmplitude, rawValue * mulFactor, Time.deltaTime * lerpFactorAttack);
        }
        else
        {
            currentAmplitude = Mathf.Lerp(currentAmplitude, rawValue * mulFactor, Time.deltaTime * lerpFactorRelease);
        }

        if (currentAmplitude > 1.0f) currentAmplitude = 1.0f;

        amp = currentAmplitude;
        //Debug.Log(currentAmplitude);
        // -------------- --------------------------------------------------------------------------------------------------------------------------------------------------------

    }
}
