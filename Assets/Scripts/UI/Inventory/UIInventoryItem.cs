using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventoryItem : MonoBehaviour, 
    IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
{
    [SerializeField] Image itemImage;                                                                                                       // �巡�� �� ��� �ǵ��, ImageOnOff
    [SerializeField] TMP_Text quantityTxt;                                                                                               // ���� ����

    [SerializeField] Image borderImage; // ���� ����
                                                                                                                // �븮��(����, C#�븮��) - �κ��丮�� �ִ� �׸��� �ٸ� ��ũ��Ʈ�� ������ ������ �Ʒ� �̺�Ʈ �۾� �븮�ڷ� ���� ����
    public event Action<UIInventoryItem> OnItemClicked, OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag,           // �̺�Ʈ �׼��� ���� ���� ������ ������ Ŭ���� ���ο����� �� �� �ְ� ��.

        OnRightMouseBtnClick;
                                                                                                                 // �� �׸� �ش��ϴ� ����Ʈ�� �ε����� �������� ��Ȯ�� �𸣴� UIpage�� ���� List���� �ش��ϴ� ���� ã�� ����.


    bool empty = true;

    private void Awake()
    {
        ResetData();                                                                                                 // ������ �缳�� �� ���� ��� ȣ��
        Deselect();
    }
    public void ResetData()
    {
        itemImage.gameObject.SetActive(false);
        empty = true;
    }
    public void Deselect()
    {
        borderImage.enabled = false;
    }
    public void SetData(Sprite sprite, int quantity)
    {
        itemImage.gameObject.SetActive(true);
        itemImage.sprite = sprite;
        quantityTxt.text = quantity + "";
        empty = false;
    }
    public void Select()                                                                                             // �����ϸ� �̹��� ���
    {
        borderImage.enabled = true;
    }

    public void OnPointerClick(PointerEventData pointerData)                                                            // Ŭ���ϰ� ���� �����͸� Ȱ���ϴ� �޼ҵ� // invoke : ��ϵ� �̺�Ʈ �ڵ鷯���� ���� ��ü�� �����Ѵٴ� �ǹ�
    {
                                                                                                                         //PointerEventData pointerData = (PointerEventData)eventData; // �̷��� ��ȯ�� �ǳ�..
        if (pointerData.button == PointerEventData.InputButton.Right)                                                       // PC������ ������ �ڵ� 
                                                                                                                            // �ڵ������� �Ϸ��� �̺�Ʈ Ʈ���� ������Ʈ Ȱ���ؾ��� 
        {
            OnRightMouseBtnClick?.Invoke(this);                                                                          // ������ ����Ϸ��� Ŭ���ߴٴ� ���� �˷���
        }
        else
        {
            OnItemClicked?.Invoke(this);                                                                            // �⺻�� ���콺 ���� Ŭ��
        }
    }

    public void OnBeginDrag(PointerEventData eventData)                                                             // �׸��� �ִ��� ������ Ȯ�� �� 
    {
        if (empty) return;
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)                                                               // ��� �� �ƹ������� �巡�׸� �����ϸ� ���� ���·� �ǵ��� ���� 
                                                                                                                    // ������ �巡�� �µǰ� ������ ��� ���� 
    {
        OnItemEndDrag?.Invoke(this);
    }

    public void OnDrop(PointerEventData eventData)                                                                  // begindrag���� emtpy ������ ���� Ȯ�� �ϱ⶧���� emptyȮ�� ������
    {
        OnItemDroppedOn?.Invoke(this);                                                                                  // �����׸��� ����ϰ� ���� ����� ������Ʈ�� 
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }
}
