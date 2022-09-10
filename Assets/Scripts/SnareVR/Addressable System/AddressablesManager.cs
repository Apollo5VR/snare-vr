using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablesManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] m_TrophyAnchors;

    private AsyncOperationHandle m_TrophyLoadingHandle;

    public BoxCollider boxCollider;
    public GameObject bear;

    // Start is called before the first frame update
    void Start()
    {
        SetTrophies();
    }

    private void SetTrophies()
    {
        //TODO - pull the handle, creating array of all CCD items
        //then pull inventory items. if has type of "trophy" then add it to the populating array
        //these inventory items will match the exact names of the CCD items, allowing to determine "if has inventory item, spawn CCD item of same name"
        //then simply load them up 1 - 4 on the trophy stands via those stored transform locations (always the same)
        //Sizing? Handled by below (means new object has to be confined within the 1:1:1 box collider format we use for trophies

        //Collider collider = bear.GetComponent<BoxCollider>();
        //GameObject bearGo = Instantiate(bear, boxCollider.transform);
        //bearGo.transform.localScale = new Vector3(boxCollider.bounds.size.x / collider.bounds.size.x, boxCollider.bounds.size.y / collider.bounds.size.y, boxCollider.bounds.size.z / collider.bounds.size.z);  //change to actual adjustment

        m_TrophyLoadingHandle = Addressables.InstantiateAsync("trophy1", m_TrophyAnchors[0]);//bearhead

        m_TrophyLoadingHandle.Completed += OnTrophiesInstantiated;
    }

    private void OnTrophiesInstantiated(AsyncOperationHandle obj)
    {
        if(obj.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Trophies have successfully loaded");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        m_TrophyLoadingHandle.Completed -= OnTrophiesInstantiated;
        Addressables.Release(m_TrophyLoadingHandle);
    }
}
