using System;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;


public class CloudCodeDataCache : Singleton<CloudCodeDataCache> 
{
    public float HealthPoints
    {
        get
        {
            return _healthPoints;
        }
    }

    private float _healthPoints;

    // Start is called before the first frame update
    void Start()
    {
        ScriptsConnector.Instance.OnGetHealthFromUGS += GrabAndCacheGSData;
    }

    public void GrabAndCacheGSData()
    {
        CacheHealthFromUGS();


        //TODO - add the other data for cached items
    }

    public async void CacheHealthFromUGS()
    {
        try
        {
            _healthPoints = await CloudCodeManager.instance.CallGetHealthRemainingEndpoint();

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

    private void OnDestroy()
    {
        if(ScriptsConnector.Instance != null)
        {
            ScriptsConnector.Instance.OnGetHealthFromUGS -= GrabAndCacheGSData;
        }
    }
}
