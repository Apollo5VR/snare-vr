using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Unity.Services.Analytics;

public class ProgressionController : MonoBehaviour
{
    public static ProgressionController Instance { get; private set; }
    public BNG.PlayerTeleport playerTeleport;
    public GameObject sceneLoadingBlackSphere;
    public bool isManualSelection = false;
    public bool debugProgressNextScene;

    public Action<float> OnLoadNextScene;
    public Action<int> OnLoadChallengeScene;

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
    

    // Use this for initialization
    private void Start()
    {
        //subs
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnLoadNextScene += LoadNextScene;
        OnLoadChallengeScene += LoadQuestionScene;

        nextLevel = SceneManager.GetActiveScene().buildIndex + 1;

        //also called in below
        autoProgressCR = TimerToEndScene(nextLevel, 300);
        StartCoroutine(autoProgressCR);
    }

    //testing only
    public void Update()
    {
        if (debugProgressNextScene)
        {
            debugProgressNextScene = false;
            SceneManager.LoadScene(nextLevel);
        }
    }

    private void LoadNextScene(float time)
    {
        //depreciated
        //sceneLoadingBlackSphere.SetActive(true);
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        int level = 0;

        //note: manual selection added for 1st go through all scenes, then choice of 1 replay
        if (isManualSelection && (buildIndex > 4 && buildIndex < 9))
        {
            level = 9;
        }
        //for first time picking up hat
        else if(buildIndex == 3)
        {
            time = 0;
            level = 5;
        }
        else
        {
            level = nextLevel;
        }

        StartCoroutine(TimerToEndScene(level, time));
    }

    public void LoadQuestionScene(int sceneIncrement = 0)
    {
        //depreciated
        //sceneLoadingBlackSphere.SetActive(true);

        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        if(buildIndex == 9)
        {
            isManualSelection = true;
        }

        if (!Application.isEditor)
        {
            //Analytics Beta
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "buildIndex", 4 + sceneIncrement }
            };
            Events.CustomData("sceneload_replay", parameters);
        }

        //4 is the challenge selection scene
        //TODO - find way to not need hard coded value
        SceneManager.LoadScene(4 + sceneIncrement);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopAllCoroutines();

        nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        if(nextLevel < 6)
        {
            autoProgressCR = TimerToEndScene(nextLevel, 210);
            StartCoroutine(autoProgressCR);
        }

        //TODO - find spawn position, relocate player
        Transform startingPosition = GameObject.Find("StartingPosition").transform;

        if (startingPosition != null)
        {
            Debug.Log("OnSceneLoaded: " + scene.name);
            StartCoroutine(playerTeleport.doTeleport(startingPosition.localPosition, startingPosition.localRotation, true));
        }
        else
        {
            Debug.Log("Player starting position not found");
        }

        startingPosition = null;
    }

    private IEnumerator WaitTillRead()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(nextLevel);
    }

    public void DelayedSceneProgression(int sceneBuildIndex, float time)
    {
        StartCoroutine(TimerToEndScene(sceneBuildIndex, time));
    }

    private IEnumerator TimerToEndScene(int sceneBuildIndex, float time)
    {
        //if a user triggers end scene by completing all actions, stop the auto progress
        if(time < 30)
        {
            if(autoProgressCR != null)
            {
                StopCoroutine(autoProgressCR);
            }
        }

        if (!Application.isEditor)
        {
            //Analytics Beta
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "buildIndex", sceneBuildIndex }
            };
            Events.CustomData("sceneload", parameters);
        }

        yield return new WaitForSeconds(time);

        SceneManager.LoadScene(sceneBuildIndex);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        OnLoadNextScene -= LoadNextScene;
        OnLoadChallengeScene -= LoadQuestionScene;
    }
}

