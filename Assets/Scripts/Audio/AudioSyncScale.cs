using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// derive the AudioSyncer class
public class AudioSyncIntensity : AudioSyncer
{
    // 2 member variables - one for on beat, one for rest
    // when a beat occurs, we move to beatintensity
    // when a rest occurs, we move to restintensity
    [SerializeField] float intensityModifier = 5.0f;
    public Vector3 beatScale;
    public Vector3 restScale;
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //override method to override all parent functions
    // intensity currently maniuplated by beat
    // if not in beat state, lerp intensity to rest intensity
    public override void OnUpdate()
    {
        base.OnUpdate();

        if (m_isBeat) {return;}
        
        Vector3.Lerp(transform.localScale, restScale, restSmoothTime * Time.deltaTime);
    }


    // spawn a coroutine to go to the beatscale
    public override void OnBeat()
    {
        base.OnBeat();

        StopCoroutine("MoveToScale");
        StartCoroutine("MoveToScale", beatScale);
    }

    private IEnumerator MoveToScale(Vector3 _target)
    {
        // mark current scale and initial scale
        // create a timer to measure time between initial and
        // target scale

        Vector3 _curr = transform.localScale;
        Vector3 _initial = _curr;
        float _timer = 0;

        // while current scale is != target scale
        // we lerp between the initial and target
        // scale and increment the transform scale to
        // the current scale variable

        while(_curr != _target)
        {
            _curr = Vector3.Lerp(_initial, _target, _timer / timeToBeat);
            _timer += Time.deltaTime;

            transform.localScale = _curr;

            yield return null;
        }

        // when timer == timeToBeat, 
        // we know current scale == beatscale

        // notify base class to continue lerping to the rest scale

        m_isBeat =false;
    }

}
