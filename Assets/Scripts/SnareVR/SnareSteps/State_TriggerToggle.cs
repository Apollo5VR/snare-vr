using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_TriggerToggle : AStateMono<SnareBuildController>
{
    public GameObject[] activatableObjects;
    [SerializeField] private bool objectiveMet; 

    public override void EnterState(in IStateController<SnareBuildController> controller)
    {
        objectiveMet = false;
        ToggleActive(true);

        base.EnterState(controller);
    }

    public override void ControlledUpdate(in IStateController<SnareBuildController> controller)
    {
        if (objectiveMet)
        {
            ToggleActive(false);
            StatesController.ProgressState();
        }
    }

    public override void CollisionEnter(in GameObject obj)
    {
        objectiveMet = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        //TODO - refactor to not need this exact name, use tag, or enum or go etc
        if(other.gameObject.name == "GrabCube")
        {
            CollisionEnter(other.gameObject);
        }
    }

    public void ToggleActive(bool activate)
    {
        foreach (var objs in activatableObjects)
        {
            objs.SetActive(activate);
        }
    }
}
