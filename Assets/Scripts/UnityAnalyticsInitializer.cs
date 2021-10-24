using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;
using Unity.Services.Core;

public class UnityAnalyticsInitializer : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        if (!Application.isEditor)
        {
            await UnityServices.InitializeAsync();
        }
    }
}
