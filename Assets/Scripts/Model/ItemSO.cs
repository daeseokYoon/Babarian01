using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    //[CreateAssetMenu]//(menuName = "ItemSO/항목이름?")]
    public abstract class ItemSO : ScriptableObject                                                // 항목(item) SO(Scriptable Object)
    {
        [field: SerializeField]                                                                  // 일반 [SerializeField]와 차이점 : 데이터만 직렬화 가능, 자동구현된 프로퍼티에만 사용 가능(람다로 구현된 Getter 프로퍼티 적용불가) 2020.2이후 버전만 가능                     
        public bool IsStackable { get; set; }
        public int ID => GetInstanceID(); // 간편하게 ID를 참조하는 유니티 코드

        [field: SerializeField]
        public int MaxStackSize { get; set; } = 1;                                                                      // 스택을 쌓을 수 있는 오브젝트(ex 총알)이 아닌 이상 1로 고정시키기 위함

        [field: SerializeField]
        public string Name { get; set; }
        
        [field: SerializeField]
        [field: TextArea]
        public string Description { get; set; }

        [field: SerializeField]
        public Sprite ItemImage { get; set; }
        
        [field: SerializeField]
        public List<ItemParameter> DefaultParametersList { get; set; }                                               // 이거 인벤토리와 인터페이스 내부에 포함시킬 예정
                                                                                                                   // missing 상태가 아닌 None상태로 작동한다. 유효한 참조가 설정되지 않는 것일 뿐 에러를 발생시키지 않음 
                                                                                                                   // 그러나 None 상태의 컴포넌트나 객체를 참조하려고 한다면 문제가 생길 수 있음.
                                                                                                                   // 동일성 판단을 위해 만든 기본 파라미터 List

        [Serializable]
        public struct ItemParameter : IEquatable<ItemParameter>                                                     // 동일성 판단 인터페이스 IEquatable 다양한 컬렉션에 활용
        {
            public ItemParameterSO itemParameter;                                                                   // 아이템파라미터 변수가 여기것과 동일한가? 구조체가 있는지 확인
            public float value;

            public bool Equals(ItemParameter other)                                                                  // 동일성 판단 메소드
            {
                return other.itemParameter == itemParameter;
            }
        }
    }
}