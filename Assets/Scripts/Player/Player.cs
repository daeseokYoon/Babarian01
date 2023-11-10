using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    // NPC ���� �Ÿ� ���ٽ� ��ȣ�ۿ��ư ���� > ��ư ������ ������ ���� (1. ��ȭ 2. ���� 3. �� ) > �� ���������� ī�޶� �̵�
    // 1. ��ȭ dialog UI 2. �� ��ȯ 3D ���ϸ� �ο�

    [Header("Player Movement")]
    public float movementSpeed = 5f;
    public EnvironmentCheck environmentCheck;
    public float rotSpeed;
    public Transform MainCameraPos;
    public CinemachineFreeLook freelook;
    bool isJumping = false; // ������ ������..!
    bool getItem;
    bool setItem0;
    bool setItem1;

    bool playerControl = true; // �ٸ� Ʈ���Ű� �۵��Ҷ� �ٸ� �ִϸ��̼��� ���۽�Ű������ �÷���

    [Header("Player Animator")]
    public Animator animator;

    [Header("Player Collision & Gravity")]
    public CharacterController CC;
    public float surfaceCheckRadius = 0.3f;
    public Vector3 surfaceCheckOffset; // transform.TransformPoint(surfaceCheckOffset); => ������Ʈ�� ��ġ�� ���ο� Vector3 ���� ����
    public LayerMask surfaceLayer;
    bool onSurface; 
    [SerializeField] float fallingSpeed;
    [SerializeField] Vector3 moveDir;
    [SerializeField] Vector3 requiredMoveDir; // ������� player�ȿ� lookPoint ������Ʈ�� ��ǻ� ������(�θ������Ʈ�� ��ٴ���, follow point�� ��ٴ���)
                                              // ī�޶��� ȸ������ ���ͼ� moveDir�� ��Ƽ� velocity�� �� ��Ƽ� Ȱ���߾���. 
    Vector3 velocity;
    GameObject nearObject; // ������ �������� �����ϱ� ���� ����
    GameObject equipWeapon;
    public GameObject[] weapons; // false�Ǿ��ִ� ���� ������
    public bool[] hasWeapons; // ��� ������ �ִ��� ����

    public int ammo;
    public int maxAmmo;
    public int health;
    public int maxHealth;
    public float fireDelay;
    public float isFireReady;

    private void Update()
    {
        getItem = Input.GetButtonDown("Interaction"); // ��ư E
        setItem0 = Input.GetButtonDown("Swap0");
        setItem1 = Input.GetButtonDown("Swap1");
        Interation(); // ��ȣ�ۿ�
        Swap(); // ȸ���� ���� ��ü // �κ���� ����

        //EnvironmentCheck.ItemInfo hitData = environmentCheck.CheckItem();
        //if (hitData.hitFound)
        //{
        //    Debug.Log("������");
        //} //�����غ��ϱ� ���� �������̶� �浹�ϴµ� ���̾�� �ε�ĥ �ʿ䰡 �ִ°�? OnTrigger�� �ݶ��̴��� �پ��ִµ�?
    }

    private void Swap()
    {
        
        int weaponIndex = -1;
        if (setItem0) weaponIndex = 0;
        if (setItem1) weaponIndex = 1;

        if (setItem0 && hasWeapons[0] == false)
        {
            Debug.Log("�� ���� (swap0)");
            return;
        }

        if (setItem1 && hasWeapons[1] == false)
        {
            Debug.Log("�� ���� (swap1)");
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
        velocity = Vector3.zero; // �ȉ�� ����, surfaceüũ�� �ʹ� �۾Ƽ� �ȉ�� ���� �װź��� �� �׳� �پ������� true�� ���۵ǳİ� ������ �����ϱ� false���� true�� �ǳ�
        //Debug.Log(onSurface);

        if (onSurface)
        {
            fallingSpeed = 0; 
            velocity = moveDir * movementSpeed; // PlayerMovement �ȿ� moveDirecton���� moveDir�� ���� velocity�� ������.
        } 
        else // �ٴ� ������ ���� �� // �ִϸ��̼� ��ü�� �߷� ������ false�� ���� �Ÿ� ����� ��.
        {
            fallingSpeed += Physics.gravity.y * Time.deltaTime;//?
            velocity = transform.forward * movementSpeed / 2;
        }

        velocity.y = fallingSpeed;
        

        animator.SetBool("onSurface", onSurface); // �ٴ� ������ true�� �ɶ����� �������� �ִϸ��̼� ����

        PlayerMovement();
        SurfaceCheck();
    }


    private void PlayerMovement()
    {
        float horizontal = Input.GetAxis("Horizontal"); // �Է� ���� ���� ���
        float vertical = Input.GetAxis("Vertical");

        float movementAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        animator.SetFloat("movementValue", movementAmount, 0.3f, Time.deltaTime);

        //Vector3 movementInput = new Vector3(horizontal, 0, vertical).normalized; // �¿� �յ� �̵�Ű       

        Vector3 cameraForward = MainCameraPos.forward;
        cameraForward.y = 0; // �� �Ʒ� ������ ���� �� ����ȭ�ϱ� ����

        Vector3 moveDirection = cameraForward.normalized * vertical + MainCameraPos.right.normalized * horizontal; // ī�޶� �ٶ󺸴� ������ ĳ������ ������
        //moveDirection.Normalize(); // �̵� ���� ����ȭ
        requiredMoveDir = moveDirection; // 
        moveDir = requiredMoveDir;
        CC.Move(velocity * Time.deltaTime); // ĳ���� ��Ʈ�� �����̴� �Լ�  // velocity�� ���� moveDirection�� ���� ��ü ���� moveDir�� ������� update���� velocty�� ���� ������ ����.

        transform.Rotate(new Vector3(0, horizontal, 0).normalized * rotSpeed * Time.deltaTime);
        if (moveDirection != Vector3.zero) // ����׷α� �ʹ� ���� �س���
            transform.rotation = Quaternion.LookRotation(moveDirection); // ī�޶� �ٶ󺸴� ������ �÷��̾��� �����̼ǿ� ���

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
// ��Ʋ�� �� ����� �÷��̾� �ִϸ��̼� ��ũ��Ʈ �ʿ���.

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

// Panel�� �������� ũ�� ���̰� ���϶� ���빰�� �������
