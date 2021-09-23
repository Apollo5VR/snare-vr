using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShellSelectionManager : MonoBehaviour
{
    public GameObject[] shells; //todo remove might not need
    public GameObject shellSelected;
    public GameObject shellOnBall;
    public int playState; //0, 1, 2, 3 (1 is currently playing, 3 is ended)

    public static Action<GameObject> OnShellSelected;
    public static Action<GameObject> OnBallStopped;

    private float minimum;
    private float maximum;
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
            UpdateShellSelected(shellSelected);
            //StartCoroutine(LiftShell(shellSelected));
            testBool = false;
        }
    }

    //Observer in an Observer Pattern
    private void UpdateShellSelected(GameObject shell)
    {
        //1 is playing, no action allowed
        if(playState == 0)
        {
            //start game
            StartCoroutine(LiftShell(shellSelected));
            playState = 1;
        }
        else if(playState == 2)
        {
            shellSelected = shell;
            StartCoroutine(LiftShell(shellSelected));
            BallGameController.OnShellSelectionGuessed?.Invoke();
        }
    }

    private IEnumerator LiftShell(GameObject shell)
    {
        bool liftCup = true;
        Collider shellCollider = shell.GetComponent<Collider>();
        shellCollider.enabled = false;
        Vector3 originalPosition = shell.transform.position;
        maximum = originalPosition.y + 1.0f;
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
                //playState = 0; //resets the play state to 0 to play again (TODO - remove? game over over?)
                shell.transform.position = originalPosition;
            }

            yield return null;
        }

        shellCollider.enabled = true;

        //to show them the ball before game starts
        if (playState == 1)
        {
            BallGameController.OnBallGameStarted?.Invoke();
        }
    }

    private void UpdateBallLocation(GameObject shell)
    {
        shellOnBall = shell;
    }

    public void OnDestroy()
    {
        OnShellSelected -= UpdateShellSelected;
        OnBallStopped -= UpdateBallLocation;
    }
}
