using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatController : MonoBehaviour
{
    public ProgressionController progressionController;
    public bool isAnimTrigger;

    private void Start()
    {
        progressionController = GameObject.Find("ProgressionController").GetComponent<ProgressionController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("hat triggered by: " + other.tag);

            if (isAnimTrigger)
            {
                this.GetComponent<Animator>().enabled = false;
            }
            else
            {
                progressionController.LoadNextScene();
            }
        }
    }
}
