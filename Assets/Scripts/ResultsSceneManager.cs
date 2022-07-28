using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsSceneManager : MonoBehaviour
{
    public GameObject[] houseCrestsHolder; //none, ravenClawPoints, gryffindorPoints, hufflePuffPoints, slytherinPoints
    public Text resultTextHolder;
    public Text hiddenResultTextHolder;

    private void Start()
    {
        ResponseCollector.Instance.OnToggleResultsCalculation?.Invoke();

        if (ProgressionController.Instance.isManualSelection)
        {
            StartCoroutine(CloseGame());
        }
    }

    public void TriggerHatStuff()
    {
        StartCoroutine(PickupDelay());
    }

    private IEnumerator PickupDelay()
    {
        yield return new WaitForSeconds(2);

        //0 is the questions scene
        ProgressionController.Instance.OnLoadSelectedScene(0);
    }

    private IEnumerator CloseGame()
    {
        yield return new WaitForSeconds(10);

        ProgressionController.Instance.OnLoadNextScene(0);
    }
}
