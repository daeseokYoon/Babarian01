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
    [SerializeField] Image itemImage;                                                                                                       // 드래그 앤 드롭 피드백, ImageOnOff
    [SerializeField] TMP_Text quantityTxt;                                                                                               // 갯수 조정

    [SerializeField] Image borderImage; // 선택 유무
                                                                                                                // 대리자(람다, C#대리자) - 인벤토리에 있는 항목을 다른 스크립트로 정보를 보낼때 아래 이벤트 작업 대리자로 쉽게 보냄
    public event Action<UIInventoryItem> OnItemClicked, OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag,           // 이벤트 액션을 쓰면 오직 변수를 선언한 클래스 내부에서만 쓸 수 있게 됨.

        OnRightMouseBtnClick;
                                                                                                                 // 각 항목에 해당하는 리스트의 인덱스가 무엇인지 정확히 모르니 UIpage에 만든 List에서 해당하는 것을 찾을 것임.


    bool empty = true;

    private void Awake()
    {
        ResetData();                                                                                                 // 데이터 재설정 및 선택 취소 호출
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
    public void Select()                                                                                             // 선택하면 이미지 출력
    {
        borderImage.enabled = true;
    }

    public void OnPointerClick(PointerEventData pointerData)                                                            // 클릭하고 얻은 데이터를 활용하는 메소드 // invoke : 등록된 이벤트 핸들러에게 현재 객체를 전달한다는 의미
    {
                                                                                                                         //PointerEventData pointerData = (PointerEventData)eventData; // 이렇게 변환이 되네..
        if (pointerData.button == PointerEventData.InputButton.Right)                                                       // PC에서만 가능한 코드 
                                                                                                                            // 핸드폰으로 하려면 이벤트 트리거 컴포넌트 활용해야함 
        {
            OnRightMouseBtnClick?.Invoke(this);                                                                          // 아이템 사용하려고 클릭했다는 것을 알려줌
        }
        else
        {
            OnItemClicked?.Invoke(this);                                                                            // 기본은 마우스 왼쪽 클릭
        }
    }

    public void OnBeginDrag(PointerEventData eventData)                                                             // 항목이 있는지 없는지 확인 후 
    {
        if (empty) return;
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)                                                               // 경계 밖 아무곳에서 드래그를 중지하면 이전 상태로 되돌릴 거임 
                                                                                                                    // 원래면 드래그 온되고 아이템 사용 예정 
    {
        OnItemEndDrag?.Invoke(this);
    }

    public void OnDrop(PointerEventData eventData)                                                                  // begindrag에서 emtpy 유무를 먼저 확인 하기때문에 empty확인 안했음
    {
        OnItemDroppedOn?.Invoke(this);                                                                                  // 현재항목을 취소하고 새로 드롭한 오브젝트를 
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }
}
