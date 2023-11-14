using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_NPC : MonoBehaviour, IInteractable
{
    [SerializeField] string _prompt; // �̰Ÿ� ���ȭ ���Ѽ� �������� �������� �������߰ڳ�. SO
    public string InteractionPrompt => _prompt;

    [SerializeField] DialogueSO dialog; // ���� ������Ʈ�� ����� ���� �ٸ� �α� / ���� �ٲ۴ٸ� ���⸸ SO�𵨷� ��ũ��Ʈ�� �ٲ��ָ� �ɵ�. �׸��� ShowDialog �Լ� �Ķ���� �ٲ��ֱ�

    public void UpdateDialogueSO(DialogueSO dialog)
    {
        this.dialog = dialog;
    }

    public bool Interact(ObjectInteractor interactor)
    {
        Interact_Dialog(dialog);

        var inventory = interactor.GetComponent<Inventory1>();

        if (inventory == null) return false;

        if (inventory.HasGun)
        {
            Debug.Log("�� ��");
            return true;
        }
        Debug.Log("�� ��");
        return false;
    }

    public void Interact_Dialog(DialogueSO log)
    {
        dialog = log;

        foreach(DialogResponseEvents responseEv in GetComponents<DialogResponseEvents>())
        {
            DialogManager.instance.AddResponseEvents(responseEv.Events);
            break;
        }

        DialogManager.instance.ShowDialogue(dialog);

        //if (TryGetComponent(out DialogResponseEvents responseEvents) && responseEvents.DialogueSO == dialog) // ��ȭ ���� ��ġ���� Ȯ�� 
        //{
        //    DialogManager.instance.AddResponseEvents(responseEvents.Events);
        //}
    }
}
