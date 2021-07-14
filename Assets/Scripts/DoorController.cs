using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(ProgressionController.Instance.nextLevel == 1)
            {
                //TODO call scene progress on sceneManager
                ProgressionController.Instance.LoadNextScene();
            }
            else if(ProgressionController.Instance.nextLevel == 2)
            {
                if (ProgressionController.Instance.petPlaced)
                {
                    //TODO call scene progress on sceneManager
                    ProgressionController.Instance.LoadNextScene();
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
        ProgressionController.Instance.petPlaced = true;
    }
}