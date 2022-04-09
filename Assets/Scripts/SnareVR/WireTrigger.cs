using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireTrigger : MonoBehaviour
{
    public int progressionInt;

    private void OnTriggerEnter(Collider other)
    {
        //TODO refactor this to be not dumb
        if(other.name == "GrabCube" || other.name == "WireGrabber")
        {
            WireController.Instance.OnWireSectionComplete?.Invoke(progressionInt);
        }
    }
}
