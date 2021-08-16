using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class ProgressionController : MonoBehaviour
{
    public static ProgressionController Instance { get; private set; }
    public BNG.PlayerTeleport playerTeleport;
    public GameObject sceneLoadingBlackSphere;
    public bool debugProgressNextScene;
    public bool testMovePlayer;

    public Action OnLoadNextScene;
    public Action<int> OnLoadChallengeScene;

    private int nextLevel;

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
        OnLoadChallengeScene += LoadSpecifiedScene;

        nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
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

    private void LoadNextScene()
    {   
        //depreciated
        //sceneLoadingBlackSphere.SetActive(true);

        if(SceneManager.GetActiveScene().buildIndex > 5)
        {
            SceneManager.LoadScene(8); //results scene
        }

        SceneManager.LoadScene(nextLevel);

    }

    public void LoadSpecifiedScene(int sceneIncrement = 0)
    {
        //depreciated
        //sceneLoadingBlackSphere.SetActive(true);

        //3 is the challenge selection scene
        //TODO - find way to not need hard coded value
        SceneManager.LoadScene(3 + sceneIncrement);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        Debug.Log(mode);

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

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        OnLoadNextScene -= LoadNextScene;
        OnLoadChallengeScene -= LoadSpecifiedScene;
    }
}

