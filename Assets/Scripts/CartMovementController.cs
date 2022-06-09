using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;

public class CartMovementController : MonoBehaviour
{
    public GameObject cart;
    public GameObject moveTrickObjects;
    public GameObject petObjects;
    private float moveSpeed = 0.075f;
    public bool go;

    public BNG.Grabbable cartGrabbable;
    private Component halo;

    private void Start()
    {
        halo = this.GetComponent("Halo");
    }

    private void OnTriggerEnter(Collider other)
    {
        CommonEnums.HouseResponses houseResponse = CommonEnums.HouseResponses.None;

        switch (other.gameObject.tag)
        {
            case "Ravenclaw":
                houseResponse = CommonEnums.HouseResponses.Ravenclaw;
                go = true;
                break;
            case "Gryfindor":
                houseResponse = CommonEnums.HouseResponses.Gryfindor;
                go = true;
                break;
            case "Hufflepuff":
                houseResponse = CommonEnums.HouseResponses.Hufflepuff;
                go = true;
                break;
            case "Slytherin":
                houseResponse = CommonEnums.HouseResponses.Slytherin;
                go = true;
                break;
            default:
                houseResponse = CommonEnums.HouseResponses.None;
                go = false;
                break;
        }

        if (go)
        {
            halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
            cartGrabbable.enabled = true;

            ResponseCollector.Instance.OnResponseSelected?.Invoke(houseResponse);

            if (!Application.isEditor)
            {
                //Analytics Beta
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    { "specificQuestion", "Pets" },
                    { "houseIndex", (int)houseResponse },
                };
                AnalyticsService.Instance.CustomData("questionResponse", parameters);
            }
        }
    }

    public void RunCartSim()
    {
        StartCoroutine(DelayedMoveTrick()); //TODO - relocate this
    }


    private IEnumerator DelayedMoveTrick()
    {
        //yield return new WaitForSeconds(12);

        petObjects.SetActive(false);

        yield return null;

        while (moveTrickObjects.transform.position != (gameObject.transform.position + new Vector3(0, 0, -3)))
        {
            moveTrickObjects.transform.position = Vector3.MoveTowards(moveTrickObjects.transform.position, gameObject.transform.position + new Vector3(0, 0, -3), moveSpeed);
            yield return null;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
