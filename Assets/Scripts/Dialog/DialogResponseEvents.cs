using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogResponseEvents : MonoBehaviour
{
    [SerializeField] DialogueSO dialogueSO;
    [SerializeField] ResponseEvent[] events;

    public ResponseEvent[] Events => events;

    public DialogueSO DialogueSO => dialogueSO;

    private void OnValidate() // 유효성 검사 : Inspector에서 스크립트의 프로퍼티 값이 수정될 때마다 호출되는 함수 // 다른 속성을 업데이트 하거나 속성이 수정될 때마다 유효성 검사를 해줌(인스펙터에서 속성을 수정시 자동 호출됨) 
                              // 예를 들어 인스펙터에서 값을 잘못 수정해도 값이 일정 범위가 넘어가지 못하게 조정할 수 있다. // RunTime에서 적용되는 것이 아니라 Editor 단계에서만 적용됨 
                              // 어떤 이벤트가 어떤 응답에 연결되어있는지 추적해서 확인할 수 있음.
    {
        if(dialogueSO == null) return;
        if(dialogueSO.Responses == null) return;
        if (events != null && events.Length == dialogueSO.Responses.Length) return;

        if(events == null)
        {
            events = new ResponseEvent[dialogueSO.Responses.Length];
        }
        else
        {
            Array.Resize(ref events, dialogueSO.Responses.Length);
        }

        for(int i = 0; i < dialogueSO.Responses.Length; i++)
        {
            Response reponse = dialogueSO.Responses[i];

            if (events[i] != null)
            {
                events[i].name = reponse.ResponseText;
                continue;
            }
            events[i] = new ResponseEvent() { name = reponse.ResponseText };
        }
    }
}
