using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Inventory.Model.ItemSO;

public class AgentWeapon : MonoBehaviour
{
    // 수정가능한 객체의 참조와 변경할 수 있는 아이템 파라미터의 참조 List
    [SerializeField] EquippableItemSO weapon;
    [SerializeField] InventorySO inventoryData; // 장착 해제 도움참조 변수
    [SerializeField] List<ItemParameter> parametersToModify, itemCurrentState;
    
    public void SetWeapon(EquippableItemSO weaponItemSO, List<ItemParameter> itemState) // 무기 아이템, 아이템상태
    {
        if(weapon != null)
        {
            inventoryData.AddItem(weapon, 1, itemCurrentState);
        }

        this.weapon = weaponItemSO;
        this.itemCurrentState = new List<ItemParameter>(itemState); // 아이템 파라미터의 new list를 생성하는데 필요한 구조체 List 전달
        ModifyParameters(); // 그리고 위에서 얻은 list를 새로운 list에 복사
    }

    private void ModifyParameters()
    {
        foreach(var parameter in parametersToModify) // list의 파라미터들 var로 
        {
            if(itemCurrentState.Contains(parameter)) // 구조체 안에 해당 파라미터가 있는지 확인하는 메소드
            {
                int index = itemCurrentState.IndexOf(parameter); // 아이템의 파라미터 index
                float newValue = itemCurrentState[index].value + parameter.value; // 기존 값 + 변화 값
                itemCurrentState[index] = new ItemParameter // 변화된 값 수정 // 구조체 기본값은 변화 없음 새로운 카피일 뿐
                {
                    itemParameter = parameter.itemParameter,
                    value = newValue,
                };
            }
        }
    }
}
