using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonEnums : MonoBehaviour
{
    public static CommonEnums Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public enum AvailableSpells
    {
        None = -1,
        Accio = 0, //Distance grab 
        Stupify = 1, //Paralyze 
        WingardiumLeviosa = 2, //Float 
        Incendio = 3 //Burn
    }

    public enum HouseResponses
    {
        None = 0,
        Ravenclaw = 1,
        Gryfindor = 2, 
        Hufflepuff = 3, 
        Slytherin = 4 
    }
}
