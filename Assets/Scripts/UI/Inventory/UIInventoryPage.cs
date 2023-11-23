using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ���� UI �������� �ִ� ���� UI�� ��ũ��Ʈ�� �ϳ��� ����ִ� ��
namespace Inventory.UI
{
    public class UIInventoryPage : MonoBehaviour
    {
        [SerializeField] UIInventoryItem itemPrefab;

        [SerializeField] RectTransform contentPanel;

        [SerializeField] UIInventoryDescription itemDescription;

        [SerializeField] MouseFollower mouseFollower;

        List<UIInventoryItem> listOfUIItems = new List<UIInventoryItem>();                         // ������ ������ �׸� �ִ� ��ũ��Ʈ�� ������ �����ϰ� �Ʒ� ����� ���ؼ� �ε����� ��� ������
                                                                                                   // �κ��丮 �������� ������ �� �� �ְ� �ȴ�.

        int currentlyDraggedItemIndex = -1;                                                        // ��� ���� list�� �ִٴ� ������ �巡�� ���� ���� ����

        public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging;   // list�� ������ �ε��� �� // Dragging�Ҷ� �巡�׵� �׸�� �巡���� �׸� �ΰ��� �ε����� �����ؾ��� // �׷��� �Ʒ� swap���� ����
                                                                                                   // UIItemInventory�� ���� �����ؼ� ������ ������ ���� �ְ����� �⺻������ �κ��丮�� �ش� �ε����� ������ �ִ��� �Ű澲���ʰ� �׻� ������ ����

        public event Action<int, int> OnSwapItems;

        [SerializeField] ItemActionPanel actionPanel;

        private void Awake()
        {
            Hide();                                                                               // �ڵ����� ���������� ��ũ��Ʈ�� ����������.. // �׼� �гθ� Ȱ��ȭ ���¿��� hide�� �߰��� ��쵵 ����
            mouseFollower.Toggle(false);
            itemDescription.ResetDescription();
        }

        public void InitializeInventoryUI(int inventorysize)                                        // ������ �κ� ũ�⸸ŭ �ν���Ʈ ����
        {
            for (int i = 0; i < inventorysize; i++)
            {
                UIInventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                uiItem.transform.SetParent(contentPanel);
                listOfUIItems.Add(uiItem);

                uiItem.OnItemClicked += HandleItemSelection;                                     // += ��ȣ ǥ�÷� handle�� �߰� // 
                uiItem.OnItemBeginDrag += HandleBeginDrag;
                uiItem.OnItemDroppedOn += HandleSwap;
                uiItem.OnItemEndDrag += HandleEndDrag;
                uiItem.OnRightMouseBtnClick += HandleShowItemActions;                            // �Ϲ������� �̷� �̺�Ʈ �ڵ鷯�� ����� ���� �ش� �޼ҵ�� ������ ������ �Ķ���͸� �޴� �޼ҵ峪
                                                                                                 // ��������Ʈ�� ����ϴ� ���� �Ϲ��������� C#6.0�̻󿡼� ���ٽİ� �͸� �޼ҵ�� �Ķ���� ���� �޼ҵ� ȣ�� ����
                                                                                                 // �̹� �̺�Ʈ �ڵ鷯�� �ҷ��Ա� ������ ȣ���Ҷ� �ʿ��� �Ķ���͸� ������. �޼ҵ� �ñ״�ó�� ��ġ�ϴ� ���� ����
                                                                                                 // But �޼ҵ� ���ο� ������ �ʴ� �Ķ���͸� �����ٸ� �� �ڵ忡�� �ش� �Ķ���͸� ������� �ʵ��� ����

            }
        }

        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)               // �κ��丮���� ���� ȣ�� // UIData
        {
            if (listOfUIItems.Count > itemIndex)                                                // ������ ��� ���� ���� ��
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
            OnItemActionRequested?.Invoke(index);                                               // why Invoke �ΰ�
        }

        private void HandleEndDrag(UIInventoryItem inventoryItemUI)
        {
            ResetDraggedItem();                                                                 // �Ʒ� �ڵ齺���� �ص�巡������ �߻��ϱ� ����
        }

        private void HandleSwap(UIInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);                                 // �巡�׵� �׸��� �ٲ�鼭 �ȷξ �پ��ִ� ������Ʈ�� ��Ȱ��ȭ�ǰ� �ε����� -1�� �Ǹ鼭 �ű涧 ���� ���빰 �ʱ�ȭ
            if (index == -1)              
            {
                return;
            }
            OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
                                                                                                  // �Ʒ��� �巡���� �������� ������ ���õ� ���·� ������ ���̵��� �� // ���û���
            HandleItemSelection(inventoryItemUI);                                                 // ������ ���󰡸� ���⼭ �ε����� ���޹ް� �ۼ��� ��û�� �ִ����� ����
                                                                                                  // �������� �����͸� �������� ����ִ��� Ȯ�� �� ������ ������Ʈ�ϰ� 
                                                                                                  // ������ �����ϰ� ��� ������ ������ ����ϰ� ������ �������� �����Ѵ�
                                                                                                  // �ణ�ΰ� �̰�? ������ �̷����ϸ� �κ��丮 ��Ʈ�ѷ�, ������, �����Ͱ�
                                                                                                  // ���� �� �ʿ���� ������ ��ȯ��
        }

        private void ResetDraggedItem()
        {
            mouseFollower.Toggle(false);
            currentlyDraggedItemIndex = -1;
        }

        private void HandleBeginDrag(UIInventoryItem inventoryItemUI)
        {
            int index = listOfUIItems.IndexOf(inventoryItemUI);                                   // IndexOf > �迭���� Ư�� ��ҳ� ���� ã�� �迭 �޼ҵ�
            if (index == -1) return;

            currentlyDraggedItemIndex = index;
            HandleItemSelection(inventoryItemUI);
            OnStartDragging?.Invoke(index);                                                      // ������ �½�ŸƮ���뿡 �ִ� �޼ҵ带 ����
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
            ResetSelection();                                                                  // ���õ� ����
            DeselectAllItems();                                                                 // ��� �׸� ���� ���

        }

        private void DeselectAllItems()
        {
            foreach (UIInventoryItem item in listOfUIItems)
            {
                item.Deselect();
            }
            actionPanel.Toggle(false);                                                          // ��ư ��� ����
        }

        public void ResetSelection()
        {   
            itemDescription.ResetDescription();                                                  // �޴��� �ٽ� ���� ������ ä�������� ������ �缳���� ������.
            DeselectAllItems();                                                                      // ���� �����Ҷ� ��� �Բ� ����
        }

        public void AddAction(string actionName, Action performAction)                              // ������ ��ư�� ���� �۵��ϰ� �Ϸ��� �޼ҵ�
        {
            actionPanel.AddButton(actionName, performAction);
        }

        public void ShowItemAction(int itemIndex)                                                    // ������ ���� ��ư ��� on + ��ġ
        {
            actionPanel.Toggle(true);
            actionPanel.transform.position = listOfUIItems[itemIndex].transform.position;
        }

        public void Hide()
        {
            actionPanel.Toggle(false);
            gameObject.SetActive(false);
            ResetDraggedItem();                                                                          // �ʱ�ȭ
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
   
