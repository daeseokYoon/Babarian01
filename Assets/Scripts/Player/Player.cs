using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    // NPC 일정 거리 접근시 상호작용버튼 생성 > 버튼 누르면 선택지 나옴 (1. 대화 2. 전투 3. 퇴각 ) > 각 선택지마다 카메라 이동
    // 1. 대화 dialog UI 2. 씬 전환 3D 포켓몬 싸움

    [Header("Player Movement")]
    public float movementSpeed = 5f;
    public EnvironmentCheck environmentCheck;
    public float rotSpeed;
    public Transform MainCameraPos;
    public CinemachineFreeLook freelook;
    bool isJumping = false; // 점프는 보류다..!
    bool getItem;
    bool setItem0;
    bool setItem1;

    bool playerControl = true; // 다른 트리거가 작동할때 다른 애니메이션을 동작시키기위한 플래그

    [Header("Player Animator")]
    public Animator animator;

    [Header("Player Collision & Gravity")]
    public CharacterController CC;
    public float surfaceCheckRadius = 0.3f;
    public Vector3 surfaceCheckOffset; // transform.TransformPoint(surfaceCheckOffset); => 오브젝트의 위치에 새로운 Vector3 값을 생성
    public LayerMask surfaceLayer;
    bool onSurface; 
    [SerializeField] float fallingSpeed;
    [SerializeField] Vector3 moveDir;
    [SerializeField] Vector3 requiredMoveDir; // 원래라면 player안에 lookPoint 오브젝트에 사실상 합쳐진(부모오브젝트가 됬다던가, follow point가 됬다던가)
                                              // 카메라의 회전값을 따와서 moveDir에 담아서 velocity에 또 담아서 활용했었음. 
    Vector3 velocity;
    GameObject nearObject; // 감지된 아이템을 저장하기 위한 변수
    GameObject equipWeapon;
    public GameObject[] weapons; // false되어있는 무기 프리팹
    public bool[] hasWeapons; // 장비를 가지고 있는지 유무

    public int ammo;
    public int maxAmmo;
    public int health;
    public int maxHealth;
    public float fireDelay;
    public float isFireReady;

    private void Update()
    {
        getItem = Input.GetButtonDown("Interaction"); // 버튼 E
        setItem0 = Input.GetButtonDown("Swap0");
        setItem1 = Input.GetButtonDown("Swap1");
        Interation(); // 상호작용
        Swap(); // 회수한 무기 교체 // 인벤사용 안함

        //EnvironmentCheck.ItemInfo hitData = environmentCheck.CheckItem();
        //if (hitData.hitFound)
        //{
        //    Debug.Log("감지됨");
        //} //생각해보니까 굳이 아이템이랑 충돌하는데 레이어로 부딪칠 필요가 있는가? OnTrigger는 콜라이더에 붙어있는데?
    }

    private void Swap()
    {
        
        int weaponIndex = -1;
        if (setItem0) weaponIndex = 0;
        if (setItem1) weaponIndex = 1;

        if (setItem0 && hasWeapons[0] == false)
        {
            Debug.Log("템 없음 (swap0)");
            return;
        }

        if (setItem1 && hasWeapons[1] == false)
        {
            Debug.Log("템 없음 (swap1)");
            return;
        }

        if (setItem0 || setItem1)
        {
            if(equipWeapon != null)
                equipWeapon.SetActive(false);

            equipWeapon = weapons[weaponIndex];
            equipWeapon.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        velocity = Vector3.zero; // 안됬던 이유, surface체크가 너무 작아서 안됬던 것임 그거보다 왜 그냥 붙어있으면 true로 시작되냐고 위에서 떨구니까 false에서 true가 되네
        //Debug.Log(onSurface);

        if (onSurface)
        {
            fallingSpeed = 0; 
            velocity = moveDir * movementSpeed; // PlayerMovement 안에 moveDirecton값이 moveDir에 들어가서 velocity가 성립됨.
        } 
        else // 바닥 판정이 없을 때 // 애니메이션 자체의 중력 적용을 false로 해줄 거면 사용할 것.
        {
            fallingSpeed += Physics.gravity.y * Time.deltaTime;//?
            velocity = transform.forward * movementSpeed / 2;
        }

        velocity.y = fallingSpeed;
        

        animator.SetBool("onSurface", onSurface); // 바닥 판정이 true가 될때까지 떨어지는 애니메이션 실행

        PlayerMovement();
        SurfaceCheck();
    }


    private void PlayerMovement()
    {
        float horizontal = Input.GetAxis("Horizontal"); // 입력 감지 숫자 담기
        float vertical = Input.GetAxis("Vertical");

        float movementAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        animator.SetFloat("movementValue", movementAmount, 0.3f, Time.deltaTime);

        //Vector3 movementInput = new Vector3(horizontal, 0, vertical).normalized; // 좌우 앞뒤 이동키       

        Vector3 cameraForward = MainCameraPos.forward;
        cameraForward.y = 0; // 위 아래 움직임 통제 후 정규화하기 위함

        Vector3 moveDirection = cameraForward.normalized * vertical + MainCameraPos.right.normalized * horizontal; // 카메라가 바라보는 방향이 캐릭터의 정방향
        //moveDirection.Normalize(); // 이동 방향 정규화
        requiredMoveDir = moveDirection; // 
        moveDir = requiredMoveDir;
        CC.Move(velocity * Time.deltaTime); // 캐릭터 컨트롤 움직이는 함수  // velocity는 위에 moveDirection을 담은 전체 변수 moveDir에 담겨져서 update문에 velocty로 들어가서 관리될 것임.

        transform.Rotate(new Vector3(0, horizontal, 0).normalized * rotSpeed * Time.deltaTime);
        if (moveDirection != Vector3.zero) // 디버그로그 너무 떠서 해놓음
            transform.rotation = Quaternion.LookRotation(moveDirection); // 카메라가 바라보는 방향을 플레이어의 로테이션에 담기

    }

    void SurfaceCheck()
    {
        onSurface = Physics.CheckSphere(transform.TransformPoint(surfaceCheckOffset), surfaceCheckRadius, surfaceLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.TransformPoint(surfaceCheckOffset), surfaceCheckRadius);
    }

    void Interation()
    {
        if(getItem && nearObject != null)
        {
            if(nearObject.tag == "Weapon")
            {
                Field_Item item = nearObject.GetComponent<Field_Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;

                Destroy(nearObject.gameObject);
            }
        }
    }

    void Attack()
    {
        if (equipWeapon == null) return;

        fireDelay += Time.deltaTime;
        //isFireReady = equipWeapon.rate < fireDelay;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item")
        {
            Field_Item item = other.GetComponent<Field_Item>();
            switch (item.type)
            {
                case Field_Item.Type.Ammo:
                    ammo += item.value;
                    if (ammo > maxAmmo) ammo = maxAmmo;
                    break;
                case Field_Item.Type.Heart:
                    health += item.value;
                    if (health > maxHealth) health = maxHealth;
                    break;
            }
            Destroy(other.gameObject);
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.gameObject.tag == null) { return; }

        if (other.gameObject.CompareTag("Weapon"))
            nearObject = other.gameObject;
        //if (nearObject == null) return;
        //Debug.Log(nearObject.name);
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Weapon"))
            nearObject = null;
    }

}
// 배틀할 때 사용할 플레이어 애니메이션 스크립트 필요함.

//if (Input.GetButtonDown("Jump") && isJumping == false && onSurface)
//{
//    velocity.y = 100;
//    isJumping = true;
//    freelook.Follow = null;
//    animator.CrossFade("EagleJumping", 0.2f);
//}
//if (!onSurface && isJumping)
//{
//    freelook.Follow = null;
//    //Debug.Log(onSurface);
//}
//if (onSurface && isJumping)
//{
//    freelook.Follow = CC.transform;
//    isJumping = false;
//}

// Panel로 만들어야지 크기 줄이고 늘일때 내용물에 영향없음
