using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpellSelection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Wand")
        {
            //"Subject" 
            SpellSelectionWheelManager.OnSpellSelected?.Invoke(this.gameObject);
        }
    }
}
