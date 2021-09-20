using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameObject previousCupFirst;

    public int firstCup;
    public int secondCup;
    public int rotationPoint;
    private int[,] presetCombinations = new int[,] {  { 0, 1, 0 }, { 0, 2, 1 }, { 1, 2, 2 } };
    private Vector3[,] resetPositions = new Vector3[,] { { new Vector3(0,0,0), new Vector3(0, 0, 0), new Vector3(0, 0, 0) } };
    //new int[1, 3] { { 0, 2, 1 } };

    public bool testBool;

    Vector3 startPosition;

    //TODO cant have the ball parented, moving on its own logic
    private void Start()
    {
        startPosition = cups[0].transform.position;
        //resetPositions[0, 0] = cups[0].transform.position;
        //resetPositions[0, 1] = cups[1].transform.position;
        //resetPositions[0, 2] = cups[2].transform.position;

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
            halfRotations = 0;
            startCounter = 0;

            int randomPresetSelection = UnityEngine.Random.Range(0, 3);

            firstCup = presetCombinations[randomPresetSelection, 0];
            secondCup = presetCombinations[randomPresetSelection, 1];
            rotationPoint = presetCombinations[randomPresetSelection, 2];

            StartCoroutine(RotatePairs(firstCup, secondCup, rotationPoint)); //note: param 1 can never be same as param 3
            testBool = false;
        }
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

        //to relocate the shells after each iteration so that 1, 2, 3 order stays the same on start of next iteration (necessary)
        for (int i = 0; i < cups.Length; i++)
        {
            cups[i].transform.position = resetPositions[0, i];
        }       
    }
}
