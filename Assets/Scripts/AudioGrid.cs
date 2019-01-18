//Wil je meer leren over audio visualisatie, algoritmes & shaders?
//bekijk dan eens de gratis video lessen op youtube.com/peerplay

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGrid : MonoBehaviour {
    //audio
    [Header("Audio")]
    public AudioPeer _audioPeer;

    //grid
    [Header("Setup")]
    public GameObject _prefab;
    public Vector2Int _gridSize;
    public float _spaceInBetween;
    private Transform[,] _prefabTransform;
    //noise
    [Header("Noise")]
    public float _noiseFrequency;
    private Vector2 _noiseOffset;
    public Vector2 _noiseSpeed;
    public Vector2 _scaleMinMax;

    //colors
    [Header("Colors")]
    public Gradient _gradient;
    private Color[] _color;
    public Material _baseMaterial;
    private Material[] _material;
    public string _materialProperty;
    public float _colorMultiplier;


	// Use this for initialization
	void Start () {
        _color = new Color[64];  //zet lengte array
        _material = new Material[64]; // zet lengte array
        for (int i = 0; i < 64; i++)
        {
            _color[i] = _gradient.Evaluate(i * (1f / 64)); //handige manier om van een gradient veel kleuren te maken
            _material[i] = new Material(_baseMaterial); //maak 64 materials aan voor iedere kleur of audioband
            _material[i].EnableKeyword("_EMISSION"); //zet emission optie aan in de standard surface shader
        }

        _prefabTransform = new Transform[_gridSize.x, _gridSize.y]; //zet lengte array
        for (int x = 0; x < _gridSize.x; x++)
        {
            for (int z = 0; z < _gridSize.y; z++)
            {
                GameObject prefabInstance = (GameObject)Instantiate(_prefab); //instantiate game object
                prefabInstance.transform.parent = this.transform; // maak het nieuwe object een child van het object waar dit script op zit
                prefabInstance.transform.localPosition = new Vector3(x * _spaceInBetween, 0, z * _spaceInBetween); //zet positie in de grid
                prefabInstance.transform.GetChild(0).GetComponent<MeshRenderer>().material = _material[Random.Range(0,64)]; //verdeel een random kleur 
                //gebruik onderstaande regel om rijen van dezelfde kleur/band te maken
                //probeer zelf om een interessante verdeling van kleuren te maken
                //prefabInstance.transform.GetChild(0).GetComponent<MeshRenderer>().material = _material[x]; 
                _prefabTransform[x, z] = prefabInstance.transform;
            }
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        //de noise wordt verschoven in 2 assen gebaseerd op binnenkomende frequentie en noiseSpeed die je zelf aangeeft
        _noiseOffset += new Vector2(_noiseSpeed.x * Time.deltaTime * _audioPeer._audioBand[1], _noiseSpeed.y * Time.deltaTime * _audioPeer._audioBand[4]);

        for (int x = 0; x < _gridSize.x; x++)
        {
            for (int z = 0; z < _gridSize.y; z++)
            {
                //bereken de noise met PerlinNoise functie met een bepaalde frequentie en offset
                float noise = (Mathf.PerlinNoise((x + _noiseOffset.x) * _noiseFrequency, (z + _noiseOffset.y) * _noiseFrequency));
                //objecten scalen in hun Y-as gebaseerd op de noise
                _prefabTransform[x, z].localScale = new Vector3(_prefabTransform[x, z].localScale.x,(Mathf.Lerp(_scaleMinMax.x, _scaleMinMax.y,Mathf.Clamp01( noise)) * _audioPeer._AmplitudeBuffer), _prefabTransform[x, z].localScale.z);
                //pas de kleur aan in de materials, die de objecten in het grid gebruiken
                _material[x].SetColor(_materialProperty, _color[x] * _colorMultiplier * _audioPeer._audioBand64[x] * noise);
            }
        }
    }
}
