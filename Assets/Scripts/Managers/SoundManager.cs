using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> WoodSounds;
    [SerializeField] private List<AudioClip> StoneSounds;
    [SerializeField] private List<AudioClip> GlassSounds;
    public AudioClip StandartDeathSound;

    //[SerializeField] AudioSource 
    public  static SoundManager Instance;
    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void PlayClip(AudioClip clip, AudioSource source)
    {
        source.clip = clip;
        source.Play();
    }

    public void PlayRandomClip(AudioClip[] clips, AudioSource source) {
        int rnd = Random.Range(0,clips.Length);

        source.clip = clips[rnd];
        source.Play();
    }

    public void PlayRandomWoodClip(AudioSource source)
    {

        int rnd = Random.Range(0, WoodSounds.Count);
        //Debug.Log($"sound {rnd} is played");
        source.clip = WoodSounds[rnd];
        source.Play();
    }
    public void PlayRandomGlassClip(AudioSource source)
    {

        int rnd = Random.Range(0, GlassSounds.Count);
        //Debug.Log($"sound {rnd} is played");
        source.clip = GlassSounds[rnd];
        source.Play();
    }

    public void PlayRandomStoneClip(AudioSource source)
    {

        int rnd = Random.Range(0, StoneSounds.Count);
        //Debug.Log($"sound {rnd} is played");
        source.clip = StoneSounds[rnd];
        source.Play();
    }

    public static void TurnAllSoundsOff() {
        AudioListener.volume = 0;
    }
    public static void TurnAllSoundsOn()
    {
        AudioListener.volume = 1;
    }

    public static void PauseAllSounds()
    {
        AudioListener.pause = true;
    }
    public static void UnPauseAllSounds()
    {
        AudioListener.pause = false;
    }
}
