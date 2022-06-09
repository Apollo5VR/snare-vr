using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConsumableBaseState
{
    public abstract void EnterState(ConsumableManager consumable);
    public abstract void UpdateState(ConsumableManager consumable); 
}
