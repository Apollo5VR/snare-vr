using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatAnimCancel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("hat trigger collided with: " + other.tag);
            this.GetComponent<Animator>().enabled = false;
        }
    }
 }
