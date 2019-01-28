using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RingData
{
    public GameObject mesh;
    public float speed;
    private float scale = 1;

    RingData(GameObject mesh, float speed)
    {
        this.mesh = mesh;
        this.speed = speed;
        scale = 1;
    }
}


[CreateAssetMenu(fileName = "BattleFile", menuName = "Data/BattleFile", order = 1)]
public class BattleFile : ScriptableObject
{

    public List<RingData> rings;
    public int ringCount = 0;
}
