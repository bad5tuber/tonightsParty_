using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncColor : AudioSyncer
{
    public Color[] beatColor;
	public Color restColor;

	private int m_randomIndex;
	private Light m_light;

	private void Start()
	{
		m_light = GetComponent<Light>();
	}

    public override void OnUpdate()
	{
		base.OnUpdate();

		if (m_isBeat) return;

		m_light.color = Color.Lerp(m_light.color, restColor, restSmoothTime * Time.deltaTime);
	}
    public override void OnBeat()
	{
		base.OnBeat();

		Color _c = RandomColor();

		StopCoroutine("MoveToColor");
		StartCoroutine("MoveToColor", _c);
	}

    private Color RandomColor()
	{
		if (beatColor == null || beatColor.Length == 0) return Color.white;
		m_randomIndex = Random.Range(0, beatColor.Length);
		return beatColor[m_randomIndex];
	}

    	private IEnumerator MoveToColor(Color _target)
	{
		Color _curr = m_light.color;
		Color _initial = _curr;
		float _timer = 0;
		
		while (_curr != _target)
		{
			_curr = Color.Lerp(_initial, _target, _timer / timeToBeat);
			_timer += Time.deltaTime;

			m_light.color = _curr;

			yield return null;
		}
		
		m_isBeat = false;
	}
}
