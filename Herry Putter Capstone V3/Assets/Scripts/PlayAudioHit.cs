using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioHit : MonoBehaviour {
  AudioSource audioSource;
    public OculusSpells oculusSpells;
    public bool iHit = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //void Update()
    //{
        
    //        //audioSource = oculusSpells.hitObject.GetComponent<AudioSource>();
    //        //audioSource.Play();
    //        //iHit = true;
        
    //}

    //FOR some reason broken
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        audioSource.Play();
    }
}
