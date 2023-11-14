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

    private void OnValidate() // ��ȿ�� �˻� : Inspector���� ��ũ��Ʈ�� ������Ƽ ���� ������ ������ ȣ��Ǵ� �Լ� // �ٸ� �Ӽ��� ������Ʈ �ϰų� �Ӽ��� ������ ������ ��ȿ�� �˻縦 ����(�ν����Ϳ��� �Ӽ��� ������ �ڵ� ȣ���) 
                              // ���� ��� �ν����Ϳ��� ���� �߸� �����ص� ���� ���� ������ �Ѿ�� ���ϰ� ������ �� �ִ�. // RunTime���� ����Ǵ� ���� �ƴ϶� Editor �ܰ迡���� ����� 
                              // � �̺�Ʈ�� � ���信 ����Ǿ��ִ��� �����ؼ� Ȯ���� �� ����.
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
