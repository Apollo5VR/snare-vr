/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTestEventBroadcaster : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        TestScriptsConnector.OnModifyHealth += ModifyHealth;
        TestScriptsConnector.GetHealth += GetHealth;
    }

    private float GetHealth(string arg)
    {
        return ScriptsConnector.Instance.GetHealth.Invoke(arg);
    }

    private void ModifyHealth(string arg1, float arg2)
    {
        ScriptsConnector.Instance.OnModifyHealth?.Invoke(arg1, arg2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
*/
