using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[System.Serializable]
public class Flag
{
    public string id;
    public bool flag;

    public Flag(string id, bool flag)
    {
        this.id = id;
        this.flag = flag;
    }
}

[CreateAssetMenu(fileName = "GameState", menuName = "Data/GameState", order = 1)]
public class GameState : ScriptableObject
{
    public List<Flag> flags;
    public Dialogue dialogueDb;


    public void TriggerFlag(String f)
    {
        Flag flag = flags.Find(a => a.id == f);
        // Here it is. The worst line of code in the world.
        flag.flag = !flag.flag;
    }

    public bool GetFlag(String f)
    {
        Flag flag = flags.Find(a => a.id == f);
        return flag.flag;
    }

    // Resets all flags in the game to their initial state
    public void ResetAllFlags()
    {
        foreach (Flag flag in flags)
        {
            flag.flag = false;
        }
        flags.Find(a => a.id == "DIALOGUE_VISIBLE").flag = true;
        flags.Find(a => a.id == "HAS_CONTROL").flag = true;
    }
    
}


