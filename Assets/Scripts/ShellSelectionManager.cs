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

    public void Start()
    {
        OnShellSelected += UpdateShellSelected;
        OnBallStopped += UpdateBallLocation;
    }

    //Observer in an Observer Pattern
    private void UpdateShellSelected(GameObject shell)
    {
         shellSelected = shell;
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
