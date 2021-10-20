using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpellSelection : MonoBehaviour
{
    public GameObject loadingLine;
    public float touchTime = 0;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name == "Wand")
        {
            //"Subject" 
            if(touchTime < 1)
            {
                loadingLine.transform.localScale = new Vector3(touchTime, loadingLine.transform.localScale.y, loadingLine.transform.localScale.z);
                touchTime += Time.deltaTime;
            }
            else
            {
                touchTime = 0;
                SpellSelectionWheelManager.OnSpellSelected?.Invoke(this.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        touchTime = 0;
    }

    private void OnDisable()
    {
        touchTime = 0;
    }

    private void Update()
    {
        if (touchTime < 1)
        {
            loadingLine.transform.localScale = new Vector3(touchTime, loadingLine.transform.localScale.y, loadingLine.transform.localScale.z);
            touchTime += Time.deltaTime;
        }
    }
}
