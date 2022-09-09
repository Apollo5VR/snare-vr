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

    // Start is called before the first frame update
    void Start()
    {
        SetTrophies();
    }

    private void SetTrophies()
    {
        m_TrophyLoadingHandle = Addressables.InstantiateAsync("bearhead", m_TrophyAnchors[0]);//bearhead

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
}
