using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class AudioSignal : MonoBehaviour
{
    public AudioSource audio_Source;
    public static float[] samples = new float[512];
    // Start is called before the first frame update
    void Start()
    {
        audio_Source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
    }

    void GetSpectrumAudioSource()
    {
        audio_Source.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }
}
