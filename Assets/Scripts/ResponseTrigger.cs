using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseTrigger : MonoBehaviour
{
    public bool isTag; //used to determine if should compare the tag of this object, or the tag of object that collided with this object
    private CommonEnums.HouseResponses response;

    private void OnTriggerEnter(Collider other)
    {
        response = ResponseCollector.OnCheckAcceptableTags.Invoke(other.tag);

        if (response == CommonEnums.HouseResponses.None)
        {
            return;
        }

        //"Subject" 
        ResponseCollector.OnResponseSelected?.Invoke(response);
    }
}
