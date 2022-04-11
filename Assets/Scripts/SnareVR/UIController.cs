using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text healthText; //TODO need to update for multiplayer
    public Text trapCaughtTimerText;

    public float timeRemaining = 10;
    public bool timerIsRunning = false;


    // Start is called before the first frame update
    void Start()
    {
        healthText.text = "YOUR HEALTH SHOULD SHOW HERE";
        trapCaughtTimerText.text = "TRAP INFO WILL SHOW HERE";

        ScriptsConnector.Instance.OnUpdateUI += UpdateUI;

        healthText.text = ScriptsConnector.Instance?.GetHealth.Invoke("playerID").ToString() + "% Health";
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                trapCaughtTimerText.text = timeRemaining.ToString();
            }
            else
            {
                Debug.Log("Did you catch something?");
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    private void UpdateUI(string type, string updatedValue)
    {
        Debug.Log("type of " + type + "with value: " + updatedValue);

        switch (type)
        {
            case "caughtTime":
                timeRemaining = float.Parse(updatedValue);
                timerIsRunning = true;
                break;
            case "caughtResult":
                trapCaughtTimerText.text = updatedValue;
                break;
            case "healthUpdate":
                healthText.text = updatedValue;
                break;
            default:
                break;
        }
    }
}
