using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScriptsConnector : MonoBehaviour
{
    public static ScriptsConnector Instance { get; private set; }

    //User Health data actions
    public Action OnCacheHealthFromUGS;
    public Action<string, float> OnSetHealth;
    public Func<string, float> GetHealth; //can only sub this once (func)
    public Action<string, string> OnSaveHealthToUGS;
    //public Func<string, float> OnGetTimerFromUGS; //depreciated

    //Trap Data actions
    public Action OnSetTrapTriggerTime;
    public Action OnTrapTriggerTimeSet;
    public Action OnCheckTrap;
    public Action<bool> OnRabbitCaught;
    public Action<string> OnDeleteKey;
    public Action<int> OnWireSectionComplete;

    //archer scene
    public Action OnStartEnemySpawnSequence;
    public Action<GameObject, bool> OnWolfDeath;
    public Func<Transform> OnGetTrapDestination;

    //UI
    public Action<CommonEnums.UIType, string> OnUpdateUI; //first string is to declare what UI type update it is, second string is the value you're updating to. //TODO - using string is best?
    public Action OnReturnMap;

    //scene data
    public Func<int> OnGetCurrentScene;

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
