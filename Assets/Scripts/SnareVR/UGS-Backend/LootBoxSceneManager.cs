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
        ScriptsConnector.Instance.OnSetTrapTriggerTime += SetTrapTriggeredTime;
        ScriptsConnector.Instance.OnCheckTrap += CheckTrap;
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

            //call scriptconnector to update Scene on Wirecontorller
            ScriptsConnector.Instance.OnTrapTriggerTimeSet?.Invoke();

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

    public async void CheckTrap()
    {
        try
        {
            //sceneView.Disable();

            // Call Cloud Code js script and wait for grant to complete.
            bool rabbitCaught = await CloudCodeManager.instance.CallCheckTrapEndpoint();
            if (this == null) return;

            if(rabbitCaught)
            {
                //TODO call reward Item
                ScriptsConnector.Instance.OnRabbitCaught?.Invoke(rabbitCaught);
            }
            else
            {
                ScriptsConnector.Instance.OnUpdateUI(CommonEnums.UIType.Generic, "SORRY, NO RABBIT CAUGHT. SET ANOTHER.");
            }

            //TODO - update so we get this key string from some organizated location
            ScriptsConnector.Instance.OnDeleteKey?.Invoke("TRAP_TRIGGER_TIME");
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
        //TODO - do this check everywhere
        if(ScriptsConnector.Instance != null)
        {
            ScriptsConnector.Instance.OnSetTrapTriggerTime -= SetTrapTriggeredTime;
        }
    }
}
