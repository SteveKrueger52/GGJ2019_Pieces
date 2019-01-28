﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantPackage : MonoBehaviour
{

    public static ConstantPackage instance;
    // Start is called before the first frame update
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
                
            //if not, set instance to this
            instance = this;
            
        //If instance already exists and it's not this:
        else if (instance != this)
                
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);    

    }
}
