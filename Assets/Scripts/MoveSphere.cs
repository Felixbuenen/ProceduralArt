using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSphere : MonoBehaviour
{
  public float trailScale = .5f;
  public float sphereScale = 0.1f;

  private Color color;
  private Gradient trailGradient;
  private AudioPeer audioPeer;
  private int bandBufferIndex;

  private GameObject container;
  private Bounds containerBounds;
  private TrailRenderer trailRenderer;
  private BeatCounter beatCounter;
  private MeshRenderer meshRenderer;
  private int[] xyz;
  private float speed = 3f;
  private float lerpedSpeed;

  //[Range(0, 1)]
  //public float bandBufferThreshold;

  bool isMoving = false;
  Vector3 direction = Vector3.zero;

  float h;
  float s;
  float v;

  /* 
  NOTE TO SELF:
  IPV met een static speed te updaten als volume over een bepaalde grens gaat,
  zet deze grens heel erg laag en map de speed van de box van 0-1. Je moet nu wel
  een manier verzinnen om vaker van direction te veranderen (bijv bij hoge volume change).
   */
  void Start()
  {
    container = transform.parent.gameObject;

    TrailContainer containerInstance = container.GetComponent<TrailContainer>();
    color = containerInstance.color;
    trailGradient = containerInstance.trailGradient;
    audioPeer = containerInstance.audioPeer;
    bandBufferIndex = containerInstance.bandBufferIndex;

    containerBounds = container.GetComponent<Collider>().bounds;
    meshRenderer = GetComponent<MeshRenderer>();
    meshRenderer.material.SetColor("_Color", color);
    trailRenderer = GetComponentInChildren<TrailRenderer>();
    //trailRenderer.time = 5f;
    beatCounter = audioPeer.GetComponent<BeatCounter>();
    xyz = new int[3];

    Color.RGBToHSV(color, out h, out s, out v);

    trailRenderer.colorGradient = trailGradient;
  }

  // Update is called once per frame
  void Update()
  {
    float bufferValue = audioPeer._audioBandBuffer[bandBufferIndex];
    float scale = bufferValue * sphereScale;
    transform.localScale = new Vector3(scale, scale, scale);
    trailRenderer.widthMultiplier = bufferValue * trailScale;

    color = Color.HSVToRGB(h, 1.5f - audioPeer._audioBandBuffer[bandBufferIndex], v);
    meshRenderer.material.SetColor("_Color", color);

    lerpedSpeed = Mathf.Lerp(0, speed, audioPeer._audioBandBuffer[bandBufferIndex]);

    if (beatCounter.NewBeat())
    {
      xyz[0] = 0;
      xyz[1] = 0;
      xyz[2] = 0;
      int index = Random.Range(0, 3);
      int value = Random.Range(0, 2) * 2 - 1;
      xyz[index] = value;

      direction = new Vector3(xyz[0], xyz[1], xyz[2]);
      isMoving = true;
      trailRenderer.startWidth = scale;
    }

    if (isMoving)
    {
      Vector3 newPosition = transform.position + direction * Time.deltaTime * lerpedSpeed;
      if (containerBounds.Contains(newPosition))
      {
        transform.position += direction * Time.deltaTime * lerpedSpeed;
      }
      else
      {
        direction = containerBounds.center - transform.position;
        int index = Random.Range(0, 3);
        if (index == 0) { direction[1] = 0; direction[2] = 0; }
        else if (index == 1) { direction[0] = 0; direction[2] = 0; }
        else if (index == 2) { direction[0] = 0; direction[1] = 0; }

        direction = direction.normalized;
        transform.position += direction * Time.deltaTime * lerpedSpeed;
      }
    }

  }
}