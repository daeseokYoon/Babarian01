using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    //[CreateAssetMenu]//(menuName = "ItemSO/�׸��̸�?")]
    public abstract class ItemSO : ScriptableObject                                                // �׸�(item) SO(Scriptable Object)
    {
        [field: SerializeField]                                                                  // �Ϲ� [SerializeField]�� ������ : �����͸� ����ȭ ����, �ڵ������� ������Ƽ���� ��� ����(���ٷ� ������ Getter ������Ƽ ����Ұ�) 2020.2���� ������ ����                     
        public bool IsStackable { get; set; }
        public int ID => GetInstanceID(); // �����ϰ� ID�� �����ϴ� ����Ƽ �ڵ�

        [field: SerializeField]
        public int MaxStackSize { get; set; } = 1;                                                                      // ������ ���� �� �ִ� ������Ʈ(ex �Ѿ�)�� �ƴ� �̻� 1�� ������Ű�� ����

        [field: SerializeField]
        public string Name { get; set; }
        
        [field: SerializeField]
        [field: TextArea]
        public string Description { get; set; }

        [field: SerializeField]
        public Sprite ItemImage { get; set; }
        
        [field: SerializeField]
        public List<ItemParameter> DefaultParametersList { get; set; }                                               // �̰� �κ��丮�� �������̽� ���ο� ���Խ�ų ����
                                                                                                                   // missing ���°� �ƴ� None���·� �۵��Ѵ�. ��ȿ�� ������ �������� �ʴ� ���� �� ������ �߻���Ű�� ���� 
                                                                                                                   // �׷��� None ������ ������Ʈ�� ��ü�� �����Ϸ��� �Ѵٸ� ������ ���� �� ����.
                                                                                                                   // ���ϼ� �Ǵ��� ���� ���� �⺻ �Ķ���� List

        [Serializable]
        public struct ItemParameter : IEquatable<ItemParameter>                                                     // ���ϼ� �Ǵ� �������̽� IEquatable �پ��� �÷��ǿ� Ȱ��
        {
            public ItemParameterSO itemParameter;                                                                   // �������Ķ���� ������ ����Ͱ� �����Ѱ�? ����ü�� �ִ��� Ȯ��
            public float value;

            public bool Equals(ItemParameter other)                                                                  // ���ϼ� �Ǵ� �޼ҵ�
            {
                return other.itemParameter == itemParameter;
            }
        }
    }
}