using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerParticles : MonoBehaviour
{
	public int bufferIndex;
	public Color startColor;
	public AudioPeer audioPeer;
	//public AnimationCurve curve;
	private ParticleSystem particleSystem;

	private float h, s, v;
	private Color hsvColor;

	// Use this for initialization
	void Start()
	{
		particleSystem = GetComponent<ParticleSystem>();
		particleSystem.startColor = startColor;

		Color.RGBToHSV(startColor, out h, out s, out v);
		hsvColor = Color.HSVToRGB(h, s, v);
	}

	// Update is called once per frame
	void Update()
	{
		//float amplitude = 1f - Mathf.Cos(audioPeer._audioBand[bufferIndex] * Mathf.PI * 0.5f);
		float amplitude = audioPeer._audioBand[bufferIndex];
		float cos = 1f - Mathf.Cos(amplitude * Mathf.PI * 0.5f);
		/*if (amplitude < .2f)
		{
			particleSystem.emissionRate = 0;
			return;
		}*/

		particleSystem.emissionRate = amplitude * 1500;
		particleSystem.startSize = cos * .225f;
		particleSystem.startSpeed = cos * 20;
		//particleSystem.startColor = Color.HSVToRGB(h, s, cos);

		//particleSystem.startColor = Color.HSVToRGB(h, Mathf.Lerp(1.0f, 0.4f, audioPeer._audioBandBuffer[bufferIndex]), Mathf.Lerp(0.5f, 1.0f, audioPeer._audioBandBuffer[bufferIndex]));
	}
}