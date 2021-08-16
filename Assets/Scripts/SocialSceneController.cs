using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SocialSceneController : MonoBehaviour
{
    public AudioSource[] audioOptions;

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
                audioOptions[1].Play();
                break;
            case 2:
                //Gryfindor
                audioOptions[2].Play();
                break;
            case 3:
                //Hufflepuff
                audioOptions[3].Play();
                break;
            case 4:
                //Slytherin
                audioOptions[4].Play();
                break;
            default:
                break;
        }

        StartCoroutine(TimerToEndScene());
    }

    private IEnumerator TimerToEndScene()
    {
        yield return new WaitForSeconds(5);

        ProgressionController.Instance.OnLoadNextScene();
    }

    void Destroy()
    {
        ResponseCollector.Instance.OnResponseSelected -= SocialCommand;
    }
}
