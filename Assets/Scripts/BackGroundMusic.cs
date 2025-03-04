using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusic : MonoBehaviour
{
    public static BackGroundMusic Instance;
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioSource source;
    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(transform.gameObject);
    }

    private void Start()
    {
        source.clip = music;
        source.Play();
    }

    public void StopMusic()
    {
        source.Pause();
    }
    public void PlayMusic()
    {
        source.UnPause();
    }
}
