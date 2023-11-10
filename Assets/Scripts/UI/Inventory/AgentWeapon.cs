using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Inventory.Model.ItemSO;

public class AgentWeapon : MonoBehaviour
{
    // ���������� ��ü�� ������ ������ �� �ִ� ������ �Ķ������ ���� List
    [SerializeField] EquippableItemSO weapon;
    [SerializeField] InventorySO inventoryData; // ���� ���� �������� ����
    [SerializeField] List<ItemParameter> parametersToModify, itemCurrentState;
    
    public void SetWeapon(EquippableItemSO weaponItemSO, List<ItemParameter> itemState) // ���� ������, �����ۻ���
    {
        if(weapon != null)
        {
            inventoryData.AddItem(weapon, 1, itemCurrentState);
        }

        this.weapon = weaponItemSO;
        this.itemCurrentState = new List<ItemParameter>(itemState); // ������ �Ķ������ new list�� �����ϴµ� �ʿ��� ����ü List ����
        ModifyParameters(); // �׸��� ������ ���� list�� ���ο� list�� ����
    }

    private void ModifyParameters()
    {
        foreach(var parameter in parametersToModify) // list�� �Ķ���͵� var�� 
        {
            if(itemCurrentState.Contains(parameter)) // ����ü �ȿ� �ش� �Ķ���Ͱ� �ִ��� Ȯ���ϴ� �޼ҵ�
            {
                int index = itemCurrentState.IndexOf(parameter); // �������� �Ķ���� index
                float newValue = itemCurrentState[index].value + parameter.value; // ���� �� + ��ȭ ��
                itemCurrentState[index] = new ItemParameter // ��ȭ�� �� ���� // ����ü �⺻���� ��ȭ ���� ���ο� ī���� ��
                {
                    itemParameter = parameter.itemParameter,
                    value = newValue,
                };
            }
        }
    }
}
