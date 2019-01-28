using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    public List<AudioClip> music;

    public AudioClip currentMusic;

    public AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SwitchMusic();
        
        if (SceneManager.GetActiveScene().name.Equals("MainMenu"))
        {
            currentMusic = music[7];
        }
        else if (SceneManager.GetActiveScene().name.Equals("BattleScene"))
        {

            if (GameManager.instance.GetFlag("END_STRETCH"))
            {
                currentMusic = music[4];
            }
            else if (GameManager.instance.GetFlag("KAI"))
            {
                currentMusic = music[3];
            }
            else if (GameManager.instance.GetFlag("QUINN"))
            {
                currentMusic = music[2];
            }
            else if (GameManager.instance.GetFlag("DARCI"))
            {
                currentMusic = music[1];
            }
            else
            {
                currentMusic = music[0];
            }
        }
        else
        {
            if (GameManager.instance.GetFlag("PHOTO_SEQUENCE"))
            {
                currentMusic = music[5];
            }
            else
            {
                currentMusic = music[6];
            }

        }
    }

    void SwitchMusic()
    {
        if (source.clip != currentMusic) {
        source.Stop();
        source.clip = currentMusic;
        source.Play();
        source.loop = true;
        }
    }
}
