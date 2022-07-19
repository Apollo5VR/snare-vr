using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text healthText; //TODO need to update for multiplayer
    public Text trapCaughtTimerText;
    public Text genericText;

    private float timeRemaining = -1.0f;
    //public bool timerIsRunning = false;


    // Start is called before the first frame update
    void Start()
    {
        healthText.text = "YOUR HEALTH SHOULD SHOW HERE";
        trapCaughtTimerText.text = "TRAP INFO WILL SHOW HERE";

        ScriptsConnector.Instance.OnUpdateUI += UpdateUI;
    }

    private void UpdateUI(CommonEnums.UIType type, string updatedValue)
    {
        Debug.Log("type of " + type.ToString() + " with value: " + updatedValue);

        switch (type)
        {
            //TODO relocate to univesal ui - time
            case CommonEnums.UIType.Time:
                timeRemaining = float.Parse(updatedValue);
                UpdateTimer();
                //timerIsRunning = true;
                break;
            //relocate to univesal ui - generic
            case CommonEnums.UIType.Generic:
                genericText.text = updatedValue;
                break;
            //relocate to univesal ui - health
            case CommonEnums.UIType.Health:
                healthText.text = "HEALTH:" + updatedValue;
                break;
            case CommonEnums.UIType.None:
                break;
            default:
                break;
        }
    }

    private void UpdateTimer()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime * 1000; //equivalent of -1 sec

            //convert time remaining to day / hour / minute format
            TimeSpan t = TimeSpan.FromMilliseconds(timeRemaining);
            //DateTime expirationDate = new DateTime(t.Ticks);
            trapCaughtTimerText.text = "Time: " + t.Days + "D " + t.Hours + "H " + t.Minutes + "M"; //expirationDate.ToString("dd:hh:mm");
        }
        else
        {
            if(timeRemaining == -1)
            {
                trapCaughtTimerText.text = "Time: No Trap Set";
            }
            else
            {
                timeRemaining = 0;
                trapCaughtTimerText.text = "Time: Trap Ready";
            }

            //TODO - relocated this to specific scenes or somewhere else? to TrapController, make TrapController Universal
            //ScriptsConnector.Instance.OnCheckTrap?.Invoke();
        }

        //TODO - what about if there is no trap?
    }
}
