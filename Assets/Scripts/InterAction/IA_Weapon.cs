using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_Weapon : MonoBehaviour, IInteractable
{
    [SerializeField] string _prompt;
    public string InteractionPrompt => _prompt;

    public bool Interact(ObjectInteractor interactor) // ��� ������ �����Ǹ� ���⸦ ���� �� �ְ� �ڵ带 �ۼ��ϰ� true�� ��ȯ�� ���� �ִ�.
    {
       Debug.Log("���� �ݱ�");
       return true;
    }

    public void Interact_Dialog(DialogueSO log)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
