using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ResponseHandler : MonoBehaviour
{
    [SerializeField] RectTransform responseBox;
    [SerializeField] RectTransform responseBtnPrefab;
    [SerializeField] RectTransform reponseContainer;

    List<GameObject> tempResponseBtn = new List<GameObject>(); // 버튼 목록

    public void ResetBoxSize()
    {
        responseBtnPrefab.sizeDelta = new Vector2(responseBox.sizeDelta.x, 50);
    }

    public void ShowResponse(Response[] responses)
    {
        responseBox.gameObject.SetActive(true);
        ResetBoxSize();
        float responseBoxHeight = 0;

        foreach(Response response in responses)
        {
            GameObject responseBtn = Instantiate(responseBtnPrefab.gameObject, reponseContainer);
            responseBtn.gameObject.SetActive(true);
            responseBtn.GetComponent<TMP_Text>().text = response.ResponseText;
            responseBtn.GetComponent<Button>().onClick.AddListener(()=> OnPickedResponse(response));

            tempResponseBtn.Add(responseBtn);

            responseBoxHeight += responseBtnPrefab.sizeDelta.y;
        }
        responseBox.sizeDelta = new Vector2(responseBox.sizeDelta.x, responseBoxHeight);
        responseBox.gameObject.SetActive(true);
    }

    private void OnPickedResponse(Response response)
    {
        responseBox.gameObject.SetActive(false);                                                   // RectTransform에 gameObject 붙이면 SetActive 쓸 수 있음
        
        foreach(GameObject button in tempResponseBtn) // 생성한 버튼들 전부 지워주기
        {
            Destroy(button);
        }
        tempResponseBtn.Clear(); // List에 넣은 것들도 다 지워주기

        DialogManager.instance.ShowDialogue(response.DialogueSO);
    }

}
