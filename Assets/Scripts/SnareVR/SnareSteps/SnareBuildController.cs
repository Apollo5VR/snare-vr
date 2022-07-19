using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public class SnareBuildController: StateController<SnareBuildController>
{
    public List<AStateMono<SnareBuildController>> buildStates; //the list of processes that need to be done in particular order
    private GameObject[] sceneResetObjs;
    private int stateIndex = 0;

    private void Awake()
    {
        SnareBuildController[] controllers = FindObjectsOfType<SnareBuildController>();

        if(controllers.Length > 1)
        {
            Debug.LogWarning("More than 1 Snare Controller in scene");
        }

        sceneResetObjs = GameObject.FindGameObjectsWithTag("SnarePart");
    }

    protected override void Start()
    {
        //scene objects reset
        foreach (GameObject go in sceneResetObjs)
        {
            go.SetActive(false);
        }

        CurrentState = buildStates[stateIndex];

        CurrentState.EnterState(this);  
    }

    protected override void Update()
    {
        CurrentState.ControlledUpdate(this);
    }

    public void ProgressState()
    {
        IStateActions<SnareBuildController> nextState;

        int stateIndex = 0;
        stateIndex = buildStates.IndexOf((AStateMono<SnareBuildController>)CurrentState);

        if ((stateIndex + 1) < buildStates.Count)
        {
            nextState = buildStates[stateIndex + 1];
        }
        else
        {
            Debug.Log("Snare Sequence Complete");
            return;
        }

        SwitchState(nextState);
    }
}
