using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookedState : ConsumableBaseState
{
    public override void EnterState(ConsumableStateManager consumable)
    {
        consumable.healthModifier *= 2;
    }

    public override void UpdateState(ConsumableStateManager consumable)
    {
        throw new System.NotImplementedException();
    }
}
