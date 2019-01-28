using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    public GameObject player;
    
    // Start is called before the first frame update

    public static FollowCamera instance;
    void Awake()
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       transform.position = Vector3.Lerp(this.transform.position,
           new Vector3(player.transform.position.x, player.transform.position.y + 2.4f, -10),
           0.08f);
    }
}
