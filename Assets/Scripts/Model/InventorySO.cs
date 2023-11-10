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
    public class InventorySO : ScriptableObject                                                                  // 인벤토리 자체도 자신이 가지고 있는 항목 수를 알아야함 
                                                                                                                // SO를 사용하여 인벤토리를 저장하는 장점 : 인스펙터에서 리스트를 확장가능 > 리스트에 있는 내용물과 수량도 확인가능

    {
        [SerializeField]
        List<InventoryItem> inventoryItems;                                                             // 아이템 목록에 대한 참조를 얻는 가장 빠른 방법은 전체 목록을 모두가져와서
                                                                                                         // 인벤토리 컨트롤러에 전달하고 이방식으로 UI 업데이트를 하려면 인벤토리 컨트롤러가 이 목록에 액세스해서 새값을 설정하고 수정
                                                                                                         // list는 참조유형 > list에 있는 힙에 참조를 전달하고 우리는 수정!

        [field: SerializeField] public int Size { get; private set; } = 10;                                         // 어떤 클래스에서도 사이즈의 크기가 변동되는 것을 원하지 않음

        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;                                         // 대리자로 딕셔너리를 인벤 아이템으로 전달 // 리스트의 인덱스를 key로 인벤 아이템 정보 찾기

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
                                                                                                                        // 이전에 작동한 코드가 작동해서 이미 사용한 스크립트에서 여기 참조를 변경할 필요없게 만들 필요있음
            if(item.IsStackable == false)
            {
                for (int i = 0; i < inventoryItems.Count; i++)                                                              // 인벤토리 크기 스캔한번 하고
                {
                    while(quantity > 0 && IsInventoryFull() == false)
                    {
                        quantity -= AddItemToFirstFreeSlot(item, 1, itemState);
                                                                                                                                //quantity --; // 위 함수가 int로 반환할거라서
                    }
                    InformAboutChange();
                    return quantity;
                }
            }
            quantity = AddStackableItem(item, quantity);                                                                    // 인벤이 열려있을때 아이템 추가시 추가된 아이템을 업데이트를 위해
                                                                                                                            // 이미 생성한 함수 변경에 대한 정보알림을 호출
            InformAboutChange();
            return quantity;
           
        }

        private int AddItemToFirstFreeSlot(ItemSO item, int quantity, List<ItemParameter> itemState = null)                              // 빈공간을 찾을게 아니라서 int로
        {
            InventoryItem newItem = new InventoryItem
            {
                item = item,
                quantity = quantity,
                itemState = new List<ItemParameter>(itemState == null ? item.DefaultParametersList : itemState)
                                                                                                                                    // 구조체를 새 목록에 복사 // 여기서는 인벤토리에 적용되는 유일한 것이라서
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
            //if (inventoryItems[i].IsEmpty) // 인벤토리가 비어있는 곳에
            //{
            //    inventoryItems[i] = new InventoryItem // 들어온 아이템을 넣는다 // 새로운 값을 넣는거임.
            //    {
            //        item = item,
            //        quantity = quantity,
            //    };
            //    return; // 빈 항목을 찾고 반환
            //}
        }

        private bool IsInventoryFull() => inventoryItems.Where(item => item.IsEmpty).Any() == false;
                                                                                                                            // 인벤 아이템 목록에 채워져잇는 아이템이 하나 이상 있다면 true 아니면 false // where / linq

        private int AddStackableItem(ItemSO item, int quantity)                                                             // 최대 용량이 99이고 현재 갯수가 98개일때 1개만 더 가져갈 수 있게하는 코드
        {
            for(int i =0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty) continue;
                if (inventoryItems[i].item.ID == item.ID)
                {
                    int amountPossibleToTake = inventoryItems[i].item.MaxStackSize - inventoryItems[i].quantity;                // 아이템 최대 스택 - 인벤아이템 자리 수 = 남은 스택

                    if(quantity > amountPossibleToTake)                                                                          // 수량이 가져갈 수 있는 수량보다 많을때 (남은 양보다 슬롯에 들어갈 아이템이 더 많다는 뜻)
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].item.MaxStackSize);           // 인벤토리 아이템 창 다 채워짐
                        quantity -= amountPossibleToTake;                                                                                // 여기서 수량은 가져가고 남은 양 // 남은 양은 반품 혹은 새로운 인벤토리 창에 추가
                    }
                    else
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].quantity + quantity);             // 인벤토리 아이템 창 추가
                        InformAboutChange();
                        return 0;                                                                                                // 남는 것 없이 전부 인벤에 넣었음.
                    }
                }
            }
            while(quantity > 0 && IsInventoryFull() == false)                                                       // 수량이 여전히 0보다 큰 상태이면서 인벤이 풀이 아닐 동안 quantity 수량이 감소하고 빼고 남은 자리(수량)을 반환
            {
                int newQuantity = Mathf.Clamp(quantity, 0, item.MaxStackSize);
                quantity -= newQuantity;
                AddItemToFirstFreeSlot(item, newQuantity);                                                              // 새 항목을 생성하고 첫 번째 빈 슬롯에 추가하기 때문에
            }
            return quantity;
        }

                                                                                                                             //인벤토리가 null이 될 수 없으니 참조 값이 동일하도록 인벤토리를 설정가능해짐. 아래는 안됬던 것들
                                                                                                                             //InventoryItem item = new InventoryItem();
                                                                                                                             //int x = null;
                                                                                                                             //InventorySO val = null;

        public Dictionary<int, InventoryItem> GetCurrentInventoryState()                                                     // 모든 아이템이 채워지지 않으니 일부 항목은 비어있다고 할 것. 모든 아이템을 업데이트 할 필요없이
                                                                                                                             // 딕셔너리에 Key값으로 인덱스가 전달되지 않으면 이 항목은 자동으로 비어있다고 가정할 수 있음 
                                                                                                                             // 따라서 원하는 항목만 업데이트하고 나머지 항목은 UI에 없데이트 되지 않은채로 둔다.
        {
            Dictionary<int, InventoryItem> returnValue = new Dictionary<int, InventoryItem>();
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty) continue;
                returnValue[i] = inventoryItems[i];
            }
            return returnValue;
        }

        public InventoryItem GetItemAt(int itemIndex)                                                                // 빈 아이템창을 반환할 수도 있지만 아래 ChangeQuantity에서 빈 아이템은 리턴함
        {
            return inventoryItems[itemIndex];
        }

        public void AddItem(InventoryItem item)                                                                         // namespace로 같은 이름을 사용할 수 있긴 한데 이렇게 하는 이유는? 
        {
            AddItem(item.item, item.quantity);                                                                           // 처음에 저장되어있는 기본 값을 불러오는 메소드(구조체에 저장된 정보를 불러오기 때문)
        }

        public void SwapItems(int itemIndex_1, int itemIndex_2)
        {
            InventoryItem item1 = inventoryItems[itemIndex_1];                                                               // 첫번째 아이템이 가져오는 아이템
            inventoryItems[itemIndex_1] = inventoryItems[itemIndex_2];
            inventoryItems[itemIndex_2] = item1;
            InformAboutChange();                                                                                             // 인벤토리 내용이 수정되고 변경사항이 있다는 것을 알아야함 // 그냥 변경 사항임 실제 바꾸는 메소드 아님
                                                                                                                            // UI에서 데이터를 저장하는게 아니라 인벤토리 컨트롤러로 UI를 수정함
        }

        private void InformAboutChange()                                                                                     // 현재 인벤토리의 상태 데이터를 전달
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
    public struct InventoryItem                                                                     // 다른 곳에서 class에 있는 인벤토리아이템 스크립트를 참조해서 사용했을때
                                                                                                    // 다른 스크립트에서 뭔가 수정이 일어날 경우 기존 인벤토리에 버그가 생기지 않도록 메모리를 다른 곳으로 설정하기위해 구조체를 사용함 
    {
        public int quantity;
        public ItemSO item;
        public List<ItemParameter> itemState;
        public bool IsEmpty => item == null;

        public InventoryItem ChangeQuantity(int newQuantity)
        {
            return new InventoryItem                                                                             // 아이템이나 인벤의 수량을 실제 수정할 수 없으니 새로운 항목을 만들어준다. 초기값은 따로 있음(참조형식이 아니기때문)
            {
                item = this.item,
                quantity = newQuantity,                                                                                      // 다른 속성을 추가할때 더 수정하기 쉽다는 이점??
                itemState = new List<ItemParameter>(this.itemState)                                                      // itemparameter는 쉽게 수정할 수 없어서 구조체로 함
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


// 구조체란 데이터와 관련 기능을 캡슐화할 수 있는 값 형식 > 클래스와 달리 상속이 불가능하고 형식이 Value다. class는 Reference 형식이다.// 메모리가 저장되는 공간이 다른 것(stack이 값 / heap이 주소)
// 클래스는 구조체와 달리 스택에 할당이 안되있어서 new 키워드를 통해 힙에 할당하고 그 주소값을 참조한다. 구조체는 할당없이 바로 사용
// 구조체는 값으로 스텍에 저장되서 새로 값을 넣으면 변화가 되긴 하지만 기존 값이 남아있음. 클래스는 새로운 값을 넣으면 같은 값을 가르키는 것이기 때문에 기존값도 함께 변함.
// C#은 힙에 할당된 메모리를 가비지컬렉터라는 녀석이 사용하지 않는 녀석들을 정리해주는데 이때 처리할게 많으면 프로그램 동작 속도가 느려지거나 아예 멈춤.
// 그러니까 굳이 힙에 할당하지 않아도 되는 것은 스택 메모리에서 사용해서 프로그램 속도를 향상시키는데 도움을 준다. 하지만 스텍 메모리만 사용하자니 메모리에 제한이 있으면 스텍오버플로우가 발생할 수 있으니 적절히 균형있이 사용!
// 구조체 사용 기준 : 인스턴트 크기 16바이트, 변경안할 거임, 자주 추가할 필요없음, 기본형식(int나 double같은 것들)과 유사한 단일값을 사용(논리적으로 나타냄?) // 16바이트 넘어가도 힙에 할당안됨 
// 반복문 안에 임시객체로 사용할때 쓸만함. gc관여없이 scope내에서 제거 .. class쓰듯이 참조하면 꼬일 수 있음
