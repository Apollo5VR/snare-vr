using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserHealth : MonoBehaviour
{
    public float healthPoints
    {
        get
        {
            return _healthPoints;
        }
        private set
        {
            _healthPoints = Mathf.Clamp(value, 0f, 100f);

            if(_healthPoints <= 0)
            {
                //TODO - Send Dead Message & Grey everything out / UI offer to Start Over
                Debug.Log("Player has died");
            }
        }
    }

    [SerializeField]
    private float _healthPoints = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        //TODO - check for Instance null
        ScriptsConnector.Instance.OnSetHealth += SetHealth;
        ScriptsConnector.Instance.GetHealth += GetHealth;
    }

    private float GetHealth(string playerID)
    {
        //TODO - V2 - set the health by playerId (ie if the player this script connected matches playerId, update adequetely) 

        return healthPoints;
    }

    private void SetHealth(string playerId, float healthValue)
    {
        //TODO - V2 - set the health by playerId (ie if the player this script connected matches playerId, update adequetely) 

        //set health value
        healthPoints = healthValue;

        //TODO - if trying to reduce calls, relocate to on session quit / exit
        ScriptsConnector.Instance.OnSaveHealthToUGS?.Invoke("stats", healthPoints.ToString());
    }

    private void OnDestroy()
    {
        if(ScriptsConnector.Instance != null)
        {
            ScriptsConnector.Instance.OnSetHealth -= SetHealth;
            ScriptsConnector.Instance.GetHealth -= GetHealth;
        }
    }
}
