using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
//using Unity.Services.Analytics;

//TODO - remove depreciated logic from last prototype
public class ProgressionController : MonoBehaviour
{
    public static ProgressionController Instance { get; private set; }
    public BNG.PlayerTeleport playerTeleport;
    public GameObject sceneLoadingBlackSphere;
    public bool isManualSelection = false;
    public bool debugProgressNextScene;
    public bool testNewScene = false;

    public Action<float> OnLoadNextScene;
    public Action<int> OnLoadSelectedScene;
    public Func<BNG.PlayerTeleport> OnRequestTeleporter;

    private int nextLevel;
    private IEnumerator autoProgressCR;

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

    private BNG.PlayerTeleport PassTeleporter()
    {
        return playerTeleport;
    }
    

    // Use this for initialization
    private void Start()
    {
        //subs
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnLoadNextScene += LoadNextScene;
        OnLoadSelectedScene += LoadSpecificScene;
        OnRequestTeleporter += PassTeleporter;
        ScriptsConnector.Instance.OnGetCurrentScene += GetCurrentScene;

        nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
    }

    //testing only
    #if UNITY_EDITOR
    public void Update()
    {
        if (debugProgressNextScene)
        {
            debugProgressNextScene = false;
            SceneManager.LoadScene(nextLevel);
        }
    }
    #endif

    private int GetCurrentScene()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    private void LoadNextScene(float time)
    {
        //readded 7.28
        sceneLoadingBlackSphere.SetActive(true);

        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        int level = 0;

        //for testing only
        if(testNewScene)
        {
            level = 1; //update this number to the test scene
            StartCoroutine(TimerToEndScene(level, time));
            return;
        }
        else
        {
            level = nextLevel;
        }

        StartCoroutine(TimerToEndScene(level, time));
    }

    public void LoadSpecificScene(int sceneInt = 1)
    {
        //TODO - refactor to one location (so we only need 1 script to have Analytics dependency)
        /*
        //TODO - update this for Snare VR
        if (!Application.isEditor)
        {
            //Analytics Beta
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                //1.17 currently sceneIncrement 1 - 6
                { "buildIndex", questionSelectionScene + sceneInt}
            };
            AnalyticsService.Instance.CustomData("sceneload_replay", parameters);
        }
        */
        //readded 7.28
        sceneLoadingBlackSphere.SetActive(true);

        SceneManager.LoadScene(sceneInt);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopAllCoroutines();

        nextLevel = SceneManager.GetActiveScene().buildIndex + 1;

        Transform startingPosition = GameObject.Find("StartingPosition").transform;

        if (startingPosition != null)
        {
            StartCoroutine(playerTeleport.doTeleport(startingPosition.localPosition, startingPosition.localRotation, true, false));
        }
        else
        {
            Debug.LogError("Player starting position not found");
        }

        //it trap points exist in scene, then place and prepare prefab
        ScriptsConnector.Instance.OnPrepareTrap?.Invoke();

        startingPosition = null;

        //readded 7.28
        sceneLoadingBlackSphere.SetActive(false);
    }

    private IEnumerator TimerToEndScene(int sceneBuildIndex, float time)
    {
        //TODO - refactor to one location (so we only need 1 script to have Analytics dependency)
        /*
        if (!Application.isEditor)
        {
            //Analytics Beta
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "buildIndex", sceneBuildIndex }
            };
            AnalyticsService.Instance.CustomData("sceneload", parameters);
        }
        */

        yield return new WaitForSeconds(time);

        SceneManager.LoadScene(sceneBuildIndex);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        OnLoadNextScene -= LoadNextScene;
        OnLoadSelectedScene -= LoadSpecificScene;

        if(ScriptsConnector.Instance != null)
        {
            ScriptsConnector.Instance.OnGetCurrentScene -= GetCurrentScene;
        }
    }
}

