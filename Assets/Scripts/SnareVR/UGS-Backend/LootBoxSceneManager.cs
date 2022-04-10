using System;
using Unity.Services.Authentication;
using Unity.Services.CloudCode;
using Unity.Services.Core;
using UnityEngine;


public class LootBoxSceneManager : MonoBehaviour
{
    //public LootBoxesSampleView sceneView;
    public bool test = false;

    private void Start()
    {
        ScriptsConnector.Instance.SetTrapTriggerTime += SetTrapTriggeredTime;
    }

    public void Update()
    {
        if(test)
        {
            SetTrapTriggeredTime();
            test = false;
        }
    }

    public async void SetTrapTriggeredTime()
    {
        try
        {
            //sceneView.Disable();

            // Call Cloud Code js script and wait for grant to complete.

            await CloudCodeManager.instance.CallSetTrapTriggeredTimeEndpoint();
            if (this == null) return;
        }
        catch (CloudCodeResultUnavailableException)
        {
            // Exception already handled by CloudCodeManager
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        finally
        {
            if (this != null)
            {
                //sceneView.Enable();
            }
        }
    }

    private void OnDestroy()
    {
        ScriptsConnector.Instance.SetTrapTriggerTime -= SetTrapTriggeredTime;
    }
}
