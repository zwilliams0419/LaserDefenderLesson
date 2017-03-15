using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    static MusicPlayer instance = null;

    public AudioClip startClip, gameClip, gameOverClip;

    private AudioSource music;

    // Use this for initialization
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            print("Duplicate MusicPlayer destroyed");
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            music = GetComponent<AudioSource>();
            music.clip = startClip;
            music.Play();
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (music != null)
        {
            music.Stop();

            switch (level)
            {
                case 0:
                    music.clip = startClip;
                    break;

                case 1:
                    music.clip = gameClip;
                    break;

                case 2:
                    music.clip = gameOverClip;
                    break;

                default:
                    break;
            }

            music.Play();
        }
    }
}
