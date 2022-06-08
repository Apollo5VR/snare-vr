using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConsumableBaseState
{
    public abstract void EnterState(ConsumableStateManager consumable);
    public abstract void UpdateState(ConsumableStateManager consumable); 
}
