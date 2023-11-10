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

    List<GameObject> tempResponseBtn = new List<GameObject>(); // ��ư ���

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
        responseBox.gameObject.SetActive(false);                                                   // RectTransform�� gameObject ���̸� SetActive �� �� ����
        
        foreach(GameObject button in tempResponseBtn) // ������ ��ư�� ���� �����ֱ�
        {
            Destroy(button);
        }
        tempResponseBtn.Clear(); // List�� ���� �͵鵵 �� �����ֱ�

        DialogManager.instance.ShowDialogue(response.DialogueSO);
    }

}
