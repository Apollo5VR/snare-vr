using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//depreciated since state machine refactor
public class WireTrigger : MonoBehaviour
{
    public int progressionInt;

    private void OnTriggerEnter(Collider other)
    {
        //TODO refactor this to be not dumb
        if(other.name == "GrabCube" || other.name == "CompletedWireGrabber")
        {
            if(other.name == "CompletedWireGrabber")
            {
                other.gameObject.SetActive(false);
            }

            ScriptsConnector.Instance.OnWireSectionComplete?.Invoke(progressionInt);
        }
    }
}
