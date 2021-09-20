using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShellSelectionManager : MonoBehaviour
{
    public GameObject[] shells; //todo remove might not need
    public GameObject shellSelected;
    public GameObject shellOnBall;

    public static Action<GameObject> OnShellSelected;
    public static Action<GameObject> OnBallStopped;

    private float minimum = -2.0f;
    private float maximum = -1.5f;
    // starting value for the Lerp
    static float t = 0.0f;

    public bool testBool;

    public void Start()
    {
        OnShellSelected += UpdateShellSelected;
        OnBallStopped += UpdateBallLocation;
    }

    //TODO - remove
    public void Update()
    {
        if (testBool)
        {
            StartCoroutine(LiftShell(shellSelected));
            testBool = false;
        }
    }

    //Observer in an Observer Pattern
    private void UpdateShellSelected(GameObject shell)
    {
        shellSelected = shell;
        StartCoroutine(LiftShell(shellSelected));
    }

    private IEnumerator LiftShell(GameObject shell)
    {
        bool liftCup = true;
        Vector3 originalPosition = shell.transform.position;
        maximum = originalPosition.y + 0.75f;
        minimum = originalPosition.y;

        while (liftCup)
        {
            shell.transform.position = new Vector3(shell.transform.position.x, Mathf.Lerp(minimum, maximum, t), shell.transform.position.z);

            // .. and increase the t interpolater
            t += 1.5f * Time.deltaTime;

            // now check if the interpolator has reached 1.0
            // and swap maximum and minimum so game object moves
            // in the opposite direction.
            if (t > 1.0f)
            {
                float temp = maximum;
                maximum = minimum;
                minimum = temp;
                t = 0.0f;

                liftCup = false;
                shell.transform.position = originalPosition;
            }

            yield return null;
        }
    }

    private void UpdateBallLocation(GameObject shell)
    {
        shellOnBall = shell;
    }

    public void OnDestroy()
    {
        OnShellSelected -= UpdateShellSelected;
        OnBallStopped += UpdateBallLocation;
    }
}
