using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_NPC : MonoBehaviour // ���ϸ� ��������?
{
    EnvironmentCheck EC;

    Rigidbody rb;
    Animator animator;
    float moveSpeed;

    string npcName;
    public int HP, MaxHP;

    
    // onSurface Check���� SideWalk, Car Road�� ���̾� �����ϰ� sideWalk ���̾� �������� �̵��ϰ� ����� ������ �Ǹ�
    // Ⱦ�ܺ����� ��ȣ�� ���缭 �ǳʴ� �͵� ���� 
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        
    }


}
