using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerShellSelection : MonoBehaviour
{
    public bool isBall;
    private GameObject shell;

    private void OnTriggerEnter(Collider other)
    {
        if (!isBall)
        {
            if (other.gameObject.name == "Wand")
            {
                //"Subject" 
                ShellSelectionManager.OnShellSelected?.Invoke(this.gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isBall)
        {
            if(shell != other.gameObject)
            {
                ShellSelectionManager.OnBallStopped?.Invoke(other.gameObject);
            }

            shell = other.gameObject;
        }
    }
}
