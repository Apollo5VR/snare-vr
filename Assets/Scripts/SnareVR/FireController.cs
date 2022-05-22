using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    //TODO - update to cooked? or a separate function?
    public void ConsumeRabbit(GameObject rabbit)
    {
        //TODO - refactor to be more realistic consumption pattern/process
        StartCoroutine(ConsumeDelay(2, rabbit));
    }

    IEnumerator ConsumeDelay(float time, GameObject rabbit)
    {
        yield return new WaitForSeconds(time);

        rabbit.SetActive(false);

        float healthMod = 5;
        float userCurrentHealth = ScriptsConnector.Instance.GetHealth("playerId");

        userCurrentHealth += healthMod;

        ScriptsConnector.Instance?.OnSetHealth("playerId", userCurrentHealth);

        ScriptsConnector.Instance.OnUpdateUI(CommonEnums.UIType.Generic, "EWW, YOU ATE A RAW RABBIT! BUT YOUR HEALTH INCREASED.");
    }

    private void OnDestroy()
    {

    }
}
