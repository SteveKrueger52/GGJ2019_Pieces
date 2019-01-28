using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleControl : MonoBehaviour
{
    public float rotationSpeed = 2f;

    public static BattleControl instance = null;
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
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.GetFlag("SHIP_CONTROL")){
        transform.Rotate(new Vector3(0, rotationSpeed * Input.GetAxis("Horizontal"), 0));

        if (Input.GetButtonDown("Use"))
        {

        }
        }
    }
}
