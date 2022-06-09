using System.Collections;
using UnityEngine;

public class ConsumableManager : MonoBehaviour
{
    public float healthModifier;

    private MeshRenderer meshRenderer;

    //state machine management
    public ConsumableBaseState currentState;
    public RawState rawState = new RawState();
    public CookingState cookingState = new CookingState();
    public CookedState cookedState = new CookedState();

    private void Start()
    {
        currentState = rawState;

        currentState.EnterState(this);  
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(ConsumableBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    //this action comes from outside source (ie grab and place in stomach bag by the player)
    public void GetConsumed()
    {
        StartCoroutine(ConsumeDelay(2));
    }

    IEnumerator ConsumeDelay(float time)
    {
        yield return new WaitForSeconds(time);

        gameObject.SetActive(false);

        ScriptsConnector.Instance?.OnModifyHealth("playerId", healthModifier);

        ScriptsConnector.Instance.OnUpdateUI(CommonEnums.UIType.Generic, "EWW, YOU ATE A RAW RABBIT! BUT YOUR HEALTH INCREASED.");
    }
}
