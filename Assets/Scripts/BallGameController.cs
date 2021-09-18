using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGameController : MonoBehaviour
{
    //Assign a GameObject in the Inspector to rotate around
    public GameObject[] rotationPoints;
    public GameObject[] cups;
    public float speed;
    public int randomizer;
    public float previousAngle = 180;
    public float currentAngle;
    public int halfRotations;
    public float halfRotationsMax = 2; //note: can never equal 0/1
    public int startCounter = 0;

    public bool testBool;

    Vector3 startPosition;

    private void Start()
    {
        startPosition = cups[0].transform.position;
    }

    void Update()
    {
        if (testBool)
        {
            halfRotations = 0;
            startCounter = 0;
            StartCoroutine(RotatePairs(2, 0, 1)); //note: param 1 can never be same as param 3
            testBool = false;
        }
    }

    private IEnumerator RotatePairs(int first, int second, int rotPoint)
    {
        //TODO - add randomizer for selecting cubes and direction?

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
    }
}
