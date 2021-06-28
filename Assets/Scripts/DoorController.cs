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
        if(other.tag == "Player")
        {
            if(progressionController.nextLevel == 1)
            {
                //TODO call scene progress on sceneManager
                progressionController.LoadNextScene();
            }
            else if(progressionController.nextLevel == 2)
            {
                if (progressionController.petPlaced)
                {
                    //TODO call scene progress on sceneManager
                    progressionController.LoadNextScene();
                }
                else
                {
                    Debug.Log("Notify user that they still need to place pet / choose pet");
                }
            }
        }
    }

    public void OnSnappedPet()
    {
        progressionController.petPlaced = true;
    }
}