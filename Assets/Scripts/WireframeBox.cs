using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireframeBox : MonoBehaviour
{
	public AudioPeer audioPeer;
	public float widthScale;

	[Range(0f, 1f)]
	public float buffer5VolumeThreshold;

	[Range(0f, 1f)]
	public float buffer6VolumeThreshold;

	[Range(0f, 1f)]
	public float buffer7VolumeThreshold;

	[Range(0f, 1f)]
	public float buffer8VolumeThreshold;

	private LineRenderer lineRenderer;

	// Use this for initialization
	void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}

	// Update is called once per frame
	void Update()
	{
		bool showWireframe = true;
		float average = 0f;
		for (int i = 4; i < 8; i++)
		{
			showWireframe = showWireframe & (audioPeer._audioBandBuffer[i] >= buffer5VolumeThreshold);
			average += audioPeer._audioBandBuffer[i];
		}

		average /= 4f;

		lineRenderer.widthMultiplier = showWireframe ? average * widthScale : 0f;
	}
}