using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;

public class ResponseCollector : MonoBehaviour {
    //public static ResponseCollector Instance { get; private set; }
    public string questionAnswer;
    public string objectUsed;
    public HPSpeechRecognitionEngine hpSpeechRecognitionEngine;
    public HPSpeechRecognitionEngine hpSpeechRecognitionConclusive;
    DestroyProjectile destroyProjectile;
    public string pullCurrentScene;
    public GameObject gryffindorCrest;
    public GameObject hufflePuffCrest;
    public GameObject slytherinCrest;
    public GameObject ravenClawCrest;
    bool crestRecieved = false;
    public int robeCount;
    public GameObject[] humanRobeObjects;
    public GameObject humanRobeObject;
    public Material humanRobeMat;
    public Material gryffindorMat;
    public Material hufflePuffMat;
    public Material slytherinMat;
    public Material ravenClawMat;

    //depreciated
    public int ravenClawPoints;
    public int gryffindorPoints;
    public int hufflePuffPoints;
    public int slytherinPoints;

    //replaced by
    private int[] housePoints = new int[5]; // none, ravenClawPoints, gryffindorPoints, hufflePuffPoints, slytherinPoints

    public bool robesCollected = false;
    public bool jumpToNextScene = false;
    public bool resultInconclusive = false;
    public AudioSource crowdCheer;
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

    public static Action<CommonEnums.HouseResponses> OnResponseSelected;
    public static Action OnToggleSceneSelectionResponse;
    public static Func<string, CommonEnums.HouseResponses> OnCheckAcceptableTags;
    public bool sceneSelectionResponse = false;

    //singleton - depreciated 
    /*
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
    */

    // Use this for initialization
    private void Start ()
    {
        //SceneManager.sceneUnloaded += CollectResponses;
        OnResponseSelected += StoreResponse;
        OnCheckAcceptableTags += CheckAcceptableTags;
        OnToggleSceneSelectionResponse += ToggleSceneSelectionResponse;

        gryffindorMat = (Material)Resources.Load("redcoatcolor", typeof(Material));
        hufflePuffMat = (Material)Resources.Load("yellowcoatcolor", typeof(Material));
        slytherinMat = (Material)Resources.Load("greencoatcolor", typeof(Material));
        ravenClawMat = (Material)Resources.Load("bluecoatcolor", typeof(Material));
        //just in case
        crestRecieved = false;

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

    //TODO - how to abstract this so it can use strings or Enums to determine storage?
    private void StoreResponse(CommonEnums.HouseResponses response)
    {
        #region depreciated
        /*
        switch (response)
        {
            case CommonEnums.HouseResponses.Ravenclaw:
                //more
                specifiedHouse = (int)CommonEnums.HouseResponses.Ravenclaw;
                break;
            case "Gryfindor":
                //do stuff
                specifiedHouse = (int)CommonEnums.HouseResponses.Gryfindor;
                break;
            case "Hufflepuff":
                //df
                specifiedHouse = (int)CommonEnums.HouseResponses.Hufflepuff;
                break;
            case "Slytherin":
                //dfd
                specifiedHouse = (int)CommonEnums.HouseResponses.Slytherin;
                break;
            default:
                //do
                specifiedHouse = (int)CommonEnums.HouseResponses.None;
                break;
        }
        */
        #endregion

        housePoints[(int)response] += 1;

        if (sceneSelectionResponse)
        {
            sceneSelectionResponse = false;
            ProgressionController.OnLoadChallengeScene((int)response);
        }
    }

    // Update is called once per frame
    /*
    void Update () {
        //Debug.Log(humanRobeObjects);

        //just to make sure this updates
        highestPoints = Mathf.Max(gryffindorPoints, slytherinPoints, ravenClawPoints, hufflePuffPoints);

        //pull current scene
        pullCurrentScene = SceneManager.GetActiveScene().name;

        //Soley for testing purposes
        if (jumpToNextScene == true && pullCurrentScene == "Scene 3 - SortingHatQuestioningScene")
            {
            questionAnswer = "Power";
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }

        if (crestRecieved == false && pullCurrentScene == "Scene 5 - GrandHallResultsScene" && highestPoints < 90)
        {
            //get crests
            gryffindorCrest = GameObject.Find("GryffindorCrestPlank");
            ravenClawCrest = GameObject.Find("RavenClawCrestPlank");
            slytherinCrest = GameObject.Find("SlytherinCrestPlank");
            hufflePuffCrest = GameObject.Find("HufflePuffCrestPlank");

            //deactivate
            gryffindorCrest.SetActive(false);
            ravenClawCrest.SetActive(false);
            slytherinCrest.SetActive(false);
            hufflePuffCrest.SetActive(false);

            //gets audio
            crowdCheer = GameObject.Find("CheeringAudio").GetComponent<AudioSource>();

            //gets text
            inconclusiveText = GameObject.Find("EndText");
            //GetComponent<Text>().text
        }

        if (pullCurrentScene == "Scene 5 - GrandHallResultsScene") //&& crestRecieved != true
        {

            if (!robesCollected == true)
            {
                robeCount = GameObject.FindGameObjectsWithTag("Robe").Length;
                //Debug.Log(robeCount);
                //collect robes to reassign color depending on response
                humanRobeObjects = GameObject.FindGameObjectsWithTag("Robe");
                humanRobeObjects.ToList();
                robesCollected = true;
            }
            if (crestRecieved == false && resultInconclusive != true)
            {
                crestRecieved = true;
                if (objectUsed == "Troll")
                {
                    gryffindorPoints++;
                }
                if (questionAnswer == "Glory")
                {
                    gryffindorPoints++;
                }
                if (objectUsed == "MerlinBook (UnityEngine.GameObject)")
                {
                    slytherinPoints++;
                }
                if (questionAnswer == "Power")
                {
                    slytherinPoints++;
                }
                if (objectUsed == "StudentRecords (UnityEngine.GameObject)")
                {
                    ravenClawPoints++;
                }
                if (questionAnswer == "Wisdom")
                {
                    ravenClawPoints++;
                }
                if (objectUsed == "DragonsBloodBottle (UnityEngine.GameObject)")
                {
                    hufflePuffPoints++;
                }
                if (questionAnswer == "Love")
                {
                    hufflePuffPoints++;
                }
                //tell program that if two houses are both the highest value, then result inconclusive

                highestPoints = Mathf.Max(gryffindorPoints, slytherinPoints, ravenClawPoints, hufflePuffPoints);
                lowestPoints = Mathf.Min(gryffindorPoints, slytherinPoints, ravenClawPoints, hufflePuffPoints);

                //if no verbal answer has been made i.e. 100pts assigned
                if (highestPoints < 90)
                {
                    //add each house points to list to check if multiple equal the max
                    pointsList.Add(gryffindorPoints);
                    pointsList.Add(slytherinPoints);
                    pointsList.Add(ravenClawPoints);
                    pointsList.Add(hufflePuffPoints);

                    //do multiple of the house results also equal the highest number
                    foreach (int housePoint in pointsList)
                    {
                        if (housePoint == highestPoints)
                        {
                            highestPointsTester = highestPointsTester + 1;
                        }
                    }
                }
 
    
                //if any equal each other, bool is true, pass this code
                if (highestPointsTester < 2)
                {
                    if (highestPoints == gryffindorPoints)
                    {
                        gryffindorCrest.SetActive(true);
                        foreach (GameObject humanRobeObject in humanRobeObjects)
                        {
                            //get a copy of the materials array
                            Material[] matArray = humanRobeObject.GetComponent<Renderer>().materials;
                           
                            if (humanRobeObject.name == "YoungWitch")
                            {
                                //designate a change to the 3rd element in the material
                                matArray[3] = new Material(gryffindorMat);
                                //reinstantiate material array
                                humanRobeObject.GetComponent<Renderer>().materials = matArray;
                                //witchCurrentMat = new Material(gryffindorMat);
                            }
                            else
                            {
                                matArray[1] = new Material(gryffindorMat);
                                humanRobeObject.GetComponent<Renderer>().materials = matArray;
                            }
                        }
                    }
                    else if (highestPoints == slytherinPoints)
                    {
                        slytherinCrest.SetActive(true);
                        foreach (GameObject humanRobeObject in humanRobeObjects)
                        {
                            Material[] matArray = humanRobeObject.GetComponent<Renderer>().materials;

                            if (humanRobeObject.name == "YoungWitch")
                            {
                                matArray[3] = new Material(slytherinMat);
                                humanRobeObject.GetComponent<Renderer>().materials = matArray;
                            }
                            else
                            {
                                matArray[1] = new Material(slytherinMat);
                                humanRobeObject.GetComponent<Renderer>().materials = matArray;
                            }
                        }
                    }
                    else if (highestPoints == ravenClawPoints)
                    {
                        ravenClawCrest.SetActive(true);
                        foreach (GameObject humanRobeObject in humanRobeObjects)
                        {
                            Material[] matArray = humanRobeObject.GetComponent<Renderer>().materials;

                            if (humanRobeObject.name == "YoungWitch")
                            {
                                matArray[3] = new Material(ravenClawMat);
                                humanRobeObject.GetComponent<Renderer>().materials = matArray;
                            }
                            else
                            {
                                matArray[1] = new Material(ravenClawMat);
                                humanRobeObject.GetComponent<Renderer>().materials = matArray;
                            }
                        }
                    }
                    else if (highestPoints == hufflePuffPoints)
                    {
                        hufflePuffCrest.SetActive(true);
                        foreach (GameObject humanRobeObject in humanRobeObjects)
                        {
                            Material[] matArray = humanRobeObject.GetComponent<Renderer>().materials;

                            if (humanRobeObject.name == "YoungWitch")
                            {
                                matArray[3] = new Material(hufflePuffMat);
                                humanRobeObject.GetComponent<Renderer>().materials = matArray;
                            }
                            else
                            {
                                matArray[1] = new Material(hufflePuffMat);
                                humanRobeObject.GetComponent<Renderer>().materials = matArray;
                            }
                        }
                    }
                }
                //if multiple equal the same, determine inconclusive
                else if (highestPointsTester >= 2)
                {
                    resultInconclusive = true;
                    crowdCheer.Stop();
                }

            }

            //Do Speech recognition thing again here...maybe print a request?

            //if inconclusive, allow the user to respond with the house they prefer aka Harry moment (slyth vs gryf)
            if (resultInconclusive == true)
            {
                //collect new response
                hpSpeechRecognitionConclusive = GameObject.Find("SpeechRecognition").GetComponent<HPSpeechRecognitionEngine>();
                //conclusiveAnswer = hpSpeechRecognitionConclusive.conclusiveAnswer;

                if (writtenInconclusive == false)
                {
                    inconclusiveText.GetComponent<Text>().text = "Results Inconclusive. Speak which house you desire...";
                    writtenInconclusive = true;
                    
                }
                if (conclusiveAnswer == "Gryffindor")
                {
                    gryffindorPoints = 100;
                    resultInconclusive = false;
                    highestPointsTester = 0;
                    crestRecieved = false;
                    crowdCheer.Play();
                }
                if (conclusiveAnswer == "Slytherin")
                {
                    slytherinPoints = 100;
                    resultInconclusive = false;
                    highestPointsTester = 0;
                    crestRecieved = false;
                    crowdCheer.Play();
                }
                if (conclusiveAnswer == "Hufflepuff")
                {
                    hufflePuffPoints = 100;
                    resultInconclusive = false;
                    highestPointsTester = 0;
                    crestRecieved = false;
                    crowdCheer.Play();
                }
                if (conclusiveAnswer == "Ravenclaw")
                {
                    ravenClawPoints = 100;
                    resultInconclusive = false;
                    highestPointsTester = 0;
                    crestRecieved = false;
                    crowdCheer.Play();
                }
                //crestRecieved = false;
            }


            //    else if (objectUsed == "MerlinBook (UnityEngine.GameObject)")
            //    {
            //        slytherinCrest.SetActive(true);
            //    foreach (GameObject humanRobeObject in humanRobeObjects)
            //    {

            //        humanRobeObject.GetComponent<Renderer>().material = new Material(slytherinMat);
            //    }
            //}
            //else if (objectUsed == "StudentRecords (UnityEngine.GameObject)")
            //{
            //    ravenClawCrest.SetActive(true);
            //    foreach (GameObject humanRobeObject in humanRobeObjects)
            //    {

            //        humanRobeObject.GetComponent<Renderer>().material = new Material(ravenClawMat);
            //    }
            //}
            //else if (objectUsed == "DragonsBloodBottle (UnityEngine.GameObject)")
            //{
            //    hufflePuffCrest.SetActive(true);
            //    foreach (GameObject humanRobeObject in humanRobeObjects)
            //    {

            //        humanRobeObject.GetComponent<Renderer>().material = new Material(hufflePuffMat);
            //    }
            //}
        }
	}
    */

    void CollectResponses<Scene>(Scene scene)
    {
        //questionAnswer = hpSpeechRecognitionEngine.questionAnswer;

        //conclusiveAnswer = hpSpeechRecognitionEngine.conclusiveAnswer;
        //objectUsed = //connect the script here that collects which object was interacted with
    }

    private void OnDestroy()
    {
        OnResponseSelected -= StoreResponse;
        OnCheckAcceptableTags -= CheckAcceptableTags;
        OnToggleSceneSelectionResponse -= ToggleSceneSelectionResponse;
    }
}
