using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseTrigger : MonoBehaviour
{
    public bool isTag; //used to determine if should compare the tag of this object, or the tag of object that collided with this object
    private CommonEnums.HouseResponses response;
    private int responseCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        //limits each object to only sending 1 response per level
        if(responseCount < 1)
        {
            if (isTag)
            {
                response = ResponseCollector.Instance.OnCheckAcceptableTags.Invoke(gameObject.tag);
                Debug.Log("gameO" + other.gameObject.name);
            }
            else
            {
                response = ResponseCollector.Instance.OnCheckAcceptableTags.Invoke(other.tag);
            }

            if (response == CommonEnums.HouseResponses.None)
            {
                return;
            }

            //note: one unique case, all other instances handled differently
            if(TrollController.Instance != null)
            {
                TrollController.Instance.OnTrollSceneResponseSelected?.Invoke(response);
            }
            else if (MaskController.Instance != null)
            {
                MaskController.Instance.OnMaskSceneResponseSelected?.Invoke(response, gameObject);
            }
            else if(TurretProgressionController.Instance != null)
            {
                TurretProgressionController.Instance.OnEggSceneResponseSelected(response, gameObject);
            }
            else
            {
                ResponseCollector.Instance.OnResponseSelected?.Invoke(response);
            }

            responseCount++;
        }
    }
}
