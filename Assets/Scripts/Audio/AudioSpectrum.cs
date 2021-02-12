using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSpectrum : MonoBehaviour
{    
    // an array of floats to hold our audio spectrum
    public float[] m_audioSpectrum;

    // public variable that will hold our spectrum data

    public static float spectrumValue {get; private set;}
    
    
    // Start is called before the first frame update
    void Start()
    {
        // value should be a ^2 size (32, 64, 128, 256)
        m_audioSpectrum = new float[128];
    }

    // Update is called once per frame
    void Update()
    {
        // fills the spectrum array

        AudioListener.GetSpectrumData(m_audioSpectrum, 0, FFTWindow.Hamming);

        // check if our audioSpectrum has any data in it,
        // set the generalized value to first value in array
        // multiply by 100 (arbitrary value)

        if (m_audioSpectrum !=null & m_audioSpectrum.Length > 0)
        {
            float spectrumValue = m_audioSpectrum[0];
            //Debug.Log("Spec_" + spectrumValue);
            
            // Debug.Log("Light_" + lightIntensity);
            // GetComponent<Light>().intensity = lightIntensity * spectrumValue;
            // Debug.Log("Post_" + lightIntensity);
        }

        
    }


}
