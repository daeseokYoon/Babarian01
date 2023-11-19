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

    List<GameObject> tempResponseBtn = new List<GameObject>(); // 버튼 목록

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        this.responseEvents = responseEvents; // this 써주면 같은 이름이라도 됨? 왠만하면 파라매터 이름을 바꿔주자.
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
            int responseIndex = i; // onpick에서 써먹을 거임.

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
        responsePivot.gameObject.SetActive(false);                                                   // RectTransform에 gameObject 붙이면 SetActive 쓸 수 있음
        
        foreach(GameObject button in tempResponseBtn) // 생성한 버튼들 전부 지워주기
        {
            Destroy(button);
        }
        tempResponseBtn.Clear(); // List에 넣은 것들도 다 지워주기

        if(responseEvents != null && responseIndex <= responseEvents.Length)
        {
            responseEvents[responseIndex].OnPickedResponse?.Invoke();
        }

        responseEvents = null; // 응답이벤트 리셋

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
