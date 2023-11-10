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

public class DialogManager : MonoBehaviour // 이거 Interaction 이랑 promptedUI 연구하고 참고해서 반대로 npc에 저장된 dialogprompt를 받아서 dialogtext에 출력되게 만들어야한다. 
                                           // 지금 형태는 참고해서 받는게 아닌 그대로 저장된 내용 // Que 데이터 구조 이용해서 다이어로그 작성배우기
                                           // 다이어로그를 매니저로 만들어야하는가? 타이핑효과는 컨트롤러역할을 하는 스크립트에 작성해주면 되는데? 
                                           // 매니저 형태라기보다 페이지와 컨트롤러가 합쳐진 모양의 스크립트... Update가 없고 ObjectInteractor에서 Update를 맡았다.
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] RectTransform reponseOptionPos; // UI에 위치해 있어서 transform 대신에 rect라고 씀 근데 transform으로 쓴 메소드에서 오류가 나지 않는다. 왜징?
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


    // 아래 타이핑 효과만 따로 뽑아서 스크립트로 만들기
    IEnumerator ShowDialog(DialogueSO dialog) // 아무리 생각해도 매니저에 둘 건 아니고 다이어로그 UI를 다루는 스크립트를 따로 만들어서 거기서 실행되게끔 해야할 것 같다 // 지금은 프롬프트에 지정한 대화목록만 불러온 형태이다.
    {
        for (int i = 0; i < dialog.Dialogue.Length; i++)
        {
            StartCoroutine(TypeDialog(dialog.Dialogue[i]));
            yield return new WaitForSeconds(1.0f);

            if (i == dialog.Dialogue.Length - 1 && dialog.HasResponses) break;

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space)); // 스페이스 눌러서 다음 문장으로 넘어가기
        }

        if (dialog.HasResponses)
        {
            responseHandler.ShowResponse(dialog.Responses);
        }
        else
        {
            CloseDialogBox(); // for문 다 돌아갔으면 dialogUI 닫기
        }
    }

    IEnumerator TypeDialog(string dialog) // 한글 타이핑 표현 수행
    {       
        int typingLength = dialog.GetTypingLength(); // GetTypingLength() : 한글 타이핑 횟수를 반환하는 int 함수

        for (int i = 0; i < typingLength; i++)
        {
            dialogText.text = dialog.Typing(i); // 첫번째 부터 마지막 타이핑까지 입력됨

            yield return new WaitForSeconds(0.0001f); // 입력 속도

            if (Input.GetKeyDown(KeyCode.Space)) // 문장이 길다면 스킵
            {
                dialogText.text = dialog;
                break;
            }

            yield return new WaitForSeconds(0.0005f); // 딜레이 버그 방지
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

    //  if (dialogBox != null) { yield break; } 이거 쓰려면 dialogBox를 인스턴트로 생성하던가 아니면 IEnumerator로 쓰지 마라고 하고 싶지만 사실 아님
}
