using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MaskController : MonoBehaviour
{
    public static MaskController Instance { get; private set; }
    public GameObject[] allMasks;
    public AudioSource danceMusic;
    public GameObject[] danceText;

    public Action<CommonEnums.HouseResponses, GameObject> OnMaskSceneResponseSelected;

    public GameObject selectedMask; //make private
    public Transform leftPoint;
    public Transform rightPoint;

    private Dictionary<GameObject, Quaternion> storedLocalRotations;

    private Camera m_MainCamera;

    private float minimum;
    private float maximum;

    private float baseMin;

    // starting value for the Lerp
    private static float t = 0.0f;
    private bool maskCaught;

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

    private void Start()
    {
        m_MainCamera = Camera.main;
        //allMasks = GameObject.FindObjectsOfType<SnapToHead>();

        minimum = leftPoint.position.x;
        maximum = rightPoint.position.x;
        baseMin = rightPoint.position.y;

        storedLocalRotations = new Dictionary<GameObject, Quaternion>();
        foreach (GameObject mask in allMasks)
        {
            storedLocalRotations.Add(mask, mask.transform.localRotation);
        }

        OnMaskSceneResponseSelected += MaskSelected;
    }

    public void MaskSelected(CommonEnums.HouseResponses response, GameObject maskGO)
    {
        ResponseCollector.Instance.OnResponseSelected?.Invoke(response);

        //TODO - diable other masks 
        foreach (GameObject mask in allMasks)
        {
            if (mask != maskGO)
            {
                mask.SetActive(false);
            }
            else
            {
                selectedMask = maskGO;
            }
        }

        danceText[0].SetActive(false);
        danceText[1].SetActive(true);

        //TODO - start the chase 
        StartCoroutine(EngageChaseSequence(selectedMask));

        //TODO - refactor to one location (so we only need 1 script to have Analytics dependency)
        /*
        if (!Application.isEditor)
        {
            //Analytics Beta
            Dictionary<string, object> parameters = new Dictionary<string, object>()
                {
                    { "specificQuestion", "Masks" },
                    { "houseIndex", (int)response },
                };
            AnalyticsService.Instance.CustomData("questionResponse", parameters);
        }
        */
    }

    private IEnumerator EngageChaseSequence(GameObject mask)
    {
        // animate the position of the game object...
        while (!maskCaught)
        {
            mask.transform.position = new Vector3(Mathf.Lerp(minimum, maximum, t), baseMin + Mathf.PingPong(Time.time * 0.5f, 0.1f) - 0.25f * 0.1f, mask.transform.position.z);

            // .. and increase the t interpolater
            t += 0.5f * Time.deltaTime;

            // now check if the interpolator has reached 1.0
            // and swap maximum and minimum so game object moves
            // in the opposite direction.
            if (t > 1.0f)
            {
                float temp = maximum;
                maximum = minimum;
                minimum = temp;
                t = 0.0f;
            }

            yield return null;
        }
    }

    //TODO - call on GRAB & update text to Put it on!
    public void MaskCaught()
    {
        maskCaught = true; //TODO - needs to be moved earlier? enought to cut off lerp?

        danceText[1].SetActive(false);
        danceText[2].SetActive(true);
    }

    public void MaskReleased()
    {
        selectedMask.transform.position = m_MainCamera.gameObject.transform.position;
        selectedMask.transform.localRotation = storedLocalRotations[selectedMask] * new Quaternion(0, 0, 0.5f, 0);
        selectedMask.transform.parent = m_MainCamera.gameObject.transform;

        danceText[2].SetActive(false);
        danceText[3].SetActive(true);
        danceMusic.Play();

        //activate timer to next level(short but not too short)
        ProgressionController.Instance.OnLoadNextScene?.Invoke(8);

        SceneManager.sceneUnloaded += DestroyMask;
    }

    private void DestroyMask(Scene scene)
    {
        Destroy(selectedMask);
        SceneManager.sceneUnloaded -= DestroyMask;
        maskCaught = false;
        selectedMask = null;
    }    
}
