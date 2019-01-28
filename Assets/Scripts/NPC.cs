using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    public String npcName;
    public String conversation;
    public bool expire = false;
    
    public override void Interact()
    {
        if (GameManager.instance.GetFlag("DIALOGUE_VISIBLE") && !GameManager.instance.GetFlag("DIALOGUE_COOLDOWN")) {
            GameManager.instance.TriggerFlag("DIALOGUE_VISIBLE");
            GameManager.instance.TriggerFlag("HAS_CONTROL");
            Debug.Log("Talked to " + npcName + ".");
            DialogBox.newNarration(Narration.SceneLibrary[conversation]);
            if (expire) Destroy(this.gameObject);
        }

    }
}
