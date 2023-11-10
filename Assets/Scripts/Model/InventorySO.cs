using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Loading;
using UnityEngine;
using static Inventory.Model.ItemSO;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class InventorySO : ScriptableObject                                                                  // �κ��丮 ��ü�� �ڽ��� ������ �ִ� �׸� ���� �˾ƾ��� 
                                                                                                                // SO�� ����Ͽ� �κ��丮�� �����ϴ� ���� : �ν����Ϳ��� ����Ʈ�� Ȯ�尡�� > ����Ʈ�� �ִ� ���빰�� ������ Ȯ�ΰ���

    {
        [SerializeField]
        List<InventoryItem> inventoryItems;                                                             // ������ ��Ͽ� ���� ������ ��� ���� ���� ����� ��ü ����� ��ΰ����ͼ�
                                                                                                         // �κ��丮 ��Ʈ�ѷ��� �����ϰ� �̹������ UI ������Ʈ�� �Ϸ��� �κ��丮 ��Ʈ�ѷ��� �� ��Ͽ� �׼����ؼ� ������ �����ϰ� ����
                                                                                                         // list�� �������� > list�� �ִ� ���� ������ �����ϰ� �츮�� ����!

        [field: SerializeField] public int Size { get; private set; } = 10;                                         // � Ŭ���������� �������� ũ�Ⱑ �����Ǵ� ���� ������ ����

        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;                                         // �븮�ڷ� ��ųʸ��� �κ� ���������� ���� // ����Ʈ�� �ε����� key�� �κ� ������ ���� ã��

        public void Initialize()
        {
            inventoryItems = new List<InventoryItem>();
            for (int i = 0; i < Size; i++)
            {
                inventoryItems.Add(InventoryItem.GetEmptyItem());
            }
        }

        public int AddItem(ItemSO item, int quantity, List<ItemParameter> itemState = null)
        {
                                                                                                                        // ������ �۵��� �ڵ尡 �۵��ؼ� �̹� ����� ��ũ��Ʈ���� ���� ������ ������ �ʿ���� ���� �ʿ�����
            if(item.IsStackable == false)
            {
                for (int i = 0; i < inventoryItems.Count; i++)                                                              // �κ��丮 ũ�� ��ĵ�ѹ� �ϰ�
                {
                    while(quantity > 0 && IsInventoryFull() == false)
                    {
                        quantity -= AddItemToFirstFreeSlot(item, 1, itemState);
                                                                                                                                //quantity --; // �� �Լ��� int�� ��ȯ�ҰŶ�
                    }
                    InformAboutChange();
                    return quantity;
                }
            }
            quantity = AddStackableItem(item, quantity);                                                                    // �κ��� ���������� ������ �߰��� �߰��� �������� ������Ʈ�� ����
                                                                                                                            // �̹� ������ �Լ� ���濡 ���� �����˸��� ȣ��
            InformAboutChange();
            return quantity;
           
        }

        private int AddItemToFirstFreeSlot(ItemSO item, int quantity, List<ItemParameter> itemState = null)                              // ������� ã���� �ƴ϶� int��
        {
            InventoryItem newItem = new InventoryItem
            {
                item = item,
                quantity = quantity,
                itemState = new List<ItemParameter>(itemState == null ? item.DefaultParametersList : itemState)
                                                                                                                                    // ����ü�� �� ��Ͽ� ���� // ���⼭�� �κ��丮�� ����Ǵ� ������ ���̶�
            };
            for(int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                {
                    inventoryItems[i] = newItem;
                    return quantity;
                }
            }
            return 0;
            //if (inventoryItems[i].IsEmpty) // �κ��丮�� ����ִ� ����
            //{
            //    inventoryItems[i] = new InventoryItem // ���� �������� �ִ´� // ���ο� ���� �ִ°���.
            //    {
            //        item = item,
            //        quantity = quantity,
            //    };
            //    return; // �� �׸��� ã�� ��ȯ
            //}
        }

        private bool IsInventoryFull() => inventoryItems.Where(item => item.IsEmpty).Any() == false;
                                                                                                                            // �κ� ������ ��Ͽ� ä�����մ� �������� �ϳ� �̻� �ִٸ� true �ƴϸ� false // where / linq

        private int AddStackableItem(ItemSO item, int quantity)                                                             // �ִ� �뷮�� 99�̰� ���� ������ 98���϶� 1���� �� ������ �� �ְ��ϴ� �ڵ�
        {
            for(int i =0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty) continue;
                if (inventoryItems[i].item.ID == item.ID)
                {
                    int amountPossibleToTake = inventoryItems[i].item.MaxStackSize - inventoryItems[i].quantity;                // ������ �ִ� ���� - �κ������� �ڸ� �� = ���� ����

                    if(quantity > amountPossibleToTake)                                                                          // ������ ������ �� �ִ� �������� ������ (���� �纸�� ���Կ� �� �������� �� ���ٴ� ��)
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].item.MaxStackSize);           // �κ��丮 ������ â �� ä����
                        quantity -= amountPossibleToTake;                                                                                // ���⼭ ������ �������� ���� �� // ���� ���� ��ǰ Ȥ�� ���ο� �κ��丮 â�� �߰�
                    }
                    else
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].quantity + quantity);             // �κ��丮 ������ â �߰�
                        InformAboutChange();
                        return 0;                                                                                                // ���� �� ���� ���� �κ��� �־���.
                    }
                }
            }
            while(quantity > 0 && IsInventoryFull() == false)                                                       // ������ ������ 0���� ū �����̸鼭 �κ��� Ǯ�� �ƴ� ���� quantity ������ �����ϰ� ���� ���� �ڸ�(����)�� ��ȯ
            {
                int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize);
                quantity -= newQuantity;
                AddItemToFirstFreeSlot(item, newQuantity);                                                              // �� �׸��� �����ϰ� ù ��° �� ���Կ� �߰��ϱ� ������
            }
            return quantity;
        }

                                                                                                                             //�κ��丮�� null�� �� �� ������ ���� ���� �����ϵ��� �κ��丮�� ������������. �Ʒ��� �ȉ�� �͵�
                                                                                                                             //InventoryItem item = new InventoryItem();
                                                                                                                             //int x = null;
                                                                                                                             //InventorySO val = null;

        public Dictionary<int, InventoryItem> GetCurrentInventoryState()                                                     // ��� �������� ä������ ������ �Ϻ� �׸��� ����ִٰ� �� ��. ��� �������� ������Ʈ �� �ʿ����
                                                                                                                             // ��ųʸ��� Key������ �ε����� ���޵��� ������ �� �׸��� �ڵ����� ����ִٰ� ������ �� ���� 
                                                                                                                             // ���� ���ϴ� �׸� ������Ʈ�ϰ� ������ �׸��� UI�� ������Ʈ ���� ����ä�� �д�.
        {
            Dictionary<int, InventoryItem> returnValue = new Dictionary<int, InventoryItem>();
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty) continue;
                returnValue[i] = inventoryItems[i];
            }
            return returnValue;
        }

        public InventoryItem GetItemAt(int itemIndex)                                                                // �� ������â�� ��ȯ�� ���� ������ �Ʒ� ChangeQuantity���� �� �������� ������
        {
            return inventoryItems[itemIndex];
        }

        public void AddItem(InventoryItem item)                                                                         // namespace�� ���� �̸��� ����� �� �ֱ� �ѵ� �̷��� �ϴ� ������? 
        {
            AddItem(item.item, item.quantity);                                                                           // ó���� ����Ǿ��ִ� �⺻ ���� �ҷ����� �޼ҵ�(����ü�� ����� ������ �ҷ����� ����)
        }

        public void SwapItems(int itemIndex_1, int itemIndex_2)
        {
            InventoryItem item1 = inventoryItems[itemIndex_1];                                                               // ù��° �������� �������� ������
            inventoryItems[itemIndex_1] = inventoryItems[itemIndex_2];
            inventoryItems[itemIndex_2] = item1;
            InformAboutChange();                                                                                             // �κ��丮 ������ �����ǰ� ��������� �ִٴ� ���� �˾ƾ��� // �׳� ���� ������ ���� �ٲٴ� �޼ҵ� �ƴ�
                                                                                                                            // UI���� �����͸� �����ϴ°� �ƴ϶� �κ��丮 ��Ʈ�ѷ��� UI�� ������
        }

        private void InformAboutChange()                                                                                     // ���� �κ��丮�� ���� �����͸� ����
        {
            OnInventoryUpdated?.Invoke (GetCurrentInventoryState());
        }

        public void RemoveItem(int itemIndex, int amount)
        {
            if(inventoryItems.Count > itemIndex)
            {
                if (inventoryItems[itemIndex].IsEmpty) return;
                int reminder = inventoryItems[itemIndex].quantity - amount;
                if(reminder <= 0)
                {
                    inventoryItems[itemIndex] = InventoryItem.GetEmptyItem();
                }
                else
                {
                    inventoryItems[itemIndex] = inventoryItems[itemIndex].ChangeQuantity(reminder);
                }
                InformAboutChange ();
            }
        }
    }



    [Serializable]
    public struct InventoryItem                                                                     // �ٸ� ������ class�� �ִ� �κ��丮������ ��ũ��Ʈ�� �����ؼ� ���������
                                                                                                    // �ٸ� ��ũ��Ʈ���� ���� ������ �Ͼ ��� ���� �κ��丮�� ���װ� ������ �ʵ��� �޸𸮸� �ٸ� ������ �����ϱ����� ����ü�� ����� 
    {
        public int quantity;
        public ItemSO item;
        public List<ItemParameter> itemState;
        public bool IsEmpty => item == null;

        public InventoryItem ChangeQuantity(int newQuantity)
        {
            return new InventoryItem                                                                             // �������̳� �κ��� ������ ���� ������ �� ������ ���ο� �׸��� ������ش�. �ʱⰪ�� ���� ����(���������� �ƴϱ⶧��)
            {
                item = this.item,
                quantity = newQuantity,                                                                                      // �ٸ� �Ӽ��� �߰��Ҷ� �� �����ϱ� ���ٴ� ����??
                itemState = new List<ItemParameter>(this.itemState)                                                      // itemparameter�� ���� ������ �� ��� ����ü�� ��
            };
        }
        public static InventoryItem GetEmptyItem()
            => new InventoryItem
            {
                item = null,
                quantity = 0,
                itemState = new List<ItemParameter>()
            };
    }
}


// ����ü�� �����Ϳ� ���� ����� ĸ��ȭ�� �� �ִ� �� ���� > Ŭ������ �޸� ����� �Ұ����ϰ� ������ Value��. class�� Reference �����̴�.// �޸𸮰� ����Ǵ� ������ �ٸ� ��(stack�� �� / heap�� �ּ�)
// Ŭ������ ����ü�� �޸� ���ÿ� �Ҵ��� �ȵ��־ new Ű���带 ���� ���� �Ҵ��ϰ� �� �ּҰ��� �����Ѵ�. ����ü�� �Ҵ���� �ٷ� ���
// ����ü�� ������ ���ؿ� ����Ǽ� ���� ���� ������ ��ȭ�� �Ǳ� ������ ���� ���� ��������. Ŭ������ ���ο� ���� ������ ���� ���� ����Ű�� ���̱� ������ �������� �Բ� ����.
// C#�� ���� �Ҵ�� �޸𸮸� �������÷��Ͷ�� �༮�� ������� �ʴ� �༮���� �������ִµ� �̶� ó���Ұ� ������ ���α׷� ���� �ӵ��� �������ų� �ƿ� ����.
// �׷��ϱ� ���� ���� �Ҵ����� �ʾƵ� �Ǵ� ���� ���� �޸𸮿��� ����ؼ� ���α׷� �ӵ��� ����Ű�µ� ������ �ش�. ������ ���� �޸𸮸� ������ڴ� �޸𸮿� ������ ������ ���ؿ����÷ο찡 �߻��� �� ������ ������ �������� ���!
// ����ü ��� ���� : �ν���Ʈ ũ�� 16����Ʈ, ������� ����, ���� �߰��� �ʿ����, �⺻����(int�� double���� �͵�)�� ������ ���ϰ��� ���(�������� ��Ÿ��?) // 16����Ʈ �Ѿ�� ���� �Ҵ�ȵ� 
// �ݺ��� �ȿ� �ӽð�ü�� ����Ҷ� ������. gc�������� scope������ ���� .. class������ �����ϸ� ���� �� ����
