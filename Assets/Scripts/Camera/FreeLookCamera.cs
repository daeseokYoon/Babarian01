using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
//using UnityEngine.UIElements;

public class FreeLookCamera : MonoBehaviour
{
    CinemachineVirtualCamera MainVcam;

    CinemachineFreeLook freeLook;
    public float scrollSpeed = 500f;
    
    float MaxFOV;
    public Transform LookAtme;
    float sensitivity = 200f; // rotspeed
    float xRotate, yRotate, xRotateMove, yRotateMove; 
    Vector3 originalRotationPos;
    Vector3 lastRoationPos;

    float rotationX;

    //public float rotationY;
    bool isRotating = false;
    bool isFighting = false;

    private void Awake()
    {
        CinemachineCore.GetInputAxis = clickControl;
    }
    
    public float clickControl(string axis)
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime;
        float[] minRadius = { 0.45f, 2.15f, 0.45f };
        float[] maxRadius = { 9f, 10.7f, 9f };

        for (int i = 0; i < freeLook.m_Orbits.Length; i++) // ������� Top, Middle, Bottom
        {
            freeLook.m_Orbits[i].m_Radius += scrollWheel;
            freeLook.m_Orbits[i].m_Radius = Mathf.Clamp(freeLook.m_Orbits[i].m_Radius, minRadius[i], maxRadius[i]);
        }

        //float scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        //freeLook.m_Orbits[0].m_Radius += scrollWheel * scrollSpeed * Time.deltaTime; // TopRig
        //freeLook.m_Orbits[1].m_Radius += scrollWheel * scrollSpeed * Time.deltaTime; ; // MiddleRig
        //freeLook.m_Orbits[2].m_Radius += scrollWheel * scrollSpeed * Time.deltaTime; ; // BottomRig min 0.45f max 9f

        //freeLook.m_Orbits[0].m_Radius = Mathf.Clamp(freeLook.m_Orbits[0].m_Radius, 0.45f, 9f);
        //freeLook.m_Orbits[1].m_Radius = Mathf.Clamp(freeLook.m_Orbits[1].m_Radius, 2.15f, 10.7f);
        //freeLook.m_Orbits[2].m_Radius = Mathf.Clamp(freeLook.m_Orbits[2].m_Radius, 0.45f, 9f);

        // localRotition�� �ǹ̴�? // freelookī�޶�� ���� �ܾƿ� �Ҷ� TopRig, MiddleRig, BottomRig�� Radius���� �������ָ� ��.
        // �Ʒ� ������ �ȸ����� �ڵ� // ���߼��� �ϴ� UI����
        //Vector3 cameraDirection = this.transform.localRotation * Vector3.forward; // this���� ī�޶� ���� ���� ������Ʈ�� �ϳ� ���� ���� �����ϴ°� �� ��������? ���� ���غ�
        //this.transform.position += cameraDirection * Time.deltaTime * scrollWheel * scrollSpeed;

        //// FOV�� ���� �ܾƿ��ϴ� �ڵ� // ȭ�� �̻�����
        //freeLook.m_Lens.FieldOfView += scrollWheel * scrollSpeed * Time.deltaTime;
        //// �� �� �ƿ� �Ҷ� fov�� �������� z���� �յڷ� ���Բ� ��������
        //freeLook.m_Lens.FieldOfView = Mathf.Clamp(freeLook.m_Lens.FieldOfView, 6.7f, 140f);

        if (Input.GetMouseButton(1))
            return UnityEngine.Input.GetAxis(axis) * sensitivity *Time.deltaTime;
        return 0;
    }

    void Start()
    {
        freeLook = GetComponent<CinemachineFreeLook>();
        //MainVcam = GetComponent<CinemachineVirtualCamera>();
        //Cursor.lockState = CursorLockMode.Locked; // �̰� esc ������ Ŀ�� �ٽ� ��Ÿ�� // �����̵��� rotation.y ����
    }

    void Update()
    {

        //var state = MainVcam.State; // ���� : �ó׸ӽ�ī�޶�
        //Quaternion rotation = state.FinalOrientation;
        //Vector3 euler = rotation.eulerAngles;
        //rotationY = euler.y;


        //if (isFighting == true) return; // �������������

        //if (!Input.GetMouseButtonDown(0) && Input.GetMouseButtonDown(1))
        //{
        //    isRotating = true;
        //}
        //if (Input.GetMouseButtonUp(1))
        //{
        //    isRotating = false;
        //}

        //WindowRotation();

    }
    //void WindowRotation()
    //{
    //    if (isRotating == false) return;

    //    var transposer = MainVcam.GetCinemachineComponent<CinemachineTransposer>();

    //    xRotateMove = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
    //    yRotateMove = -Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime; // y���̵��� ���� �ø����� -������ ������

    //    //xRotate = transposer.m_FollowOffset.x + xRotateMove;
    //    //yRotate = transposer.m_FollowOffset.y + yRotateMove;

    //    transposer.m_FollowOffset = Quaternion.Euler(yRotateMove, xRotateMove, 0) * transposer.m_FollowOffset;

    //    //xRotate = Mathf.Clamp(xRotate, -180f, 180f); // ���� �ڿ������� ���� �̵��̾����� ���ڴ�. ���ݹۿ� �Ȱ��� ���̴�. ���� ���� �ʿ�
    //    //yRotate = Mathf.Clamp(yRotate, -90f, 90f); 



    //    //transform.position = new Vector3(transform.position.x, transform.position.y, z);


    //    //transposer.m_FollowOffset = new Vector3(xRotate, yRotate, -4);




    //}


    // �ִ� �ּҰ� ����
    // �׷��� �� pitch�� x�࿡ ����? �����̵��� euler������ x�࿡�� �Ͼ�� ��
    // �� �ƴµ�... ���� Ŭ������ ��� �� �ι�° ����°�� �ּ� �ִ밪�̱���!
    // ���� ���� // ���η� ȸ��
    // ���� ���� // ���η� ȸ��

    //transposer.m_FollowOffset = Quaternion.Euler(yRotateMove, xRotateMove, 0) * transposer.m_FollowOffset; ���� �ڵ�
    // �� �� �ڿ������� ���� �̵��� ���ؼ� �θ� ������Ʈ�� �߽����� ����༭ ȸ������ �� ������Ʈ�� ����� �ڽİ�ü�� ������ ī�޶� �־ 
    // �����ͷ� �����̰� �ϸ� �������� �������� ū �Ÿ��� ������. ���� �Ÿ����� ȸ������ �ַ���
    // ī�޶� �����ǿ� ȸ������ ���ؼ� ȸ������ �����̰� �ؾ���
    // �ٷ� Ȯ�� ��� �ϴ� ��� �߰��� ��
    // �÷��̾��� ���鿡�� ���Ʒ��� �����϶� �¿�� �����̴� ������ �����ϰ�����. �׷��� ���� 3���� ������ �ۼ��� �ڵ带 �ּ�ó����
    // Quaternion.Euler(yRotateMove, xRotateMove, 0)�� ����Ͽ� ȸ�� ������ ������ ��, xRotateMove�� yRotateMove ���� ȥ���ؼ� ����ϰ� �ֱ� ����.
    // Quaternion.Euler�� ����Ͽ� ȸ���� �����ϴ� ���, X�� Y ȸ���� ���� �ٷ�� ���� �� �������̰� ���ϴ� ����� ���� �� ����
}
