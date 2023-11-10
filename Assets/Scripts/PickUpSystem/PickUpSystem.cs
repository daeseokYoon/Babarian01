using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField] InventorySO inventoryData;

    private void OnTriggerEnter(Collider other)
    {
        Item item = other.GetComponent<Item>();
        if(item != null)
        {
            int reminder = inventoryData.AddItem(item.InventoryItem, item.Quantity);
            if (reminder == 0) item.DestroyItem(); // �� ������ 0�̸� �ı�
            else item.Quantity = reminder;
        }
    }
}
