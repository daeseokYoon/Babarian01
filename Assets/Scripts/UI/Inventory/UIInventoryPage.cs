using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 하위 UI 페이지에 있는 개별 UI의 스크립트도 하나로 모아주는 곳
namespace Inventory.UI
{
    public class UIInventoryPage : MonoBehaviour
    {
        [SerializeField] UIInventoryItem itemPrefab;

        [SerializeField] RectTransform contentPanel;

        [SerializeField] UIInventoryDescription itemDescription;

        [SerializeField] MouseFollower mouseFollower;

        List<UIInventoryItem> listOfUIItems = new List<UIInventoryItem>();                         // 생성된 아이템 항목에 있는 스크립트의 참조를 저장하고 아래 목록을 통해서 인덱스를 얻고 생성된
                                                                                                   // 인벤토리 데이터의 참조를 쓸 수 있게 된다.

        int currentlyDraggedItemIndex = -1;                                                        // 목록 밖의 list에 있다는 뜻으로 드래그 하지 않을 목적

        public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging;   // list로 생성된 인덱스 값 // Dragging할때 드래그된 항목과 드래그한 항목 두개의 인덱스를 전달해야함 // 그래야 아래 swap에서 받음
                                                                                                   // UIItemInventory에 직접 접근해서 정보를 가져올 수도 있겠지만 기본적으로 인벤토리는 해당 인덱스에 무엇이 있는지 신경쓰지않고 항상 참조할 예정

        public event Action<int, int> OnSwapItems;

        [SerializeField] ItemActionPanel actionPanel;

        private void Awake()
        {
            Hide();                                                                               // 자동으로 숨겨지지만 디스크립트도 마찬가지라.. // 액션 패널만 활성화 상태에서 hide를 추가할 경우도 있음
            mouseFollower.Toggle(false);
            itemDescription.ResetDescription();
        }

        public void InitializeInventoryUI(int inventorysize)                                        // 지정한 인벤 크기만큼 인스턴트 생성
        {
            for (int i = 0; i < inventorysize; i++)
            {
                UIInventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                uiItem.transform.SetParent(contentPanel);
                listOfUIItems.Add(uiItem);

                uiItem.OnItemClicked += HandleItemSelection;                                     // += 등호 표시로 handle을 추가 // 
                uiItem.OnItemBeginDrag += HandleBeginDrag;
                uiItem.OnItemDroppedOn += HandleSwap;
                uiItem.OnItemEndDrag += HandleEndDrag;
                uiItem.OnRightMouseBtnClick += HandleShowItemActions;                            // 일반적으로 이런 이벤트 핸들러를 등록할 때는 해당 메소드와 동일한 형식의 파라미터를 받는 메소드나
                                                                                                 // 델리게이트를 등록하는 것이 일반적이지만 C#6.0이상에서 람다식과 익명 메소드로 파라미터 없이 메소드 호출 가능
                                                                                                 // 이미 이벤트 핸들러로 불러왔기 때문에 호출할때 필요한 파라미터를 무시함. 메소드 시그니처가 일치하니 오류 없음
                                                                                                 // But 메소드 내부에 사용되지 않는 파라미터를 가진다면 그 코드에서 해당 파라미터를 사용하지 않도록 주의

            }
        }

        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)               // 인벤토리에서 직접 호출 // UIData
        {
            if (listOfUIItems.Count > itemIndex)                                                // 아이템 목록 내에 있을 때
            {
                listOfUIItems[itemIndex].SetData(itemImage, itemQuantity);
            }
        }

        private void HandleShowItemActions(UIInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1)
            {
                return;
            }
            OnItemActionRequested?.Invoke(index);                                               // why Invoke 인가
        }

        private void HandleEndDrag(UIInventoryItem inventoryItemUI)
        {
            ResetDraggedItem();                                                                 // 아래 핸들스왑이 앤드드래그이후 발생하기 때문
        }

        private void HandleSwap(UIInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);                                 // 드래그된 항목이 바뀌면서 팔로어에 붙어있는 오브젝트가 비활성화되고 인덱스가 -1이 되면서 옮길때 생긴 내용물 초기화
            if (index == -1)              
            {
                return;
            }
            OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
                                                                                                  // 아래는 드래그한 아이템이 여전히 선택된 상태로 설명이 보이도록 함 // 선택사항
            HandleItemSelection(inventoryItemUI);                                                 // 순서를 따라가면 여기서 인덱스를 전달받고 템설명 요청이 있는지에 따라
                                                                                                  // 아이템의 데이터를 가져오고 비어있는지 확인 후 설명을 업데이트하고 
                                                                                                  // 설명을 설정하고 모든 아이템 선택을 취소하고 선택한 아이템을 선택한다
                                                                                                  // 약간인가 이게? 복잡함 이렇게하면 인벤토리 컨트롤러, 페이지, 데이터가
                                                                                                  // 서로 알 필요없이 정보를 교환함
        }

        private void ResetDraggedItem()
        {
            mouseFollower.Toggle(false);
            currentlyDraggedItemIndex = -1;
        }

        private void HandleBeginDrag(UIInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);                                   // IndexOf > 배열에서 특정 요소나 값을 찾는 배열 메소드
            if (index == -1) return;

            currentlyDraggedItemIndex = index;
            HandleItemSelection(inventoryItemUI);
            OnStartDragging?.Invoke(index);                                                      // 있으면 온스타트드라깅에 있는 메소드를 실행
        }

        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, quantity);

        }

        private void HandleItemSelection(UIInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);
            if (index == -1) return;
            OnDescriptionRequested?.Invoke(index);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            ResetSelection();                                                                  // 선택도 리셋
            DeselectAllItems();                                                                 // 모든 항목 선택 취소

        }

        private void DeselectAllItems()
        {
            foreach (UIInventoryItem item in listOfUIItems)
            {
                item.Deselect();
            }
            actionPanel.Toggle(false);                                                          // 버튼 토글 꺼짐
        }

        public void ResetSelection()
        {   
            itemDescription.ResetDescription();                                                  // 메뉴를 다시 열때 설명이 채워졌었기 때문에 재설정도 같이함.
            DeselectAllItems();                                                                      // 선택 리셋할때 토글 함께 꺼짐
        }

        public void AddAction(string actionName, Action performAction)                              // 생성된 버튼을 실제 작동하게 하려는 메소드
        {
            actionPanel.AddButton(actionName, performAction);
        }

        public void ShowItemAction(int itemIndex)                                                    // 아이템 선택 버튼 토글 on + 위치
        {
            actionPanel.Toggle(true);
            actionPanel.transform.position = listOfUIItems[itemIndex].transform.position;
        }

        public void Hide()
        {
            actionPanel.Toggle(false);
            gameObject.SetActive(false);
            ResetDraggedItem();                                                                          // 초기화
        }

        public void UpdateDescription(int itemIndex, Sprite itemImage, string name, string description)
        {
            itemDescription.SetDescription(itemImage, name, description);
            DeselectAllItems();
            listOfUIItems[itemIndex].Select();
        }

        internal void ResetAllItem()
        {
            foreach(var item in listOfUIItems)
            {
                item.ResetData();
                item.Deselect();
            }
        }
    }
}
   
