using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAudioSync : AudioSyncer
{
    [SerializeField] float intensityMultiplier = 5.0f;
    public Color[] lightColorArray;
    public Color lightColor;
    public Color restColor;
    private float lightIntensity;

    private Light currentLight;
    



    void Start()
    {
  
        // May not need this - lightColor = GetComponent<Light>().color;
    }

    void Update()
    {
        
    }

        public override void OnUpdate()
	{
		base.OnUpdate();

        Debug.Log(m_audioValue + " " + m_previousAudioValue + "?");

		if (m_isBeat) {return;}

		gameObject.GetComponent<Light>().color = Color.Lerp(currentLight.color, restColor, restSmoothTime * Time.deltaTime);

        lightIntensity = (this.gameObject.GetComponent<Light>().intensity * intensityMultiplier);

        Debug.Log(lightIntensity + "!");

	}

        public override void OnBeat()
	{
		base.OnBeat();
	}
}
