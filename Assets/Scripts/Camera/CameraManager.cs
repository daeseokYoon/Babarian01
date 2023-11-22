using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    static CameraManager instance;

    public static CameraManager Instance { get; private set; }

    private void Awake()
    {
        // 단 하나, 유일한 객체로 남기기
        if (instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public CinemachineVirtualCameraBase[] cameras;
    public GameObject player;

    private void Start()
    {
        SetCameraPriority(0); // 카메라 초기화
    }

    // 모든 카메라의 우선순위를 낮추고 지정된 카메라의 우선순위를 높이는 방식
    public void SetCameraPriority(int activeCameraIndex)
    {
        for(int i = 0; i < cameras.Length; i++)
        {
            if(i == activeCameraIndex)
            {
                cameras[i].Priority = 10; // 활성화
            }
            else
            {
                cameras[i].Priority = 1;  // 비활성화
            }
        }
    }

    public void InvisiblePlayer(bool view)
    {
        player.SetActive(view);
    }
}
