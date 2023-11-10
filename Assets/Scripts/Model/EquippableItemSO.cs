using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class EquippableItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        public string ActionName => "입다";

        public AudioClip actionSFX {get; private set;}

        public bool PerformAction(GameObject character, List<ItemParameter> itemState) // 작업 결과 확인 // 성공 실패 팝업
        {
            AgentWeapon weaponSystem = character.GetComponent<AgentWeapon>();
            if(weaponSystem != null)
            {
                // null 상태면 아이템 상태, 
                weaponSystem.SetWeapon(this, itemState == null ? DefaultParametersList : itemState);
                return true; 
            }
            return false;
        }
    }

}