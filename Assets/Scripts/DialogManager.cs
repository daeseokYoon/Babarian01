using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using KoreanTyper;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.VersionControl;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEditor.Animations;
using UnityEditor;

public class DialogManager : MonoBehaviour // �̰� Interaction �̶� promptedUI �����ϰ� �����ؼ� �ݴ�� npc�� ����� dialogprompt�� �޾Ƽ� dialogtext�� ��µǰ� �������Ѵ�. 
                                           // ���� ���´� �����ؼ� �޴°� �ƴ� �״�� ����� ���� // Que ������ ���� �̿��ؼ� ���̾�α� �ۼ�����
                                           // ���̾�α׸� �Ŵ����� �������ϴ°�? Ÿ����ȿ���� ��Ʈ�ѷ������� �ϴ� ��ũ��Ʈ�� �ۼ����ָ� �Ǵµ�? 
                                           // �Ŵ��� ���¶�⺸�� �������� ��Ʈ�ѷ��� ������ ����� ��ũ��Ʈ... Update�� ���� ObjectInteractor���� Update�� �þҴ�.
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] RectTransform reponseOptionPos; // UI�� ��ġ�� �־ transform ��ſ� rect��� �� �ٵ� transform���� �� �޼ҵ忡�� ������ ���� �ʴ´�. ��¡?
    [SerializeField] GameObject reponseBox;

    [SerializeField] ResponseHandler responseHandler;

    bool isDialogActive = false;
    public static DialogManager instance { get; private set; }

    private void Awake()
    {
        instance = this;
        CloseDialogBox();
    }

    public void ShowDialogue(DialogueSO dialog)
    {
        dialogBox.SetActive(true);
        reponseBox.SetActive(false);
        dialogText.text = "";
        isDialogActive = true;
        StartCoroutine(ShowDialog(dialog));
    }


    // �Ʒ� Ÿ���� ȿ���� ���� �̾Ƽ� ��ũ��Ʈ�� �����
    IEnumerator ShowDialog(DialogueSO dialog) // �ƹ��� �����ص� �Ŵ����� �� �� �ƴϰ� ���̾�α� UI�� �ٷ�� ��ũ��Ʈ�� ���� ���� �ű⼭ ����ǰԲ� �ؾ��� �� ���� // ������ ������Ʈ�� ������ ��ȭ��ϸ� �ҷ��� �����̴�.
    {
        for (int i = 0; i < dialog.Dialogue.Length; i++)
        {
            StartCoroutine(TypeDialog(dialog.Dialogue[i]));
            yield return new WaitForSeconds(1.0f);

            if (i == dialog.Dialogue.Length - 1 && dialog.HasResponses) break;

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space)); // �����̽� ������ ���� �������� �Ѿ��
        }

        if (dialog.HasResponses)
        {
            responseHandler.ShowResponse(dialog.Responses);
        }
        else
        {
            CloseDialogBox(); // for�� �� ���ư����� dialogUI �ݱ�
        }
    }

    IEnumerator TypeDialog(string dialog) // �ѱ� Ÿ���� ǥ�� ����
    {       
        int typingLength = dialog.GetTypingLength(); // GetTypingLength() : �ѱ� Ÿ���� Ƚ���� ��ȯ�ϴ� int �Լ�

        for (int i = 0; i < typingLength; i++)
        {
            dialogText.text = dialog.Typing(i); // ù��° ���� ������ Ÿ���α��� �Էµ�

            yield return new WaitForSeconds(0.0001f); // �Է� �ӵ�

            if (Input.GetKeyDown(KeyCode.Space)) // ������ ��ٸ� ��ŵ
            {
                dialogText.text = dialog;
                break;
            }

            yield return new WaitForSeconds(0.0005f); // ������ ���� ����
        }
    }

    public void CloseDialogBox()
    {
        dialogBox.SetActive(false);
        dialogText.text = string.Empty;
        isDialogActive = false;
        ResetResponseBox(reponseOptionPos);
    }

    void ResetResponseBox(Transform parent)
    {


        Transform[] children = new Transform[parent.childCount];
        for (int i = 0; i < parent.childCount; i++)
        {
            children[i] = parent.GetChild(i);
        }
        foreach(Transform child in children)
        {
            Destroy(child.gameObject);
        }
    }

    public bool IsDialogActive() => isDialogActive;

    //  if (dialogBox != null) { yield break; } �̰� ������ dialogBox�� �ν���Ʈ�� �����ϴ��� �ƴϸ� IEnumerator�� ���� ����� �ϰ� ������ ��� �ƴ�
}
