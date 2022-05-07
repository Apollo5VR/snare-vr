using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Unity.Services.Analytics;
using System.Linq;

//TODO - refactor this so it can progress to each scene - simply finding the appropriate objects to activate via its tag types
public class TrapController : MonoBehaviour
{
    public GameObject wireAroundTree;
    public GameObject rabbit;
    public Transform trapDestination;

    // Start is called before the first frame update
    void Start()
    {
        ScriptsConnector.Instance.OnWireSectionComplete += SetTrap;
        ScriptsConnector.Instance.OnGetTrapDestination += GetDestination;
        wireAroundTree.SetActive(false);
        rabbit.SetActive(false);
        SetupTrapScene();
    }

    private Transform GetDestination()
    {
        return trapDestination;
    }

    private void SetTrap(int progessionPoint)
    {
        if(progessionPoint == 2)
        {
            wireAroundTree.SetActive(true);
            ScriptsConnector.Instance.OnSetTrapTriggerTime?.Invoke();
        }

        //if zone 2, initiate wolf scenario
        if (ScriptsConnector.Instance.OnGetCurrentScene?.Invoke() == 4)
        {
            //do wolves - once all wolves eaten you or you killed, nothing. they can return to home or leave game
            ScriptsConnector.Instance.OnStartWolfSequence?.Invoke();
        }
    }

    public async void SetupTrapScene()
    {
        try
        {
            //TODO - update this singleton to use the scriptsconnector
            float time = await CloudCodeManager.instance.CallGetTrapTimeRemainingEndpoint();

            //TODO - better idea in the future to not user a hardcoded value to determine if a trap exists (should just pull if trap exists for this scenario)
            if (time == -1)
            {
                //Users first time OR last session they checked trap, but didnt set up another one (we delete on snare check)
                wireAroundTree.SetActive(false);

                ScriptsConnector.Instance.OnUpdateUI(CommonEnums.UIType.Generic, "FIND & SET SNARE. NO SNARE? BUILD ONE AT HOME BASE");
            }
            else
            {
                if (time > 0)
                {
                    //TODO - formatt not readable, refactor
                    ScriptsConnector.Instance.OnUpdateUI(CommonEnums.UIType.Time, "TIME TILL TRAP TRIGGERED: " + time.ToString());
                }
                else if (time <= 0)
                {
                    //Load directly into check trap mode

                    wireAroundTree.SetActive(true);

                    //automatically check trap on game loaded
                    ScriptsConnector.Instance.OnCheckTrap?.Invoke();
                }
            }

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
}
