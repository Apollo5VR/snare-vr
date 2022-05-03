using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public SpellSelectionWheelManager zoneSelector;

    public void Update()
    {
        if (BNG.InputBridge.Instance.XButtonUp)
        {
            if (zoneSelector.gameObject.activeSelf)
            {
                zoneSelector.gameObject.SetActive(false);
            }
            else
            {
                zoneSelector.gameObject.SetActive(true);
            }
        }
    }
}
