using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dialog : MonoBehaviour // 이 클래스의 목적 : 대화 후 player에게 아이템을 주거나 스토리아이템, 퀘스트를 트리거하기 위함 // SO 느낌으로 존재하는 중 대체하려함
{
    [SerializeField] List<string> lines; // 대사가 출력되는 인스턴스인데 List로 뽑아내는 것은 별로인가? Queue로 만들어서 대사를 기록하고... 그런데 이걸 각 Npc에 두는게 맞나? 똑같은 대사만 뱉을 거면 상관없는데

    public List<string> Lines => lines;
}
