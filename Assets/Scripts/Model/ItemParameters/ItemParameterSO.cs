using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class ItemParameterSO : ScriptableObject
    {
        [field: SerializeField]
        public string ParameterName {  get; private set; } 
        // 보통 파라미터는 SO 개체 내에 이름을 지정하면 확신할 수 있는 이름이 하나뿐이고 개체 참조가 하나만 있는 경우
        // 이 유형의 파라미터만 있고 descriptible 개체(오브젝트) 참조를 주면 매개변수를 비교해 일치하는 걸 확인가능
        // string 어떤 유형의 매개변수인지 비교확인이 가능하다!
        // 지금은 enum으로 분류를 안하고 SO로 구분중
    }
}