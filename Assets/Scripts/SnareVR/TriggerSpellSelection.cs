using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO - rename to appropriate Wire use (ZoneSelection)
public class TriggerSpellSelection : MonoBehaviour
{
    public GameObject loadingLine;
    public float touchTime = 0;
    public float distanceTilLoad = 0.25f;
    public int sceneIndex;
    public float heightReturnOffset = 0.125f;
    public float dist;

    private bool isGrabbed;
    private BNG.GrabbableHaptics selectionHaptics;
    private BNG.Grabbable selectionGrabber;

    public  void Start()
    {
        selectionHaptics = gameObject.GetComponent<BNG.GrabbableHaptics>();
        selectionGrabber = gameObject.GetComponent<BNG.Grabbable>();
    }

    private void Update()
    {
        //TODO - could use refactor - view Profiler
        if(isGrabbed)
        {
            //update line width based on distance of this object from line
            dist = Vector3.Distance(gameObject.transform.position, loadingLine.transform.position);
            loadingLine.transform.localScale = new Vector3(dist/1.5f, loadingLine.transform.localScale.y, loadingLine.transform.localScale.z);
            selectionHaptics.doHaptics(selectionHaptics.currentGrabber.HandSide);

            //if reach full 1 distance, then teleport
            if (dist > distanceTilLoad)
            {
                selectionGrabber.DropAll();

                ScriptsConnector.Instance.OnReturnMap();

                //TODO - this is where we can refactor for async load (improve run time) - view profiler
                ProgressionController.Instance.OnLoadSelectedScene(sceneIndex);
            }
        }
    }

    public void ToggleGrabbed(bool isGrab)
    {
        isGrabbed = isGrab;

        if(!isGrab)
        {
            if(dist < distanceTilLoad)
            {
                //reset line length
                loadingLine.transform.localScale = new Vector3(0.01f, loadingLine.transform.localScale.y, loadingLine.transform.localScale.z);
            }

            //return cube to origin
            gameObject.transform.rotation = new Quaternion(0,0,0,0);
            gameObject.transform.position = loadingLine.transform.position; //TODO slightly below
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y - heightReturnOffset, gameObject.transform.localPosition.z);
        }
    }

    /*
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name == "Wand")
        {
            //"Subject" 
            if(touchTime < 0.75f)
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
        loadingLine.transform.localScale = new Vector3(touchTime, loadingLine.transform.localScale.y, loadingLine.transform.localScale.z);
    }
    */
}
