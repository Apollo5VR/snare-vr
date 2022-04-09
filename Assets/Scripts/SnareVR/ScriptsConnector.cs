using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScriptsConnector : MonoBehaviour
{
    public static ScriptsConnector Instance { get; private set; }
    
    //User Health data actions
    public Action<string, float> SetHealth;
    public Func<string, float> GetHealth; //can only sub this once (func)

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
