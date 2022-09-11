using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablesManager : MonoBehaviour
{
    [SerializeField]
    private BoxCollider[] m_TrophyAnchors;

    public Material animalMat;
    //public BoxCollider boxCollider;
    //public GameObject bear;

    //private AsyncOperationHandle m_TrophyLoadingHandle;

    [SerializeField]
    private List<AssetReference> _trophyReferences;

    private List<string> userValidTrophies;
    private List<GameObject> addressableGOs;

    private void Awake()
    {
        ScriptsConnector.Instance.OnRequestItems?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        addressableGOs = new List<GameObject>();
        StartCoroutine(WaitThis());
    }

    private void SetTrophies()
    {
        //TODO - pull the handle, creating array of all CCD items
        //then pull inventory items. if has type of "trophy" then add it to the populating array
        //these inventory items will match the exact names of the CCD items, allowing to determine "if has inventory item, spawn CCD item of same name"
        //then simply load them up 1 - 4 on the trophy stands via those stored transform locations (always the same)
        //Sizing? Handled by below (means new object has to be confined within the 1:1:1 box collider format we use for trophies
        userValidTrophies = ScriptsConnector.Instance.OnRequestItemNames?.Invoke();

        foreach (var trophy in _trophyReferences)
        {
            //skip loading trophies user does not have in their inventory
            //if(!userValidTrophies.Contains(trophy.editorAsset.name))
            //{
            //    continue;
            //}

            Addressables.InstantiateAsync(trophy).Completed += (obj) =>
            {
                BoxCollider transformLocation = ReturnLocationAvailable();

                GameObject resultGO = obj.Result;
                addressableGOs.Add(resultGO);
                Collider collider = resultGO.GetComponent<BoxCollider>();
                //GameObject trophyGo = Instantiate(go, boxCollider.transform); //depreciated 9.10, instantiated earlier
                resultGO.transform.parent = transformLocation.transform;
                resultGO.transform.localScale = new Vector3(transformLocation.bounds.size.x / collider.bounds.size.x, transformLocation.bounds.size.y / collider.bounds.size.y, transformLocation.bounds.size.z / collider.bounds.size.z);  //change to actual adjustment
                resultGO.transform.position = transformLocation.transform.position;
                resultGO.transform.localRotation = Quaternion.identity;

                resultGO.GetComponentInChildren<MeshRenderer>().material = animalMat;

                Debug.Log("Trophy has successfully loaded & positioned");
            };
        }
    }

    private BoxCollider ReturnLocationAvailable()
    {
        BoxCollider collider = null;

        foreach(BoxCollider box in m_TrophyAnchors)
        {
            if(box.transform.childCount == 0)
            {
                collider = box;
                break;
            }
        }

        return collider;
    }

    private IEnumerator WaitThis()
    {
        yield return new WaitForSeconds(1.5f);

        userValidTrophies = ScriptsConnector.Instance.OnRequestItemNames?.Invoke();

        SetTrophies();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        foreach (var trophy in addressableGOs)
        {
            //TODO - to do this we need the AssetReference stored?

            Addressables.ReleaseInstance(trophy);
        }
    }
}
