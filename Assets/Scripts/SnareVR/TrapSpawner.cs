using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpawner : MonoBehaviour
{
    //TODO - Make persistant

    public GameObject[] trapLocationGOs;
    public GameObject trapPrefab;

    // Purpose: to spawn the trap in whichever scene is loaded (allow dynamic nature)
    void Start()
    {
        if (ScriptsConnector.Instance != null)
        {
            ScriptsConnector.Instance.OnPrepareTrap += SpawnTrap;
        }
    }

    //only happens on scene load / start
    private void SpawnTrap()
    {
        //Get potential trap locations in scene - ie get type of "TrapLocation"
        trapLocationGOs = GameObject.FindGameObjectsWithTag("TrapLocation");

        if(trapLocationGOs.Length > 0)
        {
            //randomize which one to choose
            int choice = Random.Range(0, trapLocationGOs.Length);

            //instantiate TrapController (with appropriate child objects)
            Instantiate(trapPrefab, trapLocationGOs[choice].transform.position, Quaternion.identity);

            //deactivate visibility of all other choices
            for(int i = 0; i < trapLocationGOs.Length; i++)
            {
                if(i != choice)
                {
                    trapLocationGOs[i].SetActive(false);
                }
            }
        }
    }

    private void OnDestroy()
    {
        if(ScriptsConnector.Instance != null)
        {
            ScriptsConnector.Instance.OnPrepareTrap -= SpawnTrap;
        }
    }
}
