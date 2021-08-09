using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToHead : MonoBehaviour
{
    public Camera m_MainCamera;

    private void Start()
    {
        m_MainCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(other.tag == "player")
        //{
            transform.position = m_MainCamera.gameObject.transform.position;
            transform.localRotation = transform.localRotation * new Quaternion(0, 0, 180, 0);
            transform.parent = m_MainCamera.gameObject.transform;
        //}
    }
}
