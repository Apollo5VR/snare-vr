using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Grab : AStateMono<SnareBuildController>
{
    [SerializeField] private bool objectiveMet; //TODO - consider adding this to parent Mono - or have this inherit from a "ProcessState"

    public override void EnterState(in IStateController<SnareBuildController> controller)
    {
        objectiveMet = false;
    }

    public override void ControlledUpdate(in IStateController<SnareBuildController> controller)
    {
        if (objectiveMet)
        {
            StatesController.ProgressState();
        }
    }

    public void OnGrab(GameObject grabbed)
    {
        if(objectiveMet == false)
        {
            objectiveMet = true;
        }
    }
}
