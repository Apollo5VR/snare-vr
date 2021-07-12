﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProgressionController : MonoBehaviour {
    public int nextLevel;
    public GameObject player;
    public BNG.PlayerTeleport playerTeleport;
    private Collider playerCollider;
    //public DoorOpen doorOpen;
    //private float doorOpenTime2;
    public string pullCurrentScene;
    public string questionScene;
    public GameObject question1Audio;
    public GameObject question2Audio;
    public bool audio1HasPlayed;
    public bool audio2HasPlayed;
    public HPSpeechRecognitionEngine hpSpeechRecognitionEngine;
    public GameObject question1Text;
    public GameObject question2Text;
    public bool here;
    public AudioSource audioHat;
    float objectTime;
    bool objectTimeTicking;
    public float bottleAppear;
    public float bottleDisappear;
    public float scrollAppear;
    public float scrollDisappear;
    public float bookAppear;
    public float bookDisappear;
    public GameObject bottle;
    public GameObject scroll;
    public GameObject book;
    public GameObject wordResponseObjects;
    public bool petPlaced = false;
    public GameObject sceneLoadingBlackSphere;
    public bool debugProgressNextScene;
    public bool testMovePlayer;

// Use this for initialization
    private void Start() {

        //playerController = player.FindComponentInChildWithTag<Transform>("Player").gameObject;
        SceneManager.sceneLoaded += OnSceneLoaded;
        //SceneManager.sceneUnloaded += OnSceneLoadedUnloaded;

        playerCollider = player.GetComponent<Collider>();
        //doorOpenTime2 = doorOpen.doorOpenTime;
        pullCurrentScene = SceneManager.GetActiveScene().name;
        nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        //audioHat = question1Audio.GetComponent<AudioSource>();

        //RESET THESE LIVE!
        //question1Text.SetActive(false);
        //question2Text.SetActive(false);

        //deactivate all objects (till time to appear)
        bottle.SetActive(false);
        scroll.SetActive(false);
        book.SetActive(false);

        //SHIT TO DELETE
        //for (int i = 0; i < totalScenes; i++)
        //{
        //    //sceneNumber.Add("tube");

        //    sceneNumberTest = "Scene " + i + ":" + SceneManager.GetSceneByBuildIndex(i).ToString();

        //    //sceneNumber.Add(i);
        //}
        //ENABLE IF #1- want delay in firework launch
        //for (int i = 0; i < 2; i++)
        //{
        //    fireWorks[i].SetActive(false);
        //}

    }

    //// Update is called once per frame
    //void Update() {

    //    //track time for object vizualization
    //    if (objectTimeTicking)
    //    {
    //        objectTime += Time.deltaTime;
    //        //Debug.Log(objectTime);
    //        if (objectTime > bottleAppear && objectTime < bottleDisappear)
    //        {
    //            wordResponseObjects.SetActive(false);
    //            bottle.SetActive(true);
    //        }
    //        else if (objectTime > scrollAppear && objectTime < scrollDisappear)
    //        {
    //            bottle.SetActive(false);
    //            scroll.SetActive(true);
    //        }
    //        else if (objectTime > bookAppear && objectTime < bookDisappear)
    //        {
    //            scroll.SetActive(false);
    //            book.SetActive(true);
    //        }
    //    }
    //    //ENABLE IF #2- want delay in firework launch
    //    //if (Time.time > doorOpenTime2 && Time.time < (doorOpenTime2 + 10))
    //    //{
    //    //    for (int i = 0; i < 2; i++)
    //    //    {
    //    //        fireWorks[i].SetActive(true);
    //    //    }
    //    //}
    //    if (audio2HasPlayed == true)
    //    {
    //        //audio2.Play();
    //    }
    //    //play audio

    //    if (pullCurrentScene == questionScene)
    //    {

    //        //THE ORIGINAL SCRIPT, Reactivate and FIX!
    //        if (!audio1HasPlayed)// && nextLevel == 4) //&& Time.time < 2.5
    //        {

    //            audioHat.Play();
    //            audio1HasPlayed = true;
    //            question1Text.SetActive(true);

    //        }

    //        if (!audio2HasPlayed && !audioHat.isPlaying)//&& !audio1.isPlaying //&& hpSpeechRecognitionEngine.questionAnswer != "notReady" 
    //        {
    //            //Start object time count to correctly activate object vizuals
    //            objectTimeTicking = true;

    //            audioHat = question2Audio.GetComponent<AudioSource>();
    //            audioHat.Play();
    //            audio2HasPlayed = true;
    //            question2Text.SetActive(true);
    //            here = true;

    //        }


    //        //12.11 USE THIS!
    //        if (audio2HasPlayed == true && !audioHat.isPlaying)
    //        {
    //            StartCoroutine(WaitTillRead());

    //        }
    //        //if (audio2HasPlayed == true)//&& audio2.isPlaying
    //        //{

    //        //    StartCoroutine(WaitTillRead());
    //        //    audio2HasPlayed = false;
    //        //}


    //    }
    //}

    //}
    public void Update()
    {
        if (debugProgressNextScene)
        {
            debugProgressNextScene = false;
            SceneManager.LoadScene(nextLevel);
        }
    }

    public void LoadNextScene()
    {   
        //depreciated
        //sceneLoadingBlackSphere.SetActive(true);

        SceneManager.LoadScene(nextLevel);
    }

    public void LoadChallengeScene(int sceneIncrement)
    {
        //depreciated
        //sceneLoadingBlackSphere.SetActive(true);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + sceneIncrement);
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
        //SceneManager.sceneUnloaded -= OnSceneLoadedUnloaded;
    }

    //TODO - connect this to some type of switchboard so we dont have to keep doing GetComponent, or figure out Instance
    public ProgressionController GetProgressionController()
    {
        return this;
    }
}

