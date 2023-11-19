using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ResponseHandler : MonoBehaviour
{
    [SerializeField] RectTransform responsePivot;
    [SerializeField] RectTransform responseBtnPrefab;
    [SerializeField] RectTransform reponseContainer;
    [SerializeField] RectTransform responseBox;
    

    ResponseEvent[] responseEvents;

    List<GameObject> tempResponseBtn = new List<GameObject>(); // ��ư ���

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        this.responseEvents = responseEvents; // this ���ָ� ���� �̸��̶� ��? �ظ��ϸ� �Ķ���� �̸��� �ٲ�����.
    }

    public void ResetBoxSize()
    {
        responseBtnPrefab.sizeDelta = new Vector2(responsePivot.sizeDelta.x, 50);
    }

    public void ShowResponse(Response[] responses)
    {
        responsePivot.gameObject.SetActive(true);
        //ResetBoxSize();
        float responseBoxHeight = 0;

        for (int i = 0; i < responses.Length; i++)
        {
            Response response = responses[i];
            int responseIndex = i; // onpick���� ����� ����.

            GameObject responseBtn = Instantiate(responseBtnPrefab.gameObject, reponseContainer);
            responseBtn.gameObject.SetActive(true);
            responseBtn.GetComponent<TMP_Text>().text = response.ResponseText;
            responseBtn.GetComponent<Button>().onClick.AddListener(() => OnPickedResponse(response, responseIndex));

            tempResponseBtn.Add(responseBtn);

            responseBoxHeight += responseBtnPrefab.sizeDelta.y;
        }
        //foreach (Response response in responses)
        //{
        //    GameObject responseBtn = Instantiate(responseBtnPrefab.gameObject, reponseContainer);
        //    responseBtn.gameObject.SetActive(true);
        //    responseBtn.GetComponent<TMP_Text>().text = response.ResponseText;
        //    responseBtn.GetComponent<Button>().onClick.AddListener(() => OnPickedResponse(response, 2));

        //    tempResponseBtn.Add(responseBtn);

        //    responseBoxHeight += responseBtnPrefab.sizeDelta.y;
        //}
        responseBox.sizeDelta = new Vector2(responseBox.sizeDelta.x, responseBoxHeight);
        responsePivot.gameObject.SetActive(true);
    }

    private void OnPickedResponse(Response response, int responseIndex)
    {
        responsePivot.gameObject.SetActive(false);                                                   // RectTransform�� gameObject ���̸� SetActive �� �� ����
        
        foreach(GameObject button in tempResponseBtn) // ������ ��ư�� ���� �����ֱ�
        {
            Destroy(button);
        }
        tempResponseBtn.Clear(); // List�� ���� �͵鵵 �� �����ֱ�

        if(responseEvents != null && responseIndex <= responseEvents.Length)
        {
            responseEvents[responseIndex].OnPickedResponse?.Invoke();
        }

        responseEvents = null; // �����̺�Ʈ ����

        if (response.DialogueSO) 
        {
            DialogManager.instance.ShowDialogue(response.DialogueSO);
        }
        else
        {
            DialogManager.instance.CloseDialogBox();
        }

        DialogManager.instance.ShowDialogue(response.DialogueSO);
    }

}
