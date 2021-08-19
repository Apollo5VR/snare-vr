using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToHead : MonoBehaviour
{
    public SnapToHead[] allMasks;
    public AudioSource danceMusic;
    public GameObject danceText;

    private Camera m_MainCamera;

    private void Start()
    {
        m_MainCamera = Camera.main;
        allMasks = GameObject.FindObjectsOfType<SnapToHead>();
    }

    private void OnTriggerEnter(Collider other)
    {
        transform.position = m_MainCamera.gameObject.transform.position;
        transform.localRotation = transform.localRotation * new Quaternion(0, 0, 180, 0);
        transform.parent = m_MainCamera.gameObject.transform;

        danceText.SetActive(true);
        danceMusic.Play();

        //TODO - diable other masks & activate timer to next level (short but not too short)
        foreach (SnapToHead mask in allMasks)
        {
            if(mask.gameObject != this.gameObject)
            {
                mask.gameObject.SetActive(false);
            }
        }

        StartCoroutine(TimerToEndScene());
    }

    private IEnumerator TimerToEndScene()
    {
        yield return new WaitForSeconds(8);

        ProgressionController.Instance.OnLoadNextScene();
    }
}
