using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncer : MonoBehaviour
{
    // determines what spectrum value will trigger a beat
    public float bias;
    // minimum interval between each beet
    public float timeStep;
    // how much time before viz completes
    public float timeToBeat;
    // how fast the object goes to rest after beat
    public float restSmoothTime;

    // determine if the value went above or below the bias
    // during current frame
    public float m_previousAudioValue;
    public float m_audioValue;
    // timestep interval
    private float m_timer;

    // is sync currently in a beat state?
    protected bool m_isBeat;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // may want subclasses to inject own update code
        OnUpdate();
    }


    // THE BELOW METHODS ARE VIRTUAL, MEANING
    // SUBCLASSES MAY NEED TO DO SOMETHING WITH THESE METHODS
    // AS WELL
    public virtual void OnUpdate()
    {
        m_previousAudioValue = m_audioValue;
        m_audioValue = AudioSpectrum.spectrumValue;

        // if audio value went below bias on current frame
        // we check if we're allowed to trigger beat 
        // based on our timestep rule, then we trigger on beat

        if (m_previousAudioValue > bias &&
            m_audioValue <= bias)
        {
            if(m_timer > timeStep)
                OnBeat();
        }
        
        // do the same thing but check if the audio value went
        // above the bias on the current frame

        if (m_previousAudioValue <= bias &&
            m_audioValue > bias)
        {
            if(m_timer > timeStep)
                OnBeat();
        }

        // increment the timeStep timer

        m_timer += Time.deltaTime;
    }

    // method that notifies us when beat occurs and
    // resets timer to 0 once occurs
    // change the beat bool true
    public virtual void OnBeat()
    
    {
        Debug.Log("Beat!");
        m_timer = 0;
        m_isBeat = true;
    }
}
