using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatAnimCancel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);

        if (other.name == "RightHandSphere")
        {
            {
                //stop animation
                this.GetComponent<Animator>().enabled = false;
            }
        }
    }
 }
