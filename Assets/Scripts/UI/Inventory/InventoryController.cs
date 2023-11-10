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
// UIPage���� ��ü������ Awake�� ����ϱ⵵ ������ ��ü������ �ڵ尡 �����̰� �ϴ� ��Ҵ� ��Ʈ�ѷ��� Update ��ġ�̴�. �Դٰ� ���� ������ �ּ� �ϳ������� �ɷ�����
// �� ���� �Լ��� ĸ��ȭ �Ǿ ��û���� �������� �ִٴ� ���̰� �� �޼ҵ� �ȿ� �޼ҵ� �ȿ� �޼ҵ尡 ���Ǿ �ϳ��� ����ϰ� ���̰� �ȴ�. + �������� �������� ������ ���� �Լ��鿡 ��Ʈ�ѷ�
namespace Inventory
{
    public class InventoryController : MonoBehaviour 
    {
        [SerializeField] UIInventoryPage inventoryUI;                                                          // �Ȱ��� �̸��� ���� ��ũ��Ʈ�� �־ ���ӽ����̽� using�� �ȉ�� ����.
        [SerializeField] InventorySO inventoryData;

        public List<InventoryItem> initialItems = new List<InventoryItem>();                                  // �ʱ� �������� �� list�� �����ϰ� �Ϸ���

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
                if(item.IsEmpty) continue;                                                                       // �κ��� ���׸��� ���� ������ �ƴ�����.. �׽�Ʈ��
                inventoryData.AddItem(item);                                                                    // ó�� ������ִ� �⺻���� �ҷ����� AddItem
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItem();
            foreach (var item in inventoryState)                                                                  // dictionary�� var
            {
                inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
            }
        }

        private void PrepaerUI()
        {
            inventoryUI.InitializeInventoryUI(inventoryData.Size);                                                  // ȣ���ϰ� �ʱ�ȭ
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnSwapItems += HandleSwapItems;
            inventoryUI.OnStartDragging += HandleDragging;
            inventoryUI.OnItemActionRequested += HandleItemActionRequest;
        }

        private void HandleItemActionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty) return;

            IItemAction itemAction = inventoryItem.item as IItemAction;                                              // �ٽ� �κ��� �߰�
                                                                                                                      // ����ü�� �������̽��� ������ �ڵ� // �� �������� ���ÿ� ����� ���� ���� ������ �������̽��� �Ҵ��Ϸ���
                                                                                                                      // �ڽ� �۾� �ʿ�(����ü�� ���� �����ؼ� ������������ �ٷ�� ���) // ����ü �ν��Ͻ� ����new, ����ȯ ���
                                                                                                                      // �� �۾����� as �����ڴ� ���ɻ� ������尡 �߻��� �� �ְ� ���纻�� ������� �Ʒ� �ڵ�� ��ȯ�� ������
                                                                                                                      // �ڽ��� �Ϲ����� �����. ����ȯó�� ���� ����� ����.
            if (itemAction != null)
            {
                inventoryUI.ShowItemAction(itemIndex);
                                                                                                                         //itemAction.PerformAction(gameObject, inventoryItem.itemState);
                                                                                                                         //// ������ ���¿� ���� none or �⺻ ���� ����
                inventoryUI.AddAction(itemAction.ActionName, () => PerformAction(itemIndex));
                                                                                                                        // ������ performAction�� �Ķ���͸� action �븮�ڿ� �������� �ʱ� ������ ���ٽ����� ȣ����.

            }

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;                                  // ������ ���� �����
            if(destroyableItem != null)
            {
                inventoryUI.AddAction("������", () => DropItem(itemIndex, inventoryItem.quantity));
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
            StringBuilder sb = new StringBuilder();                                                                                  // ���ڿ� ���� system.txt ���ڿ��� ȿ�������� ����
            sb.Append(inventoryItem.item.Description);
            sb.AppendLine();                                                                                                                 // �� �� ����
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
            if (Input.GetKeyDown(KeyCode.I))                                                                                         // Ŭ�� ��ư �߰� ����
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