using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TurretProgressionController : MonoBehaviour
{
    public static TurretProgressionController Instance { get; private set; }
    public Transform dragonTransform;
    public Transform eggsTranform;

    public Text missionText;
    public Text instructionsText;
    public GameObject[] dragonEggs;

    public Func<Transform> OnGetDinoNest;
    public Action OnDecrementLP;
    public Action OnIncrementScore;
    public Action<CommonEnums.HouseResponses, GameObject> OnEggSceneResponseSelected;

    private BNG.PlayerTeleport playerTeleport;

    private int score = 0;
    private int winScore = 10; 
    private int lifePoints = 4;

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

    // Start is called before the first frame update
    void Start()
    {
        OnGetDinoNest += GetDinoNest;
        OnDecrementLP += UpdateLifePoints;
        OnIncrementScore += UpdateScore;
        OnEggSceneResponseSelected += EggSelected;

        instructionsText.enabled = false;
        playerTeleport = ProgressionController.Instance.OnRequestTeleporter();

        for(int i = 0; i < dragonEggs.Length; i++)
        {
            dragonEggs[i].SetActive(false);
        }

        StartCoroutine(MovePlayerToDragon());
    }

    private Transform GetDinoNest()
    {
        return eggsTranform;
    }

    private IEnumerator MovePlayerToDragon()
    {
        yield return new WaitForSeconds(10);

        missionText.enabled = false;
        instructionsText.enabled = true;

        StartCoroutine(playerTeleport.doTeleport(dragonTransform.localPosition, dragonTransform.localRotation, true));
    }

    private void UpdateLifePoints()
    {
        lifePoints--;

        if(lifePoints == 0)
        {
            StartCoroutine(MoveToDragonEggs(false));
        }
    }

    private void UpdateScore()
    {
        score++;

        if (score == winScore)
        {
            StartCoroutine(MoveToDragonEggs(true));
        }
    }

    private IEnumerator MoveToDragonEggs(bool isWin)
    {
        if(!isWin)
        {
            instructionsText.text = "Oh no, all the eggs have been eaten!";
            missionText.text = "I thank you for your help. But all that was left of my children was this chicken egg. Please take it, it pains me too much";
        }
        else
        {
            instructionsText.text = "You saved the eggs!";
            missionText.text = "I will never forget your great deeds. Here are my remaining children. In gratitude, please take one. I know you will raise it well!";
        }

        yield return new WaitForSeconds(8);

        StartCoroutine(playerTeleport.doTeleport(eggsTranform.localPosition, eggsTranform.localRotation, true));


        instructionsText.enabled = false;
        missionText.enabled = true;

        //if lose - activate white egg
        if (!isWin)
        {
            dragonEggs[dragonEggs.Length - 1].SetActive(true);
        }
        else
        {
            //if win
            for (int i = 0; i < dragonEggs.Length - 1; i++)
            {
                dragonEggs[i].SetActive(true);
            }
        }
    }

    //TODO - make this so its on grab, not on trigger - then update timer to be faster
    private void EggSelected(CommonEnums.HouseResponses response, GameObject eggObj)
    {
        //if win
        for (int i = 0; i < dragonEggs.Length - 1; i++)
        {
            if(eggObj != dragonEggs[i])
            {
                dragonEggs[i].SetActive(false);
            }
        }

        ProgressionController.Instance.OnLoadNextScene?.Invoke(25);
    }
}
