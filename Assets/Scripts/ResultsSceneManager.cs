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

    //should be called by child Hat object
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //0 is the questions scene
            ProgressionController.Instance.OnLoadChallengeScene(0);
        }
    }

    private IEnumerator CloseGame()
    {
        yield return new WaitForSeconds(10);

        Application.Quit();
    }
}
