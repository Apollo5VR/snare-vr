using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScriptsConnector : MonoBehaviour
{
    public static ScriptsConnector Instance { get; private set; }
    
    //User Health data actions
    public Action<string, float> OnSetHealth;
    public Func<string, float> GetHealth; //can only sub this once (func)
    public Action<string, string> OnSaveHealthToUGS;

    //Trap Data actions
    public Action OnSetTrapTriggerTime;
    public Action OnCheckTrap;
    public Action<bool> OnRabbitCaught;

    //UI
    public Action<string, string> OnUpdateUI; //first string is to declare what UI type update it is, second string is the value you're updating to. //TODO - using string is best?

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

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
