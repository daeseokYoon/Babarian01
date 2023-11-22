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
        // �� �ϳ�, ������ ��ü�� �����
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
        SetCameraPriority(0); // ī�޶� �ʱ�ȭ
    }

    // ��� ī�޶��� �켱������ ���߰� ������ ī�޶��� �켱������ ���̴� ���
    public void SetCameraPriority(int activeCameraIndex)
    {
        for(int i = 0; i < cameras.Length; i++)
        {
            if(i == activeCameraIndex)
            {
                cameras[i].Priority = 10; // Ȱ��ȭ
            }
            else
            {
                cameras[i].Priority = 1;  // ��Ȱ��ȭ
            }
        }
    }

    public void InvisiblePlayer(bool view)
    {
        player.SetActive(view);
    }
}
