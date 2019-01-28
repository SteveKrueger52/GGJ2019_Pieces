using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : Interactable
{

    public String furnitureName = "Furniture Object";
    
    // Start is called before the first frame update
    public override void Interact()
    {
        Debug.Log("You see a " + furnitureName + ".");
    }
    
    
}
