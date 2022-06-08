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

    //TODO remove these depreciated enums once non-snare scripts eradicated
    public enum AvailableSpells
    {
        None = -1,
        Accio = 0, //Distance grab 
        Baubil = 1, //Lightning
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

    public enum SnareState
    {   None,
        Build,
        Check,
        Unavailable
    }

    public enum UIType
    {
        None,
        Health,
        Time,
        Generic
    }

    //TODO - depreciated - use replaced by ConsumableStateManager
    public enum ConsumableState
    {
        Raw,
        Cooking,
        Cooked
    }
}
