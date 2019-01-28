using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : Interactable
{
    [SerializeField]
    public string moveTo;
    public bool sceneChange;
    public Vector3 movePosition;
    public String conversation;
    
    public override void Interact()
    {
        GameManager.instance.TriggerFlag("TRANSFER_QUEUE");
        GameManager.instance.queuedScene = moveTo;
        if (GameManager.instance.GetFlag("DIALOGUE_VISIBLE") && !GameManager.instance.GetFlag("DIALOGUE_COOLDOWN")) {
            GameManager.instance.TriggerFlag("DIALOGUE_VISIBLE");
            GameManager.instance.TriggerFlag("HAS_CONTROL");
            DialogBox.newNarration(Narration.SceneLibrary[conversation]);
        }
    }

    private void MoveScene()
    {

    }
}
