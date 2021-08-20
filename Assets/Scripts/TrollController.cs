
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class TrollController : MonoBehaviour {
    public bool testBool = false;

    public GameObject knight;
    public Animator optionalDoorAnimator;
    public Animator[] trollAnims; //2 - position, leg movement
    Vector3 liveTrollPosition;
    private float delayTime;
    private bool startDelayTime;

    public AudioSource[] audioOptions;

    //animations;
    public string trollFallAnimation;
    public string trollLegsAnimation;
    public Animator trollFallStateMachine;
    public Animator trollLegsStateMachine;

    // Use this for initialization
    void Start ()
    {
        ResponseCollector.Instance.OnResponseSelected += CommandTroll;
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

        switch ((int)response)
        {
            case 1:
                //Ravenclaw;
                //turn right
                TrollDistract();
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

        ProgressionController.Instance.OnLoadNextScene?.Invoke(8);
    }

    private IEnumerator ShootTroll()
    {
            audioOptions[1].Play();
            this.GetComponent<LineRenderer>().enabled = true;

            yield return new WaitForSeconds(3.5f);

            audioOptions[1].Stop();
            this.GetComponent<LineRenderer>().enabled = false;
    }

    private void DoorOpen()
    {
        if(optionalDoorAnimator != null)
        {
            optionalDoorAnimator.SetBool("doorOpenNowBool", true);
        }
    }

    private void TrollDistract()
    {
        this.transform.LookAt(knight.transform);
    }

    private void TrollDeath()
    {
        this.transform.localScale = new Vector3(0, 0, 0);
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
    }
}
