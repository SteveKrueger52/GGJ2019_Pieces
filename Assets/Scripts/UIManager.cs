using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject screen;

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
        
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "BattleScene")
        {
            screen.SetActive(false);
        } else
        {
            screen.SetActive(true);
        }
    }
    
}
