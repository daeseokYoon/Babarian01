using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_NPC : MonoBehaviour // 포켓몬 개별변수?
{
    EnvironmentCheck EC;

    Rigidbody rb;
    Animator animator;
    float moveSpeed;

    string npcName;
    public int HP, MaxHP;

    
    // onSurface Check말고 SideWalk, Car Road랑 레이어 구분하고 sideWalk 레이어 위에서만 이동하게 만들고 여력이 되면
    // 횡단보도와 신호등 맞춰서 건너는 것도 구현 
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        
    }


}
