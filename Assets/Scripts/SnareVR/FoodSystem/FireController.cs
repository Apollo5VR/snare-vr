using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ScriptsConnector.Instance.OnConsume += Consume;
    }

    //TODO - update to cooked? or a separate function?
    public void Consume(GameObject consumable)
    {
        //TODO - refactor to be more realistic consumption pattern/process
        StartCoroutine(ConsumeDelay(2, consumable));
    }

    IEnumerator ConsumeDelay(float time, GameObject consumable)
    {
        yield return new WaitForSeconds(time);

        consumable.SetActive(false);

        //TODO - we shouldnt handle manipulating health here, and the variable should be determined by the consumable type
        float healthMod = 50; 

        ScriptsConnector.Instance?.OnModifyHealth("playerId", healthMod);

        ScriptsConnector.Instance.OnUpdateUI(CommonEnums.UIType.Generic, "EWW, YOU ATE A RAW RABBIT! BUT YOUR HEALTH INCREASED.");
    }

    private void OnDestroy()
    {

    }
}
