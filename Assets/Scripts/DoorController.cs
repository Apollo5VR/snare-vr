using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public ProgressionController progressionController;

    private void Start()
    {
        progressionController = GameObject.Find("ProgressionController").GetComponent<ProgressionController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Scene 1 Door collided with:" + other.name + "tag is: " + other.tag);

        if(other.tag == "Player")
        {
            //TODO call scene progress on sceneManager
            progressionController.LoadNextScene();
        }
    }
}