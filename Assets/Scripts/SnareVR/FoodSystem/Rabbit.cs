using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO - requires access to FireController / FoodManager, need to refactor to make better
public class Rabbit : MonoBehaviour, IConsumable
{
    public void Consume()
    {
        ScriptsConnector.Instance?.OnConsume(gameObject);
    }
}
