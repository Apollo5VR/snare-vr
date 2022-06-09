using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingState : ConsumableBaseState
{
    private float maxDuration = 5.0f;
    private float currentTime;

    public override void EnterState(ConsumableManager consumable)
    {
        consumable.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        currentTime = 0.0f;
    }

    public override void UpdateState(ConsumableManager consumable)
    {
        //play cooking sounds and vfx
        //TODO - what precautions / other conditions can we add to avoid infinite while loop potential?
        while(currentTime < maxDuration)
        {
            currentTime += Time.deltaTime;
        }

        //once done cooking switch state
        consumable.SwitchState(consumable.cookedState);
    }
}
