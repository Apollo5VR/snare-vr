using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BallGameController : MonoBehaviour
{
    //Assign a GameObject in the Inspector to rotate around
    public ShellSelectionManager shellSelectionManager;
    public GameObject[] rotationPoints;
    public GameObject[] cups;
    public GameObject ball;
    public float speed;
    public int randomizer;
    public float previousAngle = 180;
    public float currentAngle;
    public int halfRotations;
    public float halfRotationsMax = 2; //note: can never equal 0/1
    public int startCounter = 0;
    public Text instructionResults;

    public static Action OnBallGameStarted;
    public static Action OnShellSelectionGuessed;

    public GameObject previousCupFirst;

    public int firstCup;
    public int secondCup;
    public int rotationPoint;
    private int[,] presetCombinations = new int[,] {  { 0, 1, 0 }, { 0, 2, 1 }, { 1, 2, 2 } };
    private Vector3[,] resetPositions = new Vector3[,] { { new Vector3(0,0,0), new Vector3(0, 0, 0), new Vector3(0, 0, 0) } };

    public bool testBool;

    Vector3 startPosition;

    private void Start()
    {
        OnBallGameStarted += StartBallGame;
        OnShellSelectionGuessed += DetermineResult;

        startPosition = cups[0].transform.position;

        ball.transform.position = cups[1].transform.position;

        for (int i = 0; i < cups.Length; i++)
        {
            resetPositions[0, i] = cups[i].transform.position;
        }
    }

    void Update()
    {
        if (testBool)
        {
            StartCoroutine(LoopingRotate()); 
            testBool = false;
        }
    }

    private void StartBallGame()
    {
        StartCoroutine(LoopingRotate());
    }

    private void DetermineResult()
    {
        if (shellSelectionManager.shellSelected == shellSelectionManager.shellOnBall)
        {
            //win
            instructionResults.text = "YOU WIN!";
            //activate text
            //play music
            //progress to next scene in x seconds
            ProgressionController.Instance.OnLoadNextScene?.Invoke(8);
        }
        else
        {
            //try again
            instructionResults.text = "GUESS AGAIN!";
            //activate text
            //play error
        }
    }

    private IEnumerator LoopingRotate()
    {
        shellSelectionManager.playState = 1;

        for (int i = 10; i > 0; i--)
        {
            halfRotations = 0;
            startCounter = 0;

            int randomPresetSelection = UnityEngine.Random.Range(0, 3);

            firstCup = presetCombinations[randomPresetSelection, 0];
            secondCup = presetCombinations[randomPresetSelection, 1];
            rotationPoint = presetCombinations[randomPresetSelection, 2];

            StartCoroutine(RotatePairs(firstCup, secondCup, rotationPoint)); //note: param 1 can never be same as param 3

            yield return new WaitForSeconds(1);
        }

        instructionResults.text = "TAP CUBE TO GUESS BALL LOCATION";
        shellSelectionManager.playState = 2;
    }

    private IEnumerator RotatePairs(int first, int second, int rotPoint)
    {
        //to keep the ball accurately position underneath the shell even if the shell rotation / position is slightly offset
        if (shellSelectionManager.shellOnBall == cups[first])
        {
            ball.transform.position = cups[first].transform.position;
            ball.transform.SetParent(cups[first].transform);
        }
        else if (shellSelectionManager.shellOnBall == cups[second])
        {
            ball.transform.position = cups[second].transform.position;
            ball.transform.SetParent(cups[second].transform);
        }

        while (halfRotations < halfRotationsMax)
        {
            if (startCounter > 0)
            {
                previousAngle = currentAngle;
            }

            currentAngle = Vector3.Angle(rotationPoints[rotPoint].transform.position - startPosition,
                                            rotationPoints[rotPoint].transform.position - cups[first].transform.position);

            cups[first].transform.RotateAround(rotationPoints[rotPoint].transform.position, Vector3.up, speed * Time.deltaTime);
            cups[second].transform.RotateAround(rotationPoints[rotPoint].transform.position, Vector3.up, speed * Time.deltaTime);

            if ((halfRotations % 2 == 0))
            {
                if (currentAngle > previousAngle)
                {
                    halfRotations++;
                }
            }
            else
            {
                if(startCounter > 0)
                {
                    if (previousAngle > currentAngle)
                    {
                        halfRotations++;
                    }
                }
            }

            startCounter++;

            yield return null;
        }

        ball.transform.SetParent(this.gameObject.transform);

        //to relocate the shells after each iteration so that 1, 2, 3 order stays the same on start of next iteration (necessary)
        for (int i = 0; i < cups.Length; i++)
        {
            cups[i].transform.position = resetPositions[0, i];
        }       
    }

    private void OnDestroy()
    {
        OnBallGameStarted -= StartBallGame;
        OnShellSelectionGuessed -= DetermineResult;
    }
}
