
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Unity.Services.Analytics;

public class TrollController : MonoBehaviour {
    public static TrollController Instance { get; private set; }
    public bool testBool = false;

    public GameObject explosion;
    public GameObject trollBody;
    public GameObject knight;
    public Animator optionalDoorAnimator;
    public Animator[] trollAnims; //2 - position, leg movement

    public AudioSource[] audioOptions;

    public Action<CommonEnums.HouseResponses> OnTrollSceneResponseSelected;

    //depreciated
    //private Vector3 liveTrollPosition;
    //private float delayTime;
    //private bool startDelayTime;

    //depreciated animations;
    //public string trollFallAnimation;
    //public string trollLegsAnimation;
    //public Animator trollFallStateMachine;
    //public Animator trollLegsStateMachine;

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
    void Start ()
    {
        OnTrollSceneResponseSelected += CommandTroll;

        StartCoroutine(TimerTillTrollKills());
    }

    private IEnumerator TimerTillTrollKills()
    {
        //15s till the troll reaches you
        yield return new WaitForSeconds(28);

        //note: deactivating this as no response is super discouraging in initial testing (not funny)
        //ResponseCollector.Instance.OnResponseSelected?.Invoke(CommonEnums.HouseResponses.None);

        ProgressionController.Instance.OnLoadNextScene?.Invoke(0);
    }

    public void Update()
    {
        if (testBool)
        {
            StartCoroutine(ShootTroll());
            testBool = false;
        }
    }

    private void CommandTroll(CommonEnums.HouseResponses response)
    {
        DisableTroll();

        //will usually happen only 1x, but can happen more if you "change your mind" last second (part of the test)
        ResponseCollector.Instance.OnResponseSelected?.Invoke(response);

        if (!Application.isEditor)
        {
            //Analytics Beta
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "specificQuestion", "Troll" },
                { "houseIndex", (int)response },
            };
            Events.CustomData("questionResponse", parameters);
        }

        switch ((int)response)
        {
            case 1:
                //Ravenclaw;
                //turn right
                StartCoroutine(TrollDistractRotation());
                break;
            case 2:
                //Gryfindor;
                //die
                TrollDeath();
                break;
            case 3:
                //Hufflepuff;
                //shoot stun from friend
                StartCoroutine(ShootTroll());
                break;
            case 4:
                //Slytherin;
                //open door
                DoorOpen();
                break;
            default:
                break;
        }

        ProgressionController.Instance.OnLoadNextScene?.Invoke(6);
    }

    private IEnumerator ShootTroll()
    {
            audioOptions[1].Play();
            this.GetComponent<LineRenderer>().enabled = true;

            yield return new WaitForSeconds(3.5f);

            audioOptions[1].Stop();
            this.GetComponent<LineRenderer>().enabled = false;

            TrollDeath();
    }

    private void DoorOpen()
    {
        if(optionalDoorAnimator != null)
        {
            optionalDoorAnimator.SetBool("doorOpenNowBool", true);
        }
    }

    //depreciated
    private void TrollDistract()
    {
        transform.LookAt(knight.transform);

    }

    private IEnumerator TrollDistractRotation()
    {
        while(true)
        {
            Vector3 direction = knight.transform.position - transform.position;
            direction.y = 0;
            Quaternion toRotation = Quaternion.LookRotation(direction);

            yield return null;

            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 0.001f * Time.time);
        }
    }

    private void TrollDeath()
    {
        trollBody.SetActive(false);

        //vfx explosion
        explosion.SetActive(true);
    }

    private void DisableTroll()
    {
        //sfx
        audioOptions[0].Stop();

        //anims
        foreach (Animator trollanim in trollAnims)
        {
            trollanim.enabled = false;
        }

        ResponseCollector.Instance.OnResponseSelected -= CommandTroll;
    }

    private void OnDestroy()
    {
        ResponseCollector.Instance.OnResponseSelected -= CommandTroll;

        StopAllCoroutines();
    }
}
