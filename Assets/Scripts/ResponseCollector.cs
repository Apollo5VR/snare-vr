using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;
using Unity.Services.Analytics;

public class ResponseCollector : MonoBehaviour
{
    public static ResponseCollector Instance { get; private set; }
    public AudioSource crowdCheer;
    public GameObject[] houseCrestsObj; //none, ravenclaw, gryfindor, hufflepuff, slytherin
    public GameObject[] humanRobeObjs; //none, ravenclaw, gryfindor, hufflepuff, slytherin
    //public GameObject humanRobeObject;
    public Material[] robeMats;
    private int challengeSceneSelected;
    public int[] housePoints = new int[5]; // none, ravenClawPoints, gryffindorPoints, hufflePuffPoints, slytherinPoints
    //public GameObject inconclusiveText;
    public Text resultText;
    public String[] resultTextOptions;
    /*
    public bool resultInconclusive = false;
    public GameObject inconclusiveText;
    public bool writtenInconclusive;
    public string conclusiveAnswer;
    public int highestPoints;
    public int lowestPoints;
    public List<int> pointsList = new List<int>();
    public int highestPointsTester;
    public Material witchCurrentMat;
    public Material wizCurrentMat;
    public Material[] matArray;
    */

        //new stuff
    public Action<CommonEnums.HouseResponses> OnResponseSelected;
    public Action OnToggleSceneSelectionResponse;
    public Action OnToggleResultsCalculation;
    public Func<string, CommonEnums.HouseResponses> OnCheckAcceptableTags;
    public bool sceneSelectionResponse = false;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        OnResponseSelected += StoreResponse;
        OnCheckAcceptableTags += CheckAcceptableTags;
        OnToggleSceneSelectionResponse += ToggleSceneSelectionResponse;
        OnToggleResultsCalculation += DetermineFinalHouse;

        robeMats = Resources.LoadAll<Material>("robes");
    }

    private CommonEnums.HouseResponses CheckAcceptableTags(string tag)
    {
        CommonEnums.HouseResponses response;

        switch (tag)
        {
            case "Ravenclaw":
                response = CommonEnums.HouseResponses.Ravenclaw;
                break;
            case "Gryfindor":
                response = CommonEnums.HouseResponses.Gryfindor;
                break;
            case "Hufflepuff":
                response = CommonEnums.HouseResponses.Hufflepuff;
                break;
            case "Slytherin":
                response = CommonEnums.HouseResponses.Slytherin;
                break;
            default:
                response = CommonEnums.HouseResponses.None;
                break;
        }

        return response;
    }

    private void ToggleSceneSelectionResponse()
    {
        sceneSelectionResponse = true;
    }

    private void StoreResponse(CommonEnums.HouseResponses response)
    {
        housePoints[(int)response] += 1;

        if (sceneSelectionResponse)
        {
            challengeSceneSelected = (int)response + 1; //12.20 to reupdate for Santa addition (1 level) - TODO - refactor eventually for infinite levels
            sceneSelectionResponse = false;
            ProgressionController.Instance.OnLoadChallengeScene((int)response); //note: +1 to align with actual scene num
        }
    }

    //this is dumb - just going to make a local manager
    /*
    private void FindAndOrderHouseCrests()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Crest");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = new Vector3(0,0,0);
        float[] crestDistances = [];
        for(int i = 0; i < gos.Length; i++)
        {
            crestDistances[i] = Vector3.Distance(new Vector3(0,0,0), gos[i].transform.position);

            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
    }
    */

    private void DetermineFinalHouse()
    {
        humanRobeObjs = GameObject.FindGameObjectsWithTag("Robe");
        GameObject resultsSceneManagerObj = GameObject.Find("ResultsSceneHatManager");
        ResultsSceneManager rsm = resultsSceneManagerObj.GetComponent<ResultsSceneManager>();
        houseCrestsObj = rsm.houseCrestsHolder;
        resultText = rsm.resultTextHolder;
        rsm.hiddenResultTextHolder.text = "You Found It!" + "\nNa:" + housePoints[0] + "R: " + housePoints[1] + "G: " + housePoints[2] + "\nH: " + housePoints[3] + "S: " + housePoints[4];

        foreach (GameObject obj in houseCrestsObj)
        {
            obj.SetActive(false);
        }

        int greatestHousePoints = Mathf.Max(housePoints[1], housePoints[2], housePoints[3], housePoints[4]); //removing none - housePoints[0]
        int lowestHousePoints = Mathf.Min(housePoints[1], housePoints[2], housePoints[3], housePoints[4]); //removing none - housePoints[0]
        int houseTieTester = 0;
        int finalHouse = 0;

        //do multiple of the house results also equal the highest number i.e. no house wins
        for (int i = 0; i < housePoints.Length; i++)
        {
            if (housePoints[i] == greatestHousePoints)
            {
                finalHouse = i;
                houseTieTester = houseTieTester + 1;
            }
        }

        //if only 1 house is greaters, assign robe colors & cheer
        if (houseTieTester < 2)
        {
            houseCrestsObj[finalHouse].SetActive(true);

            string[] houseNames = Enum.GetNames(typeof(CommonEnums.HouseResponses));

            SwapRobeColors(finalHouse);

            if (!ProgressionController.Instance.isManualSelection)
            {
                resultText.text = "Congrats you " + houseNames[finalHouse] + "! " + resultTextOptions[0]; //HouseNameScrambler()
            }
            else
            {
                resultText.text = houseNames[finalHouse] + "! " + resultTextOptions[2]; //HouseNameScrambler()
                //crowdCheer.Play();
            }

            //TODO - refactor to one location (so we only need 1 script to have Analytics dependency)
        /*
            if (!Application.isEditor)
            {
                //Analytics Beta
                Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    { "houseIndex", finalHouse },
                };
                AnalyticsService.Instance.CustomData("finalResults", parameters);
            }
        */

        }
        //if multiple equal the same, determine inconclusive
        else
        {
            //0 is None
            houseCrestsObj[0].SetActive(true);

            resultText.text = resultTextOptions[1];

            for (int i = 0; i < housePoints.Length; i++)
            {
                housePoints[i] = 0;
            }

            //TODO - trouble with this is its not quite accurate, if they tie 2 houses, they are loaded with another scene that could lead to 4 results
            StartCoroutine(TimerToReloadChallengeScene());

            //crowdCheer.Stop();
        }
    }

    private string HouseNameScrambler()
    {
        switch (UnityEngine.Random.Range(0, 4))
        {
            case 0:
                return "a";
            case 1:
                return "d";
            case 2:
                return "z";
            case 3:
                return "o";
            case 4:
                return "i";
            default:
                return "p";
        }
    }

    private IEnumerator TimerToReloadChallengeScene()
    {
        yield return new WaitForSeconds(10);

        //exclude 0 which is a none scene & 1 which is potions scene (has 2 responses)
        ProgressionController.Instance.OnLoadChallengeScene((int)UnityEngine.Random.Range(2, 5));
    }

    private void SwapRobeColors(int finalHouse)
    {
        foreach (GameObject humanRobeObject in humanRobeObjs)
        {
            Material[] matArray = humanRobeObject.GetComponent<Renderer>().materials;
            Material finalRobeMat = robeMats[finalHouse];

            if (humanRobeObject.name == "YoungWitch")
            {
                matArray[3] = new Material(finalRobeMat);
                humanRobeObject.GetComponent<Renderer>().materials = matArray;
            }
            else
            {
                matArray[1] = new Material(finalRobeMat);
                humanRobeObject.GetComponent<Renderer>().materials = matArray;
            }
        }
    }

    private void OnDestroy()
    {
        OnResponseSelected -= StoreResponse;
        OnCheckAcceptableTags -= CheckAcceptableTags;
        OnToggleSceneSelectionResponse -= ToggleSceneSelectionResponse;
    }
}

