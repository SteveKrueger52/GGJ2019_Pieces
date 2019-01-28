using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveState
{
    public static String savedScene;
    public static Vector3 savedPlayerPosition;
    public static Vector3 savedCameraPosition;
}

public class GameManager : MonoBehaviour
{

    public GameState state;
    public Settings settings;
    public AudioManager audioController;

    // Battle data
    public List<BattleFile> battles;
    public BattleFile currentBattle;

    public string queuedScene;


    public static GameManager instance = null;
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


        if (SceneManager.GetActiveScene().name == "Overworld")
        {
            Debug.Log("hello");

            Narration.Parser.ParseDialogues("CSV/Dialogues");

            Narration.Parser.ParseQuestions("CSV/Questions");
            Narration.Parser.ParseScenes("CSV/StoryThreads");
            Debug.Log("by");
        }
    }

    void Update()
    {
        CheckBattle();
        TravelQueue();
    }

    private void CheckForGO()
    {
        if (GetFlag("GAME_OVER"))
        {
            SceneManager.LoadScene("Game Over", LoadSceneMode.Single);
        }
    }
    private void TravelQueue()
    {
        if (GetFlag("TRANSFER_QUEUE"))
        {
            TriggerFlag("TRANSFER_QUEUE");
            Debug.Log("Hewwo???");
            SceneManager.LoadScene(queuedScene, LoadSceneMode.Single);
            queuedScene = "Error";
        }
    }
    private void CheckBattle()
    {
        if (GetFlag("IN_BATTLE") && SceneManager.GetActiveScene() != SceneManager.GetSceneByName("BattleScene"))
        {
            //SaveState.savedCameraPosition = FollowCamera.instance.transform.position;
            //SaveState.savedPlayerPosition = PlayerMovement.instance.transform.position;

            if (GetFlag("END_STRETCH"))
            {
                currentBattle = battles[4];
                SaveState.savedScene = "Overworld DQK";
            }
            else if (GetFlag("KAI"))
            {
                currentBattle = battles[3];
                SaveState.savedScene = "Jesse";
            }
            else if (GetFlag("QUINN"))
            {
                currentBattle = battles[2];
                SaveState.savedScene = "Overworld DQK";
            }
            else if (GetFlag("DARCI"))
            {
                currentBattle = battles[1];
                SaveState.savedScene = "Overworld DQ";
            }
            else if (GetFlag("HAS_KEYS"))
            {
                currentBattle = battles[0];
                SaveState.savedScene = "Overworld D";
            }
            else
            {
                currentBattle = battles[0];
                SaveState.savedScene = "Overworld D";
            }

            //            FollowCamera.instance.gameObject.SetActive(false);
            SceneManager.LoadScene("BattleScene", LoadSceneMode.Single);

        }
        else if (!GetFlag("IN_BATTLE") && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("BattleScene") && GetFlag("DIALOGUE_VISIBLE"))
        {
            SceneManager.LoadScene(SaveState.savedScene, LoadSceneMode.Single);
            //FollowCamera.instance.gameObject.SetActive(true);
            //FollowCamera.instance.gameObject.transform.position = SaveState.savedCameraPosition;
            //PlayerMovement.instance.gameObject.transform.position = SaveState.savedPlayerPosition;
        }
    }

    public bool GetFlag(String f)
    {
        return state.GetFlag(f);
    }

    public void TriggerFlag(String f)
    {
        state.TriggerFlag(f);
    }

    public void ResetAllFlags()
    {
        state.ResetAllFlags();
    }

}
