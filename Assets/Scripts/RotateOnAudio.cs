using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOnAudio : MonoBehaviour {
    public AudioPeer _audioPeer;
    public Vector3 _rotateSpeed;
    public bool _useBuffer;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (_useBuffer)
        {
            this.transform.Rotate(_rotateSpeed.x * Time.deltaTime * _audioPeer._AmplitudeBuffer, _rotateSpeed.y * Time.deltaTime * _audioPeer._AmplitudeBuffer, _rotateSpeed.z * Time.deltaTime * _audioPeer._AmplitudeBuffer);
        }  
        else
        {
            this.transform.Rotate(_rotateSpeed.x * Time.deltaTime * _audioPeer._Amplitude, _rotateSpeed.y * Time.deltaTime * _audioPeer._Amplitude, _rotateSpeed.z * Time.deltaTime * _audioPeer._Amplitude);
        }


	}
}
