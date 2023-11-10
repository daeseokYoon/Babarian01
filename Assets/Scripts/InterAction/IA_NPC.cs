using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_NPC : MonoBehaviour, IInteractable
{
    [SerializeField] string _prompt; // �̰Ÿ� ���ȭ ���Ѽ� �������� �������� �������߰ڳ�. SO
    public string InteractionPrompt => _prompt;

    [SerializeField] DialogueSO dialog; // ���� ������Ʈ�� ����� ���� �ٸ� �α� / ���� �ٲ۴ٸ� ���⸸ SO�𵨷� ��ũ��Ʈ�� �ٲ��ָ� �ɵ�. �׸��� ShowDialog �Լ� �Ķ���� �ٲ��ֱ�

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
        DialogManager.instance.ShowDialogue(dialog);
    }
}
