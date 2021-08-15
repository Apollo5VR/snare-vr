
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class TrollController : MonoBehaviour {
    public GameObject liveTroll;
    public GameObject deadTroll;
    public GameObject knight;
    public Animator optionalDoorAnimator;
    //GameObject trollClub;
    Vector3 liveTrollPosition;
    //public ResponseCollector responseCollector;
    //int nextLevel;
    float delayTime;
    bool startDelayTime;

    //animations;
    public string trollFallAnimation;
    public string trollLegsAnimation;
    public Animator trollFallStateMachine;
    public Animator trollLegsStateMachine;
    public static Action<CommonEnums.HouseResponses> OnResponseSelected;

    // Use this for initialization
    void Start () {
        deadTroll.SetActive(false);
        //trollClub = GameObject.Find("TrollClub");
        //responseCollector = GameObject.Find("ResponseCollector").GetComponent<ResponseCollector>();
        //nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        OnResponseSelected += CommandTroll;
    }

    private void CommandTroll(CommonEnums.HouseResponses response)
    {
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
                break;
            case 4:
                //Slytherin;
                //open door
                DoorOpen();
                break;
            default:
                response = CommonEnums.HouseResponses.None;
                break;
        }

        StartCoroutine(TimerToEndScene());
    }

    private IEnumerator TimerToEndScene()
    {
        yield return new WaitForSeconds(8);

        ProgressionController.OnLoadNextScene();
    }
	
	// Update is called once per frame
	void Update () {
        /*
        if (startDelayTime == true)
        {
            delayTime = delayTime + Time.deltaTime;
        }
        if (delayTime > 5)
        {
                SceneManager.LoadScene(nextLevel);
        }
        */
        //for TESTING
        //if (Time.time > 5 && Time.time < 6)
        //{
        //    liveTrollPosition = liveTroll.transform.position;
        //    //Instantiate Dead Troll
        //    deadTroll.transform.position = liveTrollPosition;
        //    deadTroll.SetActive(true);

        //    //unparent club & make use gravity

        //    //delete Live Troll
        //    Destroy(liveTroll);

        //    //unparent club & make use gravity




        //    //liveTrollPosition = liveTroll.transform.position;
        //    //trollLegsStateMachine.SetTrigger(trollLegsAnimation);
        //    //trollFallStateMachine.SetTrigger(trollFallAnimation);
        //}
    }

    //void OnTriggerEnter(Collider other)
    //{

        //trollDeath();

    //}

    private void DoorOpen()
    {
        if(optionalDoorAnimator != null)
        {
            optionalDoorAnimator.SetBool("doorOpenNowBool", true);
        }
    }

    private void TrollDistract()
    {
        liveTroll.transform.LookAt(knight.transform);
    }

    private void TrollDeath()
    {
        //responseCollector.objectUsed = "Troll";
        //startDelayTime = true;
        liveTrollPosition = liveTroll.transform.position;
        //Instantiate Dead Troll
        deadTroll.transform.position = liveTrollPosition;
        deadTroll.SetActive(true);

        //unparent club & make use gravity
        //trollClub.transform.SetParent(null);
        //trollClub.GetComponent<Rigidbody>().useGravity = true;

        //record response
        


        //delete Live Troll
        //Destroy(liveTroll);
        liveTroll.transform.localScale = new Vector3(0, 0, 0);

        //unparent club & make use gravity
    }
}
