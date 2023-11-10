using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Inventory.Model.ItemSO;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class EdibleItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        [SerializeField] List<ModifierData> modifierData = new List<ModifierData>(); 
                                                                                                                    // modifirerData에 대한 참조를 여기 스크립트에 저장하지 말것. 상속하는 ItemSO 스크립트로 go
                                                                                                                    // 이 파라미터를 수정하는데 사용할 값도 있어야하고
                                                                                                                    // 아이템에서도 동일한 일을 하기 때문에 다른 구조체가 필요해지기 때문 // 또는 매개변수의 값을 정의할 클래스라서
        public string ActionName => "먹다";

        [field: SerializeField]
        public AudioClip actionSFX { get; private set; }

        public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
            foreach(ModifierData data in modifierData)
            {
                data.statModifier.AffectCharacter(character, data.value);
            }
            return true;
        }

       
    }
    public interface IDestroyableItem
    {

    }

    public interface IItemAction
    {
        public string ActionName { get; }
        public AudioClip actionSFX { get; }
        bool PerformAction(GameObject character, List<ItemParameter> itemState);
    }

    [Serializable]
    public class ModifierData
    {
        public CharacterStatModifierSO statModifier;
        public float value;
    }
}

