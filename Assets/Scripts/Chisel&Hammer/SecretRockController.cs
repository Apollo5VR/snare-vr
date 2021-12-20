using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretRockController : MonoBehaviour
{
    private float yOriginValue;
    private int sentCount;

    private void Start()
    {
        sentCount = 0;
        yOriginValue = transform.position.y;
    }

    private void OnCollisionStay(Collision collision)
    {
        if(sentCount == 0)
        {
            if ((transform.position.y - yOriginValue) > 0.01)
            {
                //rock has been pressed
                if (BNG.WholeStoneController.Instance != null)
                {
                    BNG.WholeStoneController.Instance.OnRockInteraction(CommonEnums.HouseResponses.Ravenclaw);
                    sentCount++;
                }
            }
        }
    }
}
