using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookedState : ConsumableBaseState
{
    public override void EnterState(ConsumableManager consumable)
    {
        consumable.gameObject.GetComponent<MeshRenderer>().material.color = Color.magenta;
        consumable.healthModifier *= 2;
    }

    public override void UpdateState(ConsumableManager consumable)
    {
        throw new System.NotImplementedException();
    }
}
