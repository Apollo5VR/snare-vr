using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        switch (other.name)
        {
            case "dfd":
                Debug.Log("ok");
                //ResponseCollector.Instance.XX
                break;
        }
    }
}
