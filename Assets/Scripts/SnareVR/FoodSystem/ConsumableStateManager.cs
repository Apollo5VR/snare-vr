using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableStateManager : MonoBehaviour
{
    public float healthModifier;

    //state management
    private ConsumableBaseState currentState;
    private RawState rawState = new RawState();
    private CookingState cookingState = new CookingState();
    private CookedState cookedState = new CookedState();

    private void Start()
    {
        currentState = rawState;

        currentState.EnterState(this);  
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    private void SwitchState(ConsumableBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    //this action comes from outside source (ie grab and place in stomach bag by the player)
    public void GetConsumed()
    {
        //TODO - modBoost for if we ever want to consider

        StartCoroutine(ConsumeDelay(2));
    }

    IEnumerator ConsumeDelay(float time)
    {
        yield return new WaitForSeconds(time);

        gameObject.SetActive(false);

        //TODO - we shouldnt handle manipulating health here, and the variable should be determined by the consumable type
        float healthMod = 5;

        ScriptsConnector.Instance?.OnModifyHealth("playerId", healthMod);

        ScriptsConnector.Instance.OnUpdateUI(CommonEnums.UIType.Generic, "EWW, YOU ATE A RAW RABBIT! BUT YOUR HEALTH INCREASED.");
    }
}
