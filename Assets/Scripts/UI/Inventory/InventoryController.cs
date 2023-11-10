using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
// UIPage에서 자체적으로 Awake를 사용하기도 했지만 전체적으로 코드가 움직이게 하는 장소는 컨트롤러의 Update 위치이다. 게다가 전부 참조가 최소 하나씩으로 걸려있음
// 이 말은 함수가 캡슐화 되어서 엄청나게 나뉘어져 있다는 뜻이고 각 메소드 안에 메소드 안에 메소드가 사용되어서 하나로 깔끔하게 보이게 된다. + 데이터의 유기적인 연결을 위한 함수들에 컨트롤러
namespace Inventory
{
    public class InventoryController : MonoBehaviour 
    {
        [SerializeField] UIInventoryPage inventoryUI;                                                          // 똑같은 이름을 가진 스크립트가 있어서 네임스페이스 using이 안됬던 거임.
        [SerializeField] InventorySO inventoryData;

        public List<InventoryItem> initialItems = new List<InventoryItem>();                                  // 초기 아이템이 새 list와 동일하게 하려고

        [SerializeField] AudioClip dropClip;
        [SerializeField] AudioSource audioSource;


        private void Start()
        {
            PrepaerUI();
            PrepareInventoryData();
        }

        private void PrepareInventoryData()
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            foreach (InventoryItem item in initialItems)
            {
                if(item.IsEmpty) continue;                                                                       // 인벤에 빈항목을 넣을 예정은 아니지만.. 테스트용
                inventoryData.AddItem(item);                                                                    // 처음 저장되있는 기본값을 불러오는 AddItem
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItem();
            foreach (var item in inventoryState)                                                                  // dictionary라서 var
            {
                inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
            }
        }

        private void PrepaerUI()
        {
            inventoryUI.InitializeInventoryUI(inventoryData.Size);                                                  // 호출하고 초기화
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnSwapItems += HandleSwapItems;
            inventoryUI.OnStartDragging += HandleDragging;
            inventoryUI.OnItemActionRequested += HandleItemActionRequest;
        }

        private void HandleItemActionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty) return;

            IItemAction itemAction = inventoryItem.item as IItemAction;                                              // 다시 인벤에 추가
                                                                                                                      // 구조체를 인터페이스에 참조한 코드 // 값 형식으로 스택에 저장된 것을 참조 형식의 인터페이스에 할당하려면
                                                                                                                      // 박싱 작업 필요(구조체를 힙에 복사해서 참조형식으로 다루는 방식) // 구조체 인스턴스 생성new, 형변환 비슷
                                                                                                                      // 위 작업에서 as 연산자는 성능상 오버헤드가 발생할 수 있고 복사본이 만들어짐 아래 코드로 변환을 하지만
                                                                                                                      // 박싱이 일반적인 방법임. 형변환처럼 쓰는 방식을 말함.
            if (itemAction != null)
            {
                inventoryUI.ShowItemAction(itemIndex);
                                                                                                                         //itemAction.PerformAction(gameObject, inventoryItem.itemState);
                                                                                                                         //// 아이템 상태에 따라 none or 기본 상태 변경
                inventoryUI.AddAction(itemAction.ActionName, () => PerformAction(itemIndex));
                                                                                                                        // 실제로 performAction이 파라미터를 action 대리자에 전달하지 않기 때문에 람다식으로 호출함.

            }

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;                                  // 장착된 것을 지우고
            if(destroyableItem != null)
            {
                inventoryUI.AddAction("버리다", () => DropItem(itemIndex, inventoryItem.quantity));
                                                                                                                         //inventoryData.RemoveItem(itemIndex, 1);
            }
        }

        private void DropItem(int itemIndex, int quantity)
        {
            inventoryData.RemoveItem(itemIndex, quantity);
            inventoryUI.ResetSelection();
            audioSource.PlayOneShot(dropClip);
        }

        public void PerformAction(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty) return;

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem; 
            if (destroyableItem != null)
            {
                inventoryData.RemoveItem(itemIndex, 1);
            }

            IItemAction itemAction = inventoryItem.item as IItemAction; 
            
            if (itemAction != null)
            {
                itemAction.PerformAction(gameObject, inventoryItem.itemState);
                audioSource.PlayOneShot(itemAction.actionSFX);
                if (inventoryData.GetItemAt(itemIndex).IsEmpty)
                    inventoryUI.ResetSelection();
            }
        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if(inventoryItem.IsEmpty) return;
            inventoryUI.CreateDraggedItem(inventoryItem.item.ItemImage, inventoryItem.quantity);
        }

        private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
        {
           inventoryData.SwapItems(itemIndex_1, itemIndex_2);
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                inventoryUI.ResetSelection();
                return;
            }
            ItemSO item = inventoryItem.item;
            string description = PrePareDescription(inventoryItem);
            inventoryUI.UpdateDescription(itemIndex, item.ItemImage, item.name, description);
        }

        private string PrePareDescription(InventoryItem inventoryItem)
        {
            StringBuilder sb = new StringBuilder();                                                                                  // 문자열 빌더 system.txt 문자열을 효율적으로 생성
            sb.Append(inventoryItem.item.Description);
            sb.AppendLine();                                                                                                                 // 새 줄 생성
            for(int i = 0; i < inventoryItem.itemState.Count; i++)
            {
                sb.Append($"{inventoryItem.itemState[i].itemParameter.ParameterName}" + 
                    $": {inventoryItem.itemState[i].value} / " + 
                    $"{inventoryItem.item.DefaultParametersList[i].value}");
            }
            return sb.ToString();
        }

        

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))                                                                                         // 클릭 버튼 추가 예정
            {
                if (inventoryUI.isActiveAndEnabled == false)
                {
                    inventoryUI.Show();
                    foreach (var item in inventoryData.GetCurrentInventoryState())
                    {
                        inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
                    }
                }
                else
                {
                    inventoryUI.Hide();
                }
            }
        }
    }
}