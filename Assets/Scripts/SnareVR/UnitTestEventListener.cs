/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTestEventListener : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {  
        //TestsConnector.TestThis?.Invoke();
    }

    //where we sub to actual ingame playmode events - to communicate them to the Unit Tests
    public void Init()
    {
        ScriptsConnector.Instance.OnPrepareTrap += SpawnTrap;
    }

    private void SpawnTrap()
    {
        TestScriptsConnector.SpawnTrap?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
*/
