using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject ButtonLock;
    public static bool Paused = false;
    void Start()
    {
        if (Paused)//AudioListener.pause
        {
            ButtonLock.SetActive(true);
        }
        else
        {
            ButtonLock.SetActive(false);
        }
    }

    public void PressSoundButton()
    {
        if (Paused)//AudioListener.pause
        {
            UnPauseAllSoundsButton();
        }
        else
        {
            PauseAllSoundsButton();
        }
        Debug.Log("Volume: "+AudioListener.volume);
    }

    public void PauseAllSoundsButton()
    {
        //AudioListener.pause = true;
        AudioListener.volume = 0f;
        ButtonLock.SetActive(true);
        Paused = true;
    }

    public void UnPauseAllSoundsButton()
    {
        //AudioListener.pause = false;
        AudioListener.volume = 1f;

        ButtonLock.SetActive(false);
        Paused = false;
    }

    public static void ReturnSoundIfNotPaused()
    {
        if (!Paused)//AudioListener.pause
        {
            AudioListener.volume = 1;
        }
    }
}
