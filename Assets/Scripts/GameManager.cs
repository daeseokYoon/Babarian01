using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour // 데이터 구조 조립 조립 조립 조립 조립 조립 조립 조립 조립 조립 조립 조립 조립 조립 조립
{
    static GameManager instance;
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // GameManager가 단 하나, 유일한 객체로 남기기
        if(instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
       
    }

    

    List<PersonalityTrait> playerTraits = new List<PersonalityTrait>();

    public List<PersonalityTrait> PlayerTrait() => playerTraits; // 결과창 숫자 출력

    public void AddTraitValue(string traitName) // 넣을 특성과 더할 값
    {
        // int 사용시 12345 라는 리스트 내용물이 있을 때 x> 2 면 find로 int를 찿을때 3이 나온다. 345 3개 이기 때문 x> 5면 값이 없어서 0을 반환 // 링큐 any를 활용해 찾기, Array.IndexOf로 찾기
        PersonalityTrait trait = playerTraits.Find(t => t.traitName.Contains(traitName)); 
        if (trait != null ) // 이름이 있으면 해당 이름의 값에 값 추가
        {
            //if(trait.value == null) // trait에서 직접 초기화 했기때문에 지워줌. // 고정된 초기화 값이 없었기 때문에 null일 경우 새로운 값을 추가 해줬으야 했음.
            //{
            //    trait.value = new int[trait.traitName.Length];
            //}
            
            int index = Array.IndexOf(trait.traitName, traitName); // (상자, 찾는 value)

            if(index !=  -1) // IndexOf는 찾는 값이 없을때 -1을 반환함.
            {
                trait.value[index] += 1; // 파라미터 두개가 들어가면 버튼이 작동을 안하나?
            }
        }
        //else // 없으면 새롭게 이름을 넣고 값도 추가
        //{
        //    playerTraits.Add(new PersonalityTrait { traitName = traitname, value = value }); // 딕셔너리마냥 넣는 모양새 // FindIndex, Find 함수 사용법 검색 // 이거 미리 스트링 지정으로 바꿔줘야함
        //}

        //PersonalityTrait trait = playerTraits.Find(t => t.traitValues.ContainsKey(traitName)); // Dictionary를 사용했을 때 코드

        //if (trait != null)
        //{
        //    trait.traitValues[traitName] += valueToAdd;
        //}
    }

    public int EachTotalValue(int index)
    {
        return playerTraits[index].value[index];
    }
   

    //public int CalculateEachValue()
    //{
    //    int eachValue = 0; // 전체 값
    //    foreach(var trait in playerTraits) // 리스트에 있는 값을 불러옴 (딕셔너리처럼 쓰인 건가? 조금 다르다 왜냐면 위 리스트에 든 클래스는 데이터상자이기 때문)
    //    {
    //        eachValue += trait.value; // 
    //    }
    //    return eachValue;
    //}


}
