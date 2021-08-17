using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdInitializer : MonoBehaviour
{
    private void Awake()
    {
        MobileAds.Initialize(initStatus=>{});
    }

    
}
