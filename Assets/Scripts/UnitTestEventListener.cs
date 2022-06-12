using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTestEventListener : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {  
        TestsConnector.TestThis?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
