using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : AudioSpectrum
{

    [SerializeField] [Range(0,1000)] public float intensityMultiplier = 100f;
    [SerializeField] public float restSmoothTime = 2f;
    [SerializeField] [Range(0,500)] float gateThresh = 5f;
    private float val;

    private Light lightInstance;



    void Start()
    {
        m_audioSpectrum = new float[64];
        lightInstance = GetComponent<Light>();
    }

    void Update()
    {
        AudioListener.GetSpectrumData(m_audioSpectrum, 0, FFTWindow.Hamming);
        if (m_audioSpectrum !=null & m_audioSpectrum.Length > 0)
        {
            val = m_audioSpectrum[0];

            if (val >= (gateThresh/10000)) {
                {
                lightInstance.intensity = (val * intensityMultiplier) * 10000;

                lightInstance.intensity = Mathf.Lerp(lightInstance.intensity, lightInstance.intensity/2, restSmoothTime * Time.deltaTime);
                }
            }
            else
            {
                lightInstance.intensity = 0;
            }
            

        }
    }
}
