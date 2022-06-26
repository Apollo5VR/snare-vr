using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Services.Analytics;

public class SocialSceneController : MonoBehaviour
{
    public AudioSource[] audioOptions;
    public Transform[] ballLocations;
    public BallGameController ballGameController;

    // Use this for initialization
    void Start()
    {
        ResponseCollector.Instance.OnResponseSelected += SocialCommand;
    }

    private void SocialCommand(CommonEnums.HouseResponses response)
    {
        switch ((int)response)
        {
            case 1:
                //Ravenclaw
                ballGameController.gameObject.transform.position = ballLocations[0].transform.position;
                ballGameController.gameObject.transform.rotation = ballLocations[0].transform.rotation;
                audioOptions[0].Play();
                break;
            case 2:
                //Gryfindor
                ballGameController.gameObject.transform.position = ballLocations[1].transform.position;
                ballGameController.gameObject.transform.rotation = ballLocations[1].transform.rotation;
                audioOptions[1].Play();
                break;
            case 3:
                //Hufflepuff
                ballGameController.gameObject.transform.position = ballLocations[2].transform.position;
                ballGameController.gameObject.transform.rotation = ballLocations[2].transform.rotation;
                audioOptions[2].Play();
                break;
            case 4:
                //Slytherin
                ballGameController.gameObject.transform.position = ballLocations[3].transform.position;
                ballGameController.gameObject.transform.rotation = ballLocations[3].transform.rotation;
                audioOptions[3].Play();
                break;
            default:
                break;
        }

        ballGameController.gameObject.SetActive(true);
        ballGameController.appearExplosion.Play();

        ResponseCollector.Instance.OnResponseSelected -= SocialCommand;

        //TODO - refactor to one location (so we only need 1 script to have Analytics dependency)
        /*
        if (!Application.isEditor)
        {
            //Analytics Beta
            Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    { "specificQuestion", "Social" },
                    { "houseIndex", (int)response },
                };
            AnalyticsService.Instance.CustomData("questionResponse", parameters);
        }
        */
    }

    void Destroy()
    {
        ResponseCollector.Instance.OnResponseSelected -= SocialCommand;
    }
}
