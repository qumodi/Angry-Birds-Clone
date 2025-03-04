using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    [SerializeField] private CinemachineVirtualCamera IdleCamera;
    [SerializeField] private CinemachineVirtualCamera FollowCamera;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        SwitchToIdle();
        Time.timeScale = 0.8f;
    }
    public void SwitchToIdle()
    {
        IdleCamera.enabled = true;
        FollowCamera.enabled = false;
    }

    public void SwitchToFollow(Transform obj)
    {
        FollowCamera.Follow = obj;

        IdleCamera.enabled = false;
        FollowCamera.enabled = true;
    }
}
