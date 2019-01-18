using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatCounter : MonoBehaviour
{
  public int bpm;

  private bool newBeat;
  private float interval;
  private AudioSource audioSource;
  private float time;
  private float timeSubtractor;

  // Use this for initialization
  void Start()
  {
    //time = 0f;
    newBeat = false;
    interval = 60f / (float)bpm;

    audioSource = GetComponent<AudioSource>();
  }

  // Update is called once per frame
  void Update()
  {
    if (newBeat) newBeat = false;

    time = audioSource.time - timeSubtractor;

    if (time >= interval)
    {
      timeSubtractor += interval;
      newBeat = true;
      time = 0f;
      return;
    }
  }

  public bool NewBeat()
  {
    return newBeat;
  }
}
