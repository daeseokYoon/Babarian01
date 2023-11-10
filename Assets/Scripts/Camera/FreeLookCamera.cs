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

        for (int i = 0; i < freeLook.m_Orbits.Length; i++) // 순서대로 Top, Middle, Bottom
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

        // localRotition의 의미는? // freelook카메라는 줌인 줌아웃 할때 TopRig, MiddleRig, BottomRig의 Radius값을 조정해주면 됨.
        // 아래 두줄은 안먹히는 코드 // 후추수정 일단 UI부터
        //Vector3 cameraDirection = this.transform.localRotation * Vector3.forward; // this말고 카메라 위에 상위 오브젝트를 하나 따로 만들어서 관리하는게 더 좋으려나? 아직 안해봄
        //this.transform.position += cameraDirection * Time.deltaTime * scrollWheel * scrollSpeed;

        //// FOV로 줌인 줌아웃하는 코드 // 화면 이상해짐
        //freeLook.m_Lens.FieldOfView += scrollWheel * scrollSpeed * Time.deltaTime;
        //// 줌 인 아웃 할때 fov로 하지말고 z값이 앞뒤로 가게끔 수정예정
        //freeLook.m_Lens.FieldOfView = Mathf.Clamp(freeLook.m_Lens.FieldOfView, 6.7f, 140f);

        if (Input.GetMouseButton(1))
            return UnityEngine.Input.GetAxis(axis) * sensitivity *Time.deltaTime;
        return 0;
    }

    void Start()
    {
        freeLook = GetComponent<CinemachineFreeLook>();
        //MainVcam = GetComponent<CinemachineVirtualCamera>();
        //Cursor.lockState = CursorLockMode.Locked; // 이거 esc 눌러야 커서 다시 나타남 // 수평이동이 rotation.y 값임
    }

    void Update()
    {

        //var state = MainVcam.State; // 형식 : 시네머신카메라
        //Quaternion rotation = state.FinalOrientation;
        //Vector3 euler = rotation.eulerAngles;
        //rotationY = euler.y;


        //if (isFighting == true) return; // 디버깅디버깅디버깅

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
    //    yRotateMove = -Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime; // y축이동은 위로 올리려면 -값으로 되있음

    //    //xRotate = transposer.m_FollowOffset.x + xRotateMove;
    //    //yRotate = transposer.m_FollowOffset.y + yRotateMove;

    //    transposer.m_FollowOffset = Quaternion.Euler(yRotateMove, xRotateMove, 0) * transposer.m_FollowOffset;

    //    //xRotate = Mathf.Clamp(xRotate, -180f, 180f); // 좀더 자연스러운 시점 이동이었으면 좋겠다. 절반밖에 안가는 중이다. 후추 수정 필요
    //    //yRotate = Mathf.Clamp(yRotate, -90f, 90f); 



    //    //transform.position = new Vector3(transform.position.x, transform.position.y, z);


    //    //transposer.m_FollowOffset = new Vector3(xRotate, yRotate, -4);




    //}


    // 최대 최소값 제한
    // 그런데 왜 pitch가 x축에 들어가지? 수직이동이 euler에서는 x축에서 일어나는 것
    // 은 아는데... 아하 클램프의 요소 중 두번째 세번째가 최소 최대값이구나!
    // 수평 각도 // 가로로 회전
    // 수직 각도 // 세로로 회전

    //transposer.m_FollowOffset = Quaternion.Euler(yRotateMove, xRotateMove, 0) * transposer.m_FollowOffset; 문제 코드
    // 좀 더 자연스러운 시점 이동을 위해서 부모 오브젝트로 중심점을 잡아줘서 회전값만 줄 오브젝트로 만들고 자식개체에 고정된 카메라를 넣어서 
    // 뉴벡터로 움직이게 하면 포지션이 움직여서 큰 거리로 움직임. 일정 거리에서 회전값만 주려면
    // 카메라 포지션에 회전값만 곱해서 회전값만 움직이게 해야함
    // 휠로 확대 축소 하는 기능 추가할 것
    // 플레이어의 측면에서 위아래로 움직일때 좌우로 움직이는 현상을 수정하고자함. 그래서 위의 3줄의 이유로 작성된 코드를 주석처리함
    // Quaternion.Euler(yRotateMove, xRotateMove, 0)를 사용하여 회전 각도를 적용할 때, xRotateMove와 yRotateMove 값을 혼합해서 사용하고 있기 때문.
    // Quaternion.Euler를 사용하여 회전을 적용하는 대신, X와 Y 회전을 따로 다루는 것이 더 직관적이고 원하는 결과를 얻을 수 있음
}
