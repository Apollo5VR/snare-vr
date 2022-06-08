using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserHealthManager : MonoBehaviour
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
        ScriptsConnector.Instance.OnCacheHealthFromUGS += CacheHealthFromUGS;
        ScriptsConnector.Instance.OnModifyHealth += ModifyHealth;
        ScriptsConnector.Instance.GetHealth += GetLocalHealth;
    }

    private float GetLocalHealth(string playerID)
    {
        //TODO - V2 - set the health by playerId (ie if the player this script connected matches playerId, update adequetely) 

        return healthPoints;
    }

    public async void CacheHealthFromUGS()
    {
        try
        {
            //TODO - update this singleton to use the scriptsconnector
            float health = await CloudCodeManager.instance.CallGetHealthRemainingEndpoint();
            healthPoints = health;

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

    private void ModifyHealth(string playerId, float healthMod)
    {
        //TODO - V2 - set the health by playerId (ie if the player this script connected matches playerId, update adequetely) 

        healthPoints += healthMod;

        //TODO - if trying to reduce calls, relocate to on session quit / exit
        ScriptsConnector.Instance.OnSaveHealthToUGS?.Invoke("stats", healthPoints.ToString());
    }

    private void OnDestroy()
    {
        if(ScriptsConnector.Instance != null)
        {
            ScriptsConnector.Instance.OnCacheHealthFromUGS -= CacheHealthFromUGS;
            ScriptsConnector.Instance.OnModifyHealth -= ModifyHealth;
            ScriptsConnector.Instance.GetHealth -= GetLocalHealth;
        }
    }
}
