using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Settings", menuName = "Data/Settings", order = 1)]
public class Settings : ScriptableObject
{
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;

    public int graphicsSetting = 2;
}
