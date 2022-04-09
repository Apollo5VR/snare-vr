using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text healthText; //TODO need to update for multiplayer

    // Start is called before the first frame update
    void Start()
    {
        healthText.text = ScriptsConnector.Instance?.GetHealth.Invoke("playerID").ToString() + "% Health";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
