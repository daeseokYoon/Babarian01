using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_NPC : MonoBehaviour, IInteractable
{
    [SerializeField] string _prompt; // 이거를 모듈화 시켜서 여러가지 선택지를 만들어줘야겠네. SO
    public string InteractionPrompt => _prompt;

    [SerializeField] DialogueSO dialog; // 개별 오브젝트에 저장된 서로 다른 로그 / 만약 바꾼다면 여기만 SO모델로 스크립트를 바꿔주면 될듯. 그리고 ShowDialog 함수 파라미터 바꿔주기

    public bool Interact(ObjectInteractor interactor)
    {
        Interact_Dialog(dialog);

        var inventory = interactor.GetComponent<Inventory1>();

        if (inventory == null) return false;

        if (inventory.HasGun)
        {
            Debug.Log("쏴 봐");
            return true;
        }
        Debug.Log("뭘 봐");
        return false;
    }

    public void Interact_Dialog(DialogueSO log)
    {
        dialog = log;
        DialogManager.instance.ShowDialogue(dialog);
    }
}
