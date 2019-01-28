using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    private GameManager gameManager;
    private bool creditsVisible = false;
    private const float TRANSITION_TIME = 2f; // the time to transition from Title to Credits or back
    private float transitionTimer; // 0 - title, TRANSITION_TIME - credits
    [SerializeField]
    private Image credits, title, swap, shimmer;
    [SerializeField]
    private Sprite creditSprite, titleSprite;
    private Color unseen = new Color(255,255,255,0);

    public void OnEnable()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.ResetAllFlags();
        Debug.Log("Reset all flags");
        gameManager.TriggerFlag("HAS_CONTROL");
    }

    public void StartGame()
    {
        credits.color = unseen;
        title.color = Color.white;
        swap.sprite = titleSprite;
        gameManager.TriggerFlag("HAS_CONTROL");
        SceneManager.LoadScene("Overworld");
    }

    public void Update()
    {   // Jesse Face Shimmer
        if (shimmer.gameObject.activeInHierarchy)//) && gameManager.GetFlag("GAME_BEATEN"))
            shimmer.gameObject.SetActive(false);
        if (!shimmer.gameObject.activeInHierarchy)// && !gameManager.GetFlag("GAME_BEATEN"))
            shimmer.gameObject.SetActive(true);
        
        // Back to Title, view Credits, or Exit Game
        if (Input.GetButtonDown("Cancel"))
        {
            if (creditsVisible)
            {
                creditsVisible = false;
                credits.color = unseen;
                title.color = Color.white;
                swap.sprite = titleSprite;
            }
            else
                ExitGame();
        }
        if (Input.GetButtonDown("Use"))
        {
            creditsVisible = true;
            credits.color = Color.white;
            title.color = unseen;
            swap.sprite = creditSprite;
        }

        if (Input.GetButtonDown("Submit"))
        {
            StartGame();
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
