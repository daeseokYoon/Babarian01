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
                                                                                                                    // modifirerData�� ���� ������ ���� ��ũ��Ʈ�� �������� ����. ����ϴ� ItemSO ��ũ��Ʈ�� go
                                                                                                                    // �� �Ķ���͸� �����ϴµ� ����� ���� �־���ϰ�
                                                                                                                    // �����ۿ����� ������ ���� �ϱ� ������ �ٸ� ����ü�� �ʿ������� ���� // �Ǵ� �Ű������� ���� ������ Ŭ������
        public string ActionName => "�Դ�";

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

