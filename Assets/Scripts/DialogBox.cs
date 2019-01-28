using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{
    private const float BG_FADE_TIME = .5f;
    public static DialogBox instance;
    public StoryThread activeThread;
    public Narration activeNarration { private get; set; }
    private List<Button> replies = new List<Button>();
    public Button selected;
    private bool midFlash = false;
    private static bool midLoad = false;
    [SerializeField] private GameObject replyPool;
    [SerializeField] private GameObject speaker;
    [SerializeField] private GameObject body;
    [SerializeField] private Image left;
    [SerializeField] private Image right;
    [SerializeField] private Image bg;
    
    // Start is called before the first frame update
    void Start()
    {
        // Singleton code
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        // START WITH NOTHING
        DestroyAllButtons();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        // Fade BG In/Out
        Color color = bg.color;
        if (midFlash)
        {
            color.a += 255 * (Time.deltaTime/BG_FADE_TIME);
            color.a = color.a > 255 ? 255 : color.a;
        }
        else
        {
            color.a += -255 * (Time.deltaTime/BG_FADE_TIME);
            color.a = color.a < 0 ? 255 : color.a;
        }
        bg.color = color;
        */
        
        if (GameManager.instance.GetFlag("DIALOGUE_VISIBLE"))
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        
        if (Input.GetButtonDown("Use") && activeNarration != null && activeNarration.GetType() != typeof(Question) && midLoad == false)
            // Cycle Narration if not a Question 
        {
            Cycle();
        }

        midLoad = false;

        if (activeNarration != null && activeNarration.GetType() == typeof(Question))
        {
            if (selected == null && (GameManager.instance.GetFlag("IN_BATTLE")|| Input.GetAxis("Vertical") != 0.0))
            {
                selected = replies[0];
                FindObjectOfType<EventSystem>().SetSelectedGameObject(selected.gameObject);
            }
        }
        /*
        // test basic Dialogue
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Test Narration Loaded");
            activeNarration = Narration.DialogueLibrary["Rowe-FlavorText-10"];
            updateUI();
        }
        
        // test basic Questions
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Test Question Loaded");
            activeNarration = Narration.QuestionLibrary["Darci-Argument-2"];
            updateUI();
        }
        */   
    }

    public void Cycle()
    {
        Debug.Log("Cycling Narration");
        activeNarration = activeNarration.Cycle();
        if (SceneManager.GetActiveScene().name == "BattleScene") StartCoroutine(Battlefield.instance.Advance());
        updateUI();
    }

    // Updates UI Elements to contain activeNarration's values
    private void updateUI()
    {
        // No narration, close dialog window
        if (activeNarration == null)
        {
            Narration next = activeThread.GetNext();
            if (next == null) {
                body.gameObject.SetActive(false);
                speaker.gameObject.SetActive(false);
                if (GameManager.instance.GetFlag("HAS_CONTROL") == false)
                {
                    GameManager.instance.TriggerFlag("HAS_CONTROL");
                }
                
                if (GameManager.instance.GetFlag("DIALOGUE_VISIBLE") == false)
                {
                    GameManager.instance.TriggerFlag("DIALOGUE_VISIBLE");
                }
    
                StartCoroutine("cooldown");
                DestroyAllButtons();
                return;
            }
            else
            {
                activeNarration = next;
            }
        }
        // Populate text boxes if not empty
        if (!string.IsNullOrEmpty(activeNarration.speaker)){
            speaker.gameObject.SetActive(true);
            speaker.GetComponentInChildren<TextMeshProUGUI>().text = activeNarration.speaker;
        }
        else
            speaker.gameObject.SetActive(false);

        body.gameObject.SetActive(true);
        body.GetComponentInChildren<TextMeshProUGUI>().text = string.IsNullOrWhiteSpace(activeNarration.body) 
            ? "" : activeNarration.body;
        
        // Generate Button List
        string[] n_replies = activeNarration.GetReplies();
        DestroyAllButtons();
        for (int i = 0; i < n_replies.Length; i++)
        {
            // Create New Button
            GameObject b = (GameObject) Instantiate(Resources.Load("RuntimePrefabs/ReplyButton"));
            b.transform.SetParent(replyPool.transform, false);
            b.GetComponentInChildren<TextMeshProUGUI>().text = n_replies[i];
            replies.Add(b.GetComponent<Button>());
        }

        float offset = replies.Count == 0 ? -20 : -500;
        //Debug.Log(offset + " -> " + body.GetComponent<RectTransform>().offsetMax);
        body.GetComponent<RectTransform>().offsetMax = new Vector2(offset, body.GetComponent<RectTransform>().offsetMax.y);

        // Remove Sprites
        changeAlpha(left,0);
        changeAlpha(right,0);
        // Add sprite if necessary
        if (activeNarration != null && !String.IsNullOrWhiteSpace(activeNarration.sprite))
        {
            Image activeSide = activeNarration.side == Narration.Direction.Right ? right : left;
            string[] callsign = activeNarration.sprite.Split('_');
            activeSide.sprite = Resources.LoadAll<Sprite>(callsign[0])[int.Parse(callsign[1])-1];
            changeAlpha(activeSide, 128f);
        }
    }

    private void changeAlpha(Image toChange, float a)
    {
        Color color = toChange.color;
        color.a = a;
        toChange.color = color;
    }
    
    public void UpdateBG(Sprite sprite)
    {
        midFlash = sprite == null;
        if (sprite != null)
            bg.sprite = sprite;
    }
    

    // Takes call from a clicked button to cycle the narration using that button's index
    public void buttonClicked(Button clicked)
    {
        Debug.Log(replies.IndexOf(clicked));
        int stat;
        activeNarration = activeNarration.Cycle(replies.IndexOf(clicked),out stat);
        if (GameManager.instance.GetFlag("IN_BATTLE"))
        {
            StartCoroutine(Battlefield.instance.Advance());
            Battlefield.instance.currentValue = stat;
        }
        
        updateUI();
    }
    
    public static void newNarration(StoryThread n)
    {
        instance.activeThread = n;
        instance.activeNarration = instance.activeThread.GetNext();
        midLoad = true;
        instance.updateUI();

    }

    private void DestroyAllButtons()
    {   
        replies = new List<Button>();
        foreach (Button b in replyPool.GetComponentsInChildren<Button>())
            Destroy(b.gameObject);
    }

    private IEnumerator cooldown()
    {
        if (!GameManager.instance.GetFlag("DIALOGUE_COOLDOWN"))
        {
            GameManager.instance.TriggerFlag("DIALOGUE_COOLDOWN");
        };
        
        yield return new WaitForSeconds(1.2f);
        
        GameManager.instance.TriggerFlag(("DIALOGUE_COOLDOWN"));
    }
}
