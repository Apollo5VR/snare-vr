using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapController : MonoBehaviour
{
    public ZoneSelectionManager zoneSelector;
    public float distanceTilLoad = 0.25f;
    public float dist;
    public MeshRenderer m_Renderer;
    public Material m_Material;

    public Transform watchOrigin;

    public BNG.GrabbableHaptics selectionHaptics;
    private BNG.Grabbable mapGrabbable;

    private Coroutine distCoroutine;
    private bool checkToggle;

    public void Start()
    {
        selectionHaptics = gameObject.GetComponent<BNG.GrabbableHaptics>();
        mapGrabbable = gameObject.GetComponent<BNG.Grabbable>();
        m_Material = m_Renderer.material;

        ScriptsConnector.Instance.OnReturnMap += ReturnMap;
    }

    #if UNITY_EDITOR
    public void Update()
    {
        //testing only
        if (BNG.InputBridge.Instance.XButtonUp)
        {
            if (zoneSelector.gameObject.activeSelf)
            {
                zoneSelector.gameObject.SetActive(false);
            }
            else
            {
                zoneSelector.gameObject.SetActive(true);


                string health = ScriptsConnector.Instance?.GetHealth.Invoke("playerID").ToString();
                ScriptsConnector.Instance.OnUpdateUI(CommonEnums.UIType.Health, health);
            }
        }
    }
    #endif

    private void ReturnMap()
    {
        zoneSelector.gameObject.SetActive(false);

        //relocate to wrist, child to right hand, color grey
        gameObject.transform.SetParent(watchOrigin.transform);
        gameObject.transform.position = watchOrigin.transform.position; //TODO need?
        m_Material.color = new Color32(0, 0, 0, 0);
    }

    private async void GetTime()
    {
        if (Unity.Services.Authentication.AuthenticationService.Instance.IsSignedIn)
        {
            float time = await CloudCodeManager.instance.CallGetTrapTimeRemainingEndpoint();
            string timeStr = time.ToString();
            ScriptsConnector.Instance.OnUpdateUI(CommonEnums.UIType.Time, timeStr);
        }
    }

    public void ToggleMap(bool isGrab)
    {
        if(isGrab)
        {
            checkToggle = true;
            StartCoroutine(MapGrabLoopCheck());
        }
        else
        {
            //drop all
            mapGrabbable.DropAll();

            if(zoneSelector.gameObject.activeSelf)
            {
                gameObject.transform.position = zoneSelector.transform.position;
                gameObject.transform.SetParent(zoneSelector.transform);
            }
        }
    }

    private IEnumerator MapGrabLoopCheck()
    {
        //TODO - room for optimization here 7.28 - review in Profiler
        while(checkToggle)
        {
            dist = Vector3.Distance(gameObject.transform.position, watchOrigin.transform.position);

            if (zoneSelector.gameObject.activeSelf)
            {
                if (dist < distanceTilLoad)
                {
                    checkToggle = false;
                    mapGrabbable.DropAll();
                    ReturnMap();
                    m_Material.color = new Color32(0, 0, 0, 0);
                }
            }
            else
            {
                if (dist > distanceTilLoad)
                {
                    zoneSelector.gameObject.SetActive(true);

                    //relocate to 1ft out, force ungrab, unchild from right hand, relocate to above map, change color to green
                    mapGrabbable.DropAll();
                    gameObject.transform.SetParent(zoneSelector.transform);
                    gameObject.transform.position = zoneSelector.transform.position;
                    m_Material.color = new Color32(0, 255, 243, 0);

                    string health = ScriptsConnector.Instance?.GetHealth.Invoke("playerID").ToString();
                    ScriptsConnector.Instance.OnUpdateUI(CommonEnums.UIType.Health, health);

                    GetTime();

                    checkToggle = false;
                }
            }

            yield return null;
        }
    }

    private void OnDestroy()
    {
        if(ScriptsConnector.Instance != null)
        {
            ScriptsConnector.Instance.OnReturnMap -= ReturnMap;
        }
    }
}


