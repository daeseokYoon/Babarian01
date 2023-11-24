using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PersonalityTrait // 안쓰게 되었지만 자료덩어리를 만들어봐야함
{
    public string[] traitName = {"항목0", "항목1", "항목2", "항목3"};
    public int[] value; // = new int[4]; // 배열을 직접 초기화. 변동 안할거라서 고정된 배열을 사용함
    // 배열, 리스트 장단점에 따른 선택 // Dictionary를 썼다면 더 코드가 간결해졌을 것이다.
    // public Dictionary<string, int> traitValues = new Dictionary<string, int>();
}
