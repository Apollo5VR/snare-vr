using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapController : MonoBehaviour
{
    public SpellSelectionWheelManager zoneSelector;
    public float distanceTilLoad = 0.25f;
    public float dist;
    public MeshRenderer m_Renderer;
    public Material m_Material;
    public GameObject selectedWrist;

    public BNG.GrabbableHaptics selectionHaptics;
    private BNG.Grabbable mapGrabber;

    private Coroutine distCoroutine;
    private bool checkToggle;

    public void Start()
    {
        selectionHaptics = gameObject.GetComponent<BNG.GrabbableHaptics>();
        mapGrabber = gameObject.GetComponent<BNG.Grabbable>();
        m_Material = m_Renderer.material;
    }

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

        if(checkToggle)
        {
            //selectionHaptics.doHaptics(selectionHaptics.currentGrabber.HandSide);

            dist = Vector3.Distance(gameObject.transform.position, selectedWrist.transform.position);

            if (zoneSelector.gameObject.activeSelf)
            {
                //if reach full 1 distance, then teleport
                if (dist < distanceTilLoad)
                {
                    zoneSelector.gameObject.SetActive(false);

                    //relocate to wrist, child to right hand, color grey
                    mapGrabber.DropAll();
                    gameObject.transform.position = new Vector3(selectedWrist.transform.position.x, selectedWrist.transform.position.y, selectedWrist.transform.position.z - 0.2f);
                    gameObject.transform.SetParent(selectedWrist.transform);
                    m_Material.color = new Color32(0, 0, 0, 0);

                    //StopCoroutine(distCoroutine);
                    checkToggle = false;
                }
            }
            else
            {
                //if reach full 1 distance, then teleport
                if (dist > distanceTilLoad)
                {
                    zoneSelector.gameObject.SetActive(true);

                    //relocate to 1ft out, force ungrab, unchild from right hand, relocate to above map, change color to green?
                    mapGrabber.DropAll();
                    gameObject.transform.SetParent(null);
                    gameObject.transform.position = zoneSelector.transform.position;
                    m_Material.color = new Color32(0, 255, 243, 0);

                    //StopCoroutine(distCoroutine);
                    checkToggle = false;
                }
            }
        }
    }

    public void ToggleMapGrab()
    {
        checkToggle = true;

        //distCoroutine = StartCoroutine(MapGrabLoopCheck());
    }

    private IEnumerator MapGrabLoopCheck()
    {
        while(true)
        {
            //selectionHaptics.doHaptics(selectionHaptics.currentGrabber.HandSide);

            dist = Vector3.Distance(gameObject.transform.position, selectedWrist.transform.position);

            if (zoneSelector.gameObject.activeSelf)
            {
                //if reach full 1 distance, then teleport
                if (dist < distanceTilLoad)
                {
                    zoneSelector.gameObject.SetActive(false);

                    //relocate to wrist, child to right hand, color grey
                    mapGrabber.DropAll();
                    gameObject.transform.position = selectedWrist.transform.position;
                    gameObject.transform.SetParent(selectedWrist.transform);
                    m_Material.SetColor("_EmissionColor", new Color32(0, 0, 0, 0));

                    StopCoroutine(distCoroutine);
                }
            }
            else
            {
                //if reach full 1 distance, then teleport
                if (dist > distanceTilLoad)
                {
                    zoneSelector.gameObject.SetActive(true);

                    //relocate to 1ft out, force ungrab, unchild from right hand, relocate to above map, change color to green?
                    mapGrabber.DropAll();
                    gameObject.transform.SetParent(null);
                    gameObject.transform.position = zoneSelector.transform.position;
                    m_Material.SetColor("_EmissionColor", new Color32(0, 255, 243, 0));

                    StopCoroutine(distCoroutine);
                }
            }

            return null;
        }
    }
}


